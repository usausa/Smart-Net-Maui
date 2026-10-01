namespace Smart.Maui.ViewModels;

using Smart.Maui.Input;
using Smart.Mvvm.ViewModels;

public sealed class ExtendViewModelBaseTests
{
    private sealed class TestViewModel : ExtendViewModelBase
    {
        public IObserveCommand Command { get; }

        public TestViewModel(IExtendViewModelOptions? options = null)
            : base(options)
        {
            Command = MakeDelegateCommand(static () => { });
        }

        public void RaiseChanged(string name) => RaisePropertyChanged(name);
    }

    private sealed class BehaviorViewModel : ExtendViewModelBase
    {
        public BehaviorViewModel(CommandBehavior behavior = CommandBehavior.Standard)
            : base(new ExtendViewModelOptions { BusyState = new BusyState(), CommandBehavior = behavior })
        {
        }

        public bool Accepts
        {
            get => AcceptsOperation;
            set => AcceptsOperation = value;
        }

        public IObserveCommand MakeDelegate(Action execute, CommandBehavior behavior = CommandBehavior.Default) =>
            MakeDelegateCommand(execute, behavior);

        public IObserveCommand MakeDelegateWithParameter(Action<int> execute, CommandBehavior behavior = CommandBehavior.Default) =>
            MakeDelegateCommand(execute, behavior);

        public IObserveCommand MakeAsync(Func<Task> execute, CommandBehavior behavior = CommandBehavior.Default) =>
            MakeAsyncCommand(execute, behavior);

        public IObserveCommand MakeAsyncWithParameter(Func<int, Task> execute, CommandBehavior behavior = CommandBehavior.Default) =>
            MakeAsyncCommand(execute, behavior);

        public void RaiseChanged(string name) => RaisePropertyChanged(name);
    }

    private static async Task WaitUntilAsync(Func<bool> condition)
    {
        for (var i = 0; (i < 100) && !condition(); i++)
        {
            await Task.Delay(10, TestContext.Current.CancellationToken).ConfigureAwait(true);
        }
    }

    //------------------------------------------------------------------
    // ViewModel
    //------------------------------------------------------------------

    [Fact]
    public void UpdatesCommandStateOnPropertyChangedByDefault()
    {
        // Arrange
        using var viewModel = new TestViewModel();
        var count = 0;
        viewModel.Command.CanExecuteChanged += (_, _) => count++;

        // Act
        viewModel.RaiseChanged("Any");

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void DoesNotUpdateCommandStateWhenAutoUpdateIsDisabled()
    {
        // Arrange
        using var viewModel = new TestViewModel(new ExtendViewModelOptions { AutoUpdateCommandState = false });
        var count = 0;
        viewModel.Command.CanExecuteChanged += (_, _) => count++;

        // Act
        viewModel.RaiseChanged("Any");

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void DefaultOptionsEnableAutoUpdate()
    {
        // Assert
        Assert.True(new ExtendViewModelOptions().AutoUpdateCommandState);
    }

    //------------------------------------------------------------------
    // Standard
    //------------------------------------------------------------------

    [Fact]
    public void StandardExecutesWithBusyState()
    {
        // Arrange
        using var viewModel = new BehaviorViewModel();
        var count = 0;
        var busy = false;
        var command = viewModel.MakeDelegate(() =>
        {
            count++;
            // ReSharper disable once AccessToDisposedClosure
            busy = viewModel.BusyState.IsBusy;
        });

        // Act
        command.Execute(null);

        // Assert
        Assert.Equal(1, count);
        Assert.True(busy);
        Assert.False(viewModel.BusyState.IsBusy);
    }

    [Fact]
    public void StandardSkipsWhenOperationNotAccepted()
    {
        // Arrange
        using var viewModel = new BehaviorViewModel();
        var count = 0;
        var command = viewModel.MakeDelegate(() => count++);
        var parameterCount = 0;
        var parameterCommand = viewModel.MakeDelegateWithParameter(_ => parameterCount++);
        viewModel.Accepts = false;

        // Act
        command.Execute(null);
        parameterCommand.Execute(1);

        // Assert
        Assert.Equal(0, count);
        Assert.Equal(0, parameterCount);
    }

    [Fact]
    public void StandardSkipsWhenBusy()
    {
        // Arrange
        using var viewModel = new BehaviorViewModel();
        var count = 0;
        var command = viewModel.MakeDelegate(() => count++);

        // Act
        using (viewModel.BusyState.Begin())
        {
            command.Execute(null);
        }

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task StandardAsyncHoldsBusyStateWhileExecuting()
    {
        // Arrange
        using var viewModel = new BehaviorViewModel();
        var completion = new TaskCompletionSource();
        var command = viewModel.MakeAsync(() => completion.Task);

        // Act
        command.Execute(null);

        // Assert
        Assert.True(viewModel.BusyState.IsBusy);

        // Act
        completion.SetResult();
        await WaitUntilAsync(() => !viewModel.BusyState.IsBusy);

        // Assert
        Assert.False(viewModel.BusyState.IsBusy);
    }

    //------------------------------------------------------------------
    // ControlByBusyState
    //------------------------------------------------------------------

    [Fact]
    public void ControlByBusyStateDisablesWhileBusy()
    {
        // Arrange
        using var viewModel = new BehaviorViewModel();
        var command = viewModel.MakeDelegate(static () => { }, CommandBehavior.ControlByBusyState);

        // Assert
        Assert.True(command.CanExecute(null));
        using (viewModel.BusyState.Begin())
        {
            Assert.False(command.CanExecute(null));
        }
    }

    [Fact]
    public void ControlByBusyStateSkipsWhenOperationNotAccepted()
    {
        // Arrange
        using var viewModel = new BehaviorViewModel();
        var count = 0;
        var command = viewModel.MakeDelegate(() => count++, CommandBehavior.ControlByBusyState);
        viewModel.Accepts = false;

        // Act
        command.Execute(null);

        // Assert
        Assert.Equal(0, count);
    }

    //------------------------------------------------------------------
    // Simple
    //------------------------------------------------------------------

    [Fact]
    public void SimpleExecutesRegardlessOfAcceptsOperationAndBusyState()
    {
        // Arrange
        using var viewModel = new BehaviorViewModel();
        var count = 0;
        var busy = true;
        var command = viewModel.MakeDelegate(() =>
        {
            count++;
            // ReSharper disable once AccessToDisposedClosure
            busy = viewModel.BusyState.IsBusy;
        }, CommandBehavior.Simple);
        var parameterCount = 0;
        var parameterCommand = viewModel.MakeDelegateWithParameter(_ => parameterCount++, CommandBehavior.Simple);
        viewModel.Accepts = false;

        // Act
        command.Execute(null);
        parameterCommand.Execute(1);

        // Assert
        Assert.Equal(1, count);
        Assert.False(busy);
        Assert.Equal(1, parameterCount);
    }

    [Fact]
    public void SimpleExecutesWhileBusy()
    {
        // Arrange
        using var viewModel = new BehaviorViewModel();
        var count = 0;
        var command = viewModel.MakeDelegate(() => count++, CommandBehavior.Simple);

        // Act
        using (viewModel.BusyState.Begin())
        {
            command.Execute(null);
        }

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void SimpleAsyncDoesNotHoldBusyState()
    {
        // Arrange
        using var viewModel = new BehaviorViewModel();
        var completion = new TaskCompletionSource();
        var command = viewModel.MakeAsync(() => completion.Task, CommandBehavior.Simple);
        var parameterCount = 0;
        var parameterCommand = viewModel.MakeAsyncWithParameter(_ =>
        {
            parameterCount++;
            return Task.CompletedTask;
        }, CommandBehavior.Simple);
        viewModel.Accepts = false;

        // Act
        command.Execute(null);
        parameterCommand.Execute(1);

        // Assert
        Assert.False(viewModel.BusyState.IsBusy);
        Assert.Equal(1, parameterCount);

        completion.SetResult();
    }

    [Fact]
    public void SimpleUpdatesCommandState()
    {
        // Arrange
        using var viewModel = new BehaviorViewModel();
        var command = viewModel.MakeDelegate(static () => { }, CommandBehavior.Simple);
        var count = 0;
        command.CanExecuteChanged += (_, _) => count++;

        // Act
        viewModel.RaiseChanged("Any");

        // Assert
        Assert.Equal(1, count);
    }

    //------------------------------------------------------------------
    // Default
    //------------------------------------------------------------------

    [Fact]
    public void DefaultUsesBehaviorOfOptions()
    {
        // Arrange
        using var viewModel = new BehaviorViewModel(CommandBehavior.Simple);
        var count = 0;
        var command = viewModel.MakeDelegate(() => count++);
        viewModel.Accepts = false;

        // Act
        command.Execute(null);

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void DefaultOptionsUseStandard()
    {
        // Assert
        Assert.Equal(CommandBehavior.Standard, new ExtendViewModelOptions().CommandBehavior);
    }
}

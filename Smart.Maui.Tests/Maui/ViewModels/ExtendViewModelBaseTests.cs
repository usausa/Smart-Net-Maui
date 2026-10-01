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

    private sealed class ModeViewModel : ExtendViewModelBase
    {
        public ModeViewModel(CommandMode mode = CommandMode.Standard)
            : base(new ExtendViewModelOptions { BusyState = new BusyState(), CommandMode = mode })
        {
        }

        public bool Accepts
        {
            get => AcceptsOperation;
            set => AcceptsOperation = value;
        }

        public IObserveCommand MakeDelegate(Action execute, CommandMode mode = CommandMode.Default) =>
            MakeDelegateCommand(execute, mode);

        public IObserveCommand MakeDelegateWithParameter(Action<int> execute, CommandMode mode = CommandMode.Default) =>
            MakeDelegateCommand(execute, mode);

        public IObserveCommand MakeAsync(Func<Task> execute, CommandMode mode = CommandMode.Default) =>
            MakeAsyncCommand(execute, mode);

        public IObserveCommand MakeAsyncWithParameter(Func<int, Task> execute, CommandMode mode = CommandMode.Default) =>
            MakeAsyncCommand(execute, mode);

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
        using var viewModel = new ModeViewModel();
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
        using var viewModel = new ModeViewModel();
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
        using var viewModel = new ModeViewModel();
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
        using var viewModel = new ModeViewModel();
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
        using var viewModel = new ModeViewModel();
        var command = viewModel.MakeDelegate(static () => { }, CommandMode.ControlByBusyState);

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
        using var viewModel = new ModeViewModel();
        var count = 0;
        var command = viewModel.MakeDelegate(() => count++, CommandMode.ControlByBusyState);
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
    public void SimpleExecutesWithoutBusyState()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var count = 0;
        var busy = true;
        var command = viewModel.MakeDelegate(() =>
        {
            count++;
            // ReSharper disable once AccessToDisposedClosure
            busy = viewModel.BusyState.IsBusy;
        }, CommandMode.Simple);
        var parameterCount = 0;
        var parameterCommand = viewModel.MakeDelegateWithParameter(_ => parameterCount++, CommandMode.Simple);

        // Act
        command.Execute(null);
        parameterCommand.Execute(1);

        // Assert
        Assert.Equal(1, count);
        Assert.False(busy);
        Assert.Equal(1, parameterCount);
    }

    [Fact]
    public void SimpleDoesNotExecuteWhenNotAccepted()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var count = 0;
        var command = viewModel.MakeDelegate(() => count++, CommandMode.Simple);
        var parameterCommand = viewModel.MakeDelegateWithParameter(_ => count++, CommandMode.Simple);
        var asyncCommand = viewModel.MakeAsync(() =>
        {
            count++;
            return Task.CompletedTask;
        }, CommandMode.Simple);
        var asyncParameterCommand = viewModel.MakeAsyncWithParameter(_ =>
        {
            count++;
            return Task.CompletedTask;
        }, CommandMode.Simple);
        viewModel.Accepts = false;

        // Act
        command.Execute(null);
        parameterCommand.Execute(1);
        asyncCommand.Execute(null);
        asyncParameterCommand.Execute(1);

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void SimpleExecutesWhileBusy()
    {
        // Arrange
        using var viewModel = new ModeViewModel();
        var count = 0;
        var command = viewModel.MakeDelegate(() => count++, CommandMode.Simple);

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
        using var viewModel = new ModeViewModel();
        var completion = new TaskCompletionSource();
        var command = viewModel.MakeAsync(() => completion.Task, CommandMode.Simple);
        var parameterCount = 0;
        var parameterCommand = viewModel.MakeAsyncWithParameter(_ =>
        {
            parameterCount++;
            return Task.CompletedTask;
        }, CommandMode.Simple);

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
        using var viewModel = new ModeViewModel();
        var command = viewModel.MakeDelegate(static () => { }, CommandMode.Simple);
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
    public void DefaultUsesModeOfOptions()
    {
        // Arrange
        using var viewModel = new ModeViewModel(CommandMode.Simple);
        var count = 0;
        var command = viewModel.MakeDelegate(() => count++);

        // Act
        using (viewModel.BusyState.Begin())
        {
            command.Execute(null);
        }

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void DefaultOptionsUseStandard()
    {
        // Assert
        Assert.Equal(CommandMode.Standard, new ExtendViewModelOptions().CommandMode);
    }
}

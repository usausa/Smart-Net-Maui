namespace Smart.Maui.ViewModels;

using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

using Smart.Maui.Input;
using Smart.Maui.Internal;
using Smart.Mvvm.ViewModels;

public abstract class ExtendViewModelBase : ViewModelBase
{
    private static readonly ExtendViewModelOptions DefaultOptions = new();

    // ------------------------------------------------------------
    // Member
    // ------------------------------------------------------------

    private readonly CommandBehavior defaultBehavior;

    private readonly bool autoUpdateCommandState;

    private List<IObserveCommand>? commands;

    // ------------------------------------------------------------
    // Property
    // ------------------------------------------------------------

    protected bool AcceptsOperation { get; set; } = true;

    // ------------------------------------------------------------
    // Constructor
    // ------------------------------------------------------------

    protected ExtendViewModelBase(IExtendViewModelOptions? options = null)
        : base(options ?? DefaultOptions)
    {
        var behavior = options?.CommandBehavior ?? DefaultOptions.CommandBehavior;
        defaultBehavior = behavior == CommandBehavior.Default ? CommandBehavior.Standard : behavior;
        autoUpdateCommandState = options?.AutoUpdateCommandState ?? DefaultOptions.AutoUpdateCommandState;
    }

    // ------------------------------------------------------------
    // Override
    // ------------------------------------------------------------

    protected override void RaisePropertyChanged(PropertyChangedEventArgs args)
    {
        base.RaisePropertyChanged(args);

        if (autoUpdateCommandState)
        {
            UpdateCommandState();
        }
    }

    // ------------------------------------------------------------
    // Command helper
    // ------------------------------------------------------------

    private void AddCommandObserver(IObserveCommand command)
    {
        if (commands is null)
        {
            commands = [];
            BusyState.PropertyChanged += BusyStateOnPropertyChanged;
            Disposables.Add(new DelegateDisposable(() => BusyState.PropertyChanged -= BusyStateOnPropertyChanged));
        }
        commands.Add(command);
    }

    private void BusyStateOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IBusyState.IsBusy))
        {
            UpdateCommandState();
        }
    }

    private void UpdateCommandState()
    {
        if (commands is not null)
        {
            foreach (var command in commands)
            {
                command.RaiseCanExecuteChanged();
            }
        }
    }

    protected TCommand Observe<T, TCommand>(IObservable<T> observable, TCommand command)
        where TCommand : IObserveCommand
    {
        Disposables.Add(observable.Subscribe(_ => command.RaiseCanExecuteChanged()));
        return command;
    }

    protected IObserveCommand MakeDelegateCommand(Action execute, CommandBehavior behavior = CommandBehavior.Default) =>
        MakeDelegateCommand(execute, Functions.True, behavior);

    protected IObserveCommand MakeDelegateCommand(Action execute, Func<bool> canExecute, CommandBehavior behavior = CommandBehavior.Default)
    {
        DelegateCommand command;
        var resolved = ResolveBehavior(behavior);
        if (resolved == CommandBehavior.Simple)
        {
            command = new DelegateCommand(execute, canExecute);
        }
        else if (resolved == CommandBehavior.ControlByBusyState)
        {
            command = new DelegateCommand(() =>
            {
                if (!AcceptsOperation)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    execute();
                }
            }, () => !BusyState.IsBusy && canExecute());
        }
        else
        {
            command = new DelegateCommand(() =>
            {
                if (!AcceptsOperation || BusyState.IsBusy)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    execute();
                }
            }, canExecute);
        }
        AddCommandObserver(command);
        return command;
    }

    protected IObserveCommand MakeDelegateCommand<TParameter>(Action<TParameter> execute, CommandBehavior behavior = CommandBehavior.Default) =>
        MakeDelegateCommand(execute, Functions<TParameter>.True, behavior);

    protected IObserveCommand MakeDelegateCommand<TParameter>(Action<TParameter> execute, Func<TParameter, bool> canExecute, CommandBehavior behavior = CommandBehavior.Default)
    {
        DelegateCommand<TParameter> command;
        var resolved = ResolveBehavior(behavior);
        if (resolved == CommandBehavior.Simple)
        {
            command = new DelegateCommand<TParameter>(execute, canExecute);
        }
        else if (resolved == CommandBehavior.ControlByBusyState)
        {
            command = new DelegateCommand<TParameter>(x =>
            {
                if (!AcceptsOperation)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    execute(x);
                }
            }, x => !BusyState.IsBusy && canExecute(x));
        }
        else
        {
            command = new DelegateCommand<TParameter>(x =>
            {
                if (!AcceptsOperation || BusyState.IsBusy)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    execute(x);
                }
            }, canExecute);
        }
        AddCommandObserver(command);
        return command;
    }

    protected IObserveCommand MakeAsyncCommand(Func<Task> execute, CommandBehavior behavior = CommandBehavior.Default) =>
        MakeAsyncCommand(execute, Functions.True, behavior);

    protected IObserveCommand MakeAsyncCommand(Func<Task> execute, Func<bool> canExecute, CommandBehavior behavior = CommandBehavior.Default)
    {
        AsyncCommand command;
        var resolved = ResolveBehavior(behavior);
        if (resolved == CommandBehavior.Simple)
        {
            command = new AsyncCommand(execute, canExecute);
        }
        else if (resolved == CommandBehavior.ControlByBusyState)
        {
            command = new AsyncCommand(async () =>
            {
                if (!AcceptsOperation)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    await execute().ConfigureAwait(true);
                }
            }, () => !BusyState.IsBusy && canExecute());
        }
        else
        {
            command = new AsyncCommand(async () =>
            {
                if (!AcceptsOperation || BusyState.IsBusy)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    await execute().ConfigureAwait(true);
                }
            }, canExecute);
        }
        AddCommandObserver(command);
        return command;
    }

    protected IObserveCommand MakeAsyncCommand<TParameter>(Func<TParameter, Task> execute, CommandBehavior behavior = CommandBehavior.Default) =>
        MakeAsyncCommand(execute, Functions<TParameter>.True, behavior);

    protected IObserveCommand MakeAsyncCommand<TParameter>(Func<TParameter, Task> execute, Func<TParameter, bool> canExecute, CommandBehavior behavior = CommandBehavior.Default)
    {
        AsyncCommand<TParameter> command;
        var resolved = ResolveBehavior(behavior);
        if (resolved == CommandBehavior.Simple)
        {
            command = new AsyncCommand<TParameter>(execute, canExecute);
        }
        else if (resolved == CommandBehavior.ControlByBusyState)
        {
            command = new AsyncCommand<TParameter>(async x =>
            {
                if (!AcceptsOperation)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    await execute(x).ConfigureAwait(true);
                }
            }, x => !BusyState.IsBusy && canExecute(x));
        }
        else
        {
            command = new AsyncCommand<TParameter>(async x =>
            {
                if (!AcceptsOperation || BusyState.IsBusy)
                {
                    return;
                }

                using (BusyState.Begin())
                {
                    await execute(x).ConfigureAwait(true);
                }
            }, canExecute);
        }
        AddCommandObserver(command);
        return command;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CommandBehavior ResolveBehavior(CommandBehavior behavior) =>
        behavior == CommandBehavior.Default ? defaultBehavior : behavior;

    // ------------------------------------------------------------
    // Reactive helper
    // ------------------------------------------------------------

    protected IObservable<string?> Observe(string name)
    {
        return Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                static h => (_, e) => h(e),
                h => PropertyChanged += h,
                h => PropertyChanged -= h)
            .Where(x => x.PropertyName == name)
            .Select(x => x.PropertyName);
    }

    protected void Subscribe<T>(IObservable<T> observable, Action<T> action)
    {
        Disposables.Add(observable.Subscribe(action));
    }
}

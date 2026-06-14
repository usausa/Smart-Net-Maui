namespace Smart.Maui.Input;

using System.Windows.Input;

public sealed class DelegateCommandTest
{
    //------------------------------------------------------------------
    //DelegateCommand tests
    //------------------------------------------------------------------

    [Fact]
    public void ExecuteInvokesDelegate()
    {
        // Arrange
        var count = 0;
        var command = new DelegateCommand(() => count++);

        // Act
        ((ICommand)command).Execute(null);

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void CanExecuteReturnsTrueByDefault()
    {
        // Arrange
        var command = new DelegateCommand(() => { });

        // Act & Assert
        Assert.True(((ICommand)command).CanExecute(null));
    }

    [Fact]
    public void CanExecuteRespectsUserPredicate()
    {
        // Arrange
        var allow = false;
        // ReSharper disable once AccessToModifiedClosure
        var command = new DelegateCommand(() => { }, () => allow);
        var iCommand = (ICommand)command;

        // Act & Assert
        Assert.False(iCommand.CanExecute(null));
        allow = true;
        Assert.True(iCommand.CanExecute(null));
    }

    [Fact]
    public void ManualCanExecuteChangedNotifiesSubscribers()
    {
        // Arrange
        var command = new DelegateCommand(() => { });
        var raised = 0;
        command.CanExecuteChanged += (_, _) => raised++;

        // Act
        command.RaiseCanExecuteChanged();

        // Assert
        Assert.Equal(1, raised);
    }

    //------------------------------------------------------------------
    // Generic DelegateCommand<T>
    //------------------------------------------------------------------

    [Fact]
    public void GenericExecuteInvokesDelegate()
    {
        // Arrange
        var count = 0;
        var command = new DelegateCommand<int>(_ => count++);

        // Act
        ((ICommand)command).Execute(42);

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void GenericParameterPassedToExecute()
    {
        // Arrange
        var received = 0;
        var command = new DelegateCommand<int>(v => received = v);

        // Act
        ((ICommand)command).Execute(99);

        // Assert
        Assert.Equal(99, received);
    }

    [Fact]
    public void GenericCanExecuteReturnsTrueByDefault()
    {
        // Arrange
        var command = new DelegateCommand<int>(_ => { });

        // Act & Assert
        Assert.True(((ICommand)command).CanExecute(0));
    }

    [Fact]
    public void GenericCanExecuteRespectsUserPredicate()
    {
        // Arrange
        var allow = false;
        // ReSharper disable once AccessToModifiedClosure
        var command = new DelegateCommand<int>(_ => { }, _ => allow);
        var iCommand = (ICommand)command;

        // Act & Assert
        Assert.False(iCommand.CanExecute(0));
        allow = true;
        Assert.True(iCommand.CanExecute(0));
    }

    [Fact]
    public void GenericParameterPassedToCanExecute()
    {
        // Arrange
        var received = 0;
        var command = new DelegateCommand<int>(_ => { }, v =>
        {
            received = v;
            return true;
        });

        // Act
        ((ICommand)command).CanExecute(7);

        // Assert
        Assert.Equal(7, received);
    }

    [Fact]
    public void GenericManualCanExecuteChangedNotifiesSubscribers()
    {
        // Arrange
        var command = new DelegateCommand<int>(_ => { });
        var raised = 0;
        command.CanExecuteChanged += (_, _) => raised++;

        // Act
        command.RaiseCanExecuteChanged();

        // Assert
        Assert.Equal(1, raised);
    }

    [Fact]
    public void GenericCastNullToDefaultForValueType()
    {
        // Arrange
        var received = -1;
        var command = new DelegateCommand<int>(v => received = v);

        // Act
        // null parameter for value-type T must map to default(T)=0
        ((ICommand)command).Execute(null);

        // Assert
        Assert.Equal(0, received);
    }

    [Fact]
    public void GenericCastNullForReferenceType()
    {
        // Arrange
        var called = false;
        var command = new DelegateCommand<string?>(_ => called = true);

        // Act
        // null parameter for reference-type T must be accepted
        ((ICommand)command).Execute(null);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public void GenericCastStringValue()
    {
        // Arrange
        var received = string.Empty;
        var command = new DelegateCommand<string>(v => received = v);

        // Act
        ((ICommand)command).Execute("hello");

        // Assert
        Assert.Equal("hello", received);
    }
}

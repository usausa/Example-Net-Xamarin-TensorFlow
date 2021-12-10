namespace LegoDetect.FormsApp;

using System.Windows.Input;

using Smart.ComponentModel;
using Smart.Forms.ViewModels;
using Smart.Navigation;

using LegoDetect.FormsApp.Shell;

public class MainPageViewModel : ViewModelBase, IShellControl
{
    public ApplicationState ApplicationState { get; }

    public INavigator Navigator { get; }

    public NotificationValue<bool> TitleVisible { get; } = new();

    public NotificationValue<string> Title { get; } = new();

    public NotificationValue<bool> FunctionVisible { get; } = new();

    public NotificationValue<string> Function1Text { get; } = new();
    public NotificationValue<string> Function2Text { get; } = new();

    public NotificationValue<bool> Function1Enabled { get; } = new();
    public NotificationValue<bool> Function2Enabled { get; } = new();

    public ICommand Function1Command { get; }
    public ICommand Function2Command { get; }

    //--------------------------------------------------------------------------------
    // Constructor
    //--------------------------------------------------------------------------------

    public MainPageViewModel(
        ApplicationState applicationState,
        INavigator navigator)
        : base(applicationState)
    {
        ApplicationState = applicationState;
        Navigator = navigator;

        Function1Command = MakeAsyncCommand(
                () => Navigator.NotifyAsync(ShellEvent.Function1),
                () => Function1Enabled.Value)
            .Observe(Function1Enabled);
        Function2Command = MakeAsyncCommand(
                () => Navigator.NotifyAsync(ShellEvent.Function2),
                () => Function2Enabled.Value)
            .Observe(Function2Enabled);
    }
}

namespace LegoDetect.FormsApp.Modules;

using System.Threading.Tasks;

using Smart.Forms.ViewModels;
using Smart.Navigation;

using LegoDetect.FormsApp.Shell;

public class AppViewModelBase : ViewModelBase, INavigatorAware, INavigationEventSupport, INotifySupportAsync<ShellEvent>
{
    public INavigator Navigator { get; set; } = default!;

    public ApplicationState ApplicationState { get; }

    protected AppViewModelBase(ApplicationState applicationState)
        : base(applicationState)
    {
        ApplicationState = applicationState;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        System.Diagnostics.Debug.WriteLine($"{GetType()} is Disposed");
    }

    public virtual void OnNavigatingFrom(INavigationContext context)
    {
    }

    public virtual void OnNavigatingTo(INavigationContext context)
    {
    }

    public virtual void OnNavigatedTo(INavigationContext context)
    {
    }

    public Task NavigatorNotifyAsync(ShellEvent parameter)
    {
        return parameter switch
        {
            ShellEvent.Back => OnNotifyBackAsync(),
            ShellEvent.Function1 => OnNotifyFunction1(),
            ShellEvent.Function2 => OnNotifyFunction2(),
            _ => Task.CompletedTask
        };
    }

    protected virtual Task OnNotifyBackAsync() => Task.CompletedTask;

    protected virtual Task OnNotifyFunction1() => Task.CompletedTask;

    protected virtual Task OnNotifyFunction2() => Task.CompletedTask;
}

namespace LegoDetect.FormsApp.Modules.Navigation.Stack;

using System.Threading.Tasks;

using Smart.Navigation;

public class Stack1ViewModel : AppViewModelBase
{
    public Stack1ViewModel(ApplicationState applicationState)
        : base(applicationState)
    {
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.NavigationMenu);

    protected override Task OnNotifyFunction1() => OnNotifyBackAsync();

    protected override Task OnNotifyFunction4() => Navigator.PushAsync(ViewId.NavigationStack2);
}

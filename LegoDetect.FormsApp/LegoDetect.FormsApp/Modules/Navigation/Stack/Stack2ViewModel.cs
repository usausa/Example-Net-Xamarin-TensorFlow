namespace LegoDetect.FormsApp.Modules.Navigation.Stack;

using System.Threading.Tasks;

using Smart.Navigation;

public class Stack2ViewModel : AppViewModelBase
{
    public Stack2ViewModel(ApplicationState applicationState)
        : base(applicationState)
    {
    }

    protected override Task OnNotifyBackAsync() => Navigator.PopAsync(1);

    protected override Task OnNotifyFunction1() => OnNotifyBackAsync();

    protected override Task OnNotifyFunction4() => Navigator.PushAsync(ViewId.NavigationStack3);
}

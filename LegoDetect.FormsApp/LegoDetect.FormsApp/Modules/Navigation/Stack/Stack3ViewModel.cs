namespace LegoDetect.FormsApp.Modules.Navigation.Stack;

using System.Threading.Tasks;

using Smart.Navigation;

public class Stack3ViewModel : AppViewModelBase
{
    public Stack3ViewModel(ApplicationState applicationState)
        : base(applicationState)
    {
    }

    protected override Task OnNotifyBackAsync() => Navigator.PopAsync(1);

    protected override Task OnNotifyFunction1() => OnNotifyBackAsync();

    protected override Task OnNotifyFunction2() => Navigator.PopAsync(2);
}

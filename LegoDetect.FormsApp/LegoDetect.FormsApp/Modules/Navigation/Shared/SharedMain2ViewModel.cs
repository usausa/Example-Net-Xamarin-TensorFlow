namespace LegoDetect.FormsApp.Modules.Navigation.Shared;

using System.Threading.Tasks;

using Smart.ComponentModel;
using Smart.Navigation;

public class SharedMain2ViewModel : AppViewModelBase
{
    public NotificationValue<string> No { get; } = new();

    public SharedMain2ViewModel(ApplicationState applicationState)
        : base(applicationState)
    {
    }

    public override void OnNavigatedTo(INavigationContext context)
    {
        if (!context.Attribute.IsRestore())
        {
            No.Value = context.Parameter.GetNo();
        }
    }

    protected override Task OnNotifyBackAsync() =>
        Navigator.ForwardAsync(ViewId.NavigationSharedInput, Parameters.MakeNextViewId(ViewId.NavigationSharedMain2));

    protected override Task OnNotifyFunction1() => OnNotifyBackAsync();

    protected override Task OnNotifyFunction4() => Navigator.ForwardAsync(ViewId.NavigationMenu);
}

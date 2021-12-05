namespace LegoDetect.FormsApp.Modules.Component;

using System.Threading.Tasks;

using Smart.Navigation;

public class ComponentMenuViewModel : AppViewModelBase
{
    public ComponentMenuViewModel(ApplicationState applicationState)
        : base(applicationState)
    {
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.Menu);
}

namespace LegoDetect.FormsApp.Modules.Toolkit;

using System.Threading.Tasks;

using Smart.Navigation;

public class ToolkitMenuViewModel : AppViewModelBase
{
    public ToolkitMenuViewModel(ApplicationState applicationState)
        : base(applicationState)
    {
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.Menu);
}

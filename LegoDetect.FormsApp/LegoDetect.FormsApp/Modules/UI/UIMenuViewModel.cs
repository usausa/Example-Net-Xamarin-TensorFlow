namespace LegoDetect.FormsApp.Modules.UI;

using System.Threading.Tasks;

using Smart.Navigation;

public class UIMenuViewModel : AppViewModelBase
{
    public UIMenuViewModel(ApplicationState applicationState)
        : base(applicationState)
    {
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.Menu);
}

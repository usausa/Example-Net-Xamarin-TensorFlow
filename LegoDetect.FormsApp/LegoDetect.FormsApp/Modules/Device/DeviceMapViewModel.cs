namespace LegoDetect.FormsApp.Modules.Device;

using System.Threading.Tasks;

using Smart.Navigation;

public class DeviceMapViewModel : AppViewModelBase
{
    public DeviceMapViewModel(ApplicationState applicationState)
        : base(applicationState)
    {
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.DeviceMenu);
}

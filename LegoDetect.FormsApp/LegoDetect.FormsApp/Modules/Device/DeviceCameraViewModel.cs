namespace LegoDetect.FormsApp.Modules.Device;

using System.Threading.Tasks;

using Smart.Navigation;

public class DeviceCameraViewModel : AppViewModelBase
{
    public DeviceCameraViewModel(ApplicationState applicationState)
        : base(applicationState)
    {
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.DeviceMenu);
}

namespace LegoDetect.FormsApp.Modules.Device;

using System.Threading.Tasks;

using Smart.Navigation;

public class DeviceBarcodeViewModel : AppViewModelBase
{
    public DeviceBarcodeViewModel(ApplicationState applicationState)
        : base(applicationState)
    {
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.DeviceMenu);
}

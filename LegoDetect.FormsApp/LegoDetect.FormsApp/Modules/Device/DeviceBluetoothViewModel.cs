namespace LegoDetect.FormsApp.Modules.Device;

using System.Threading.Tasks;

using Smart.Navigation;

public class DeviceBluetoothViewModel : AppViewModelBase
{
    public DeviceBluetoothViewModel(ApplicationState applicationState)
        : base(applicationState)
    {
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.DeviceMenu);
}

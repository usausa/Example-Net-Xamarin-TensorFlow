namespace LegoDetect.FormsApp.Modules.Detect;

using System.Threading.Tasks;
using System.Windows.Input;

using LegoDetect.FormsApp.Messaging;

using Smart.Navigation;

public class DetectCameraViewModel : AppViewModelBase
{
    public CameraCaptureRequest CaptureRequest { get; } = new();

    public ICommand BackCommand { get; }
    public ICommand CaptureCommand { get; }

    public DetectCameraViewModel(
        ApplicationState applicationState)
        : base(applicationState)
    {
        BackCommand = MakeAsyncCommand(OnNotifyBackAsync);
        CaptureCommand = MakeAsyncCommand(ExecuteCapture);
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.Menu);

    protected override Task OnNotifyFunction1() => OnNotifyBackAsync();

    private async Task ExecuteCapture()
    {
        var image = await CaptureRequest.CaptureAsync();
        if (image is not null)
        {
            await Navigator.ForwardAsync(ViewId.DetectResult, Parameters.MakeImage(image));
        }
    }
}

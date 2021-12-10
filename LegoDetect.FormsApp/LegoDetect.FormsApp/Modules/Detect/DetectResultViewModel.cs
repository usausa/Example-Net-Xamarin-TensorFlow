namespace LegoDetect.FormsApp.Modules.Detect;

using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LegoDetect.FormsApp.Messaging;

using Models;

using Services;

using Smart.ComponentModel;
using Smart.Forms.ViewModels;
using Smart.Navigation;

public class DetectResultViewModel : AppViewModelBase
{
    private readonly IObjectDetectService objectDetectService;

    public NotificationValue<DetectResult[]> Result { get; } = new();

    public LoadImageRequest LoadImageRequest { get; } = new();

    public ICommand RetryCommand { get; }
    public ICommand CloseCommand { get; }

    public DetectResultViewModel(
        ApplicationState applicationState,
        IObjectDetectService objectDetectService)
        : base(applicationState)
    {
        this.objectDetectService = objectDetectService;

        RetryCommand = MakeAsyncCommand(OnNotifyBackAsync);
        CloseCommand = MakeAsyncCommand(() => Navigator.ForwardAsync(ViewId.Menu));
    }

    public override async void OnNavigatingTo(INavigationContext context)
    {
        if (!context.Attribute.IsRestore())
        {
            var image = context.Parameter.GetImage();
            LoadImageRequest.Load(image);

            await Navigator.PostActionAsync(() => BusyState.UsingAsync(async () =>
            {
                Result.Value = (await objectDetectService.DetectAsync(image))
                    .Where(x => x.Score > 0.3)
                    .ToArray();
            }));
        }
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.DetectCamera);
}

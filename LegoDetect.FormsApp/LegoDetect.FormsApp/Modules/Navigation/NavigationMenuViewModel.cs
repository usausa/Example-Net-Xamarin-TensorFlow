namespace LegoDetect.FormsApp.Modules.Navigation;

using System.Threading.Tasks;
using System.Windows.Input;

using Smart.Navigation;

using LegoDetect.FormsApp.Components.Dialog;

using XamarinFormsComponents.Popup;

public class NavigationMenuViewModel : AppViewModelBase
{
    private readonly IApplicationDialog dialog;

    private readonly IPopupNavigator popupNavigator;

    public ICommand ForwardCommand { get; }
    public ICommand SharedCommand { get; }
    public ICommand ModalCommand { get; }

    public NavigationMenuViewModel(
        ApplicationState applicationState,
        IApplicationDialog dialog,
        IPopupNavigator popupNavigator)
    : base(applicationState)
    {
        this.dialog = dialog;
        this.popupNavigator = popupNavigator;

        ForwardCommand = MakeAsyncCommand<ViewId>(x => Navigator.ForwardAsync(x));
        SharedCommand = MakeAsyncCommand<ViewId>(x => Navigator.ForwardAsync(ViewId.NavigationSharedInput, Parameters.MakeNextViewId(x)));
        ModalCommand = MakeAsyncCommand(ShowModalAsync);
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.Menu);

    protected override Task OnNotifyFunction1() => OnNotifyBackAsync();

    private async Task ShowModalAsync()
    {
        var result = await popupNavigator.InputNumberAsync("Number", string.Empty, 8);
        if (result != null)
        {
            await dialog.Information($"result=[{result}]");
        }
    }
}

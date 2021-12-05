namespace LegoDetect.FormsApp.Modules.Navigation.Navigate;

using System.Threading.Tasks;

using Smart.Forms.ViewModels;
using Smart.Navigation;

using LegoDetect.FormsApp.Components.Dialog;

public class NavigateCancelViewModel : AppViewModelBase
{
    private readonly IApplicationDialog dialog;

    public NavigateCancelViewModel(
        ApplicationState applicationState,
        IApplicationDialog dialog)
        : base(applicationState)
    {
        this.dialog = dialog;
    }

    public override async void OnNavigatedTo(INavigationContext context)
    {
        if (!context.Attribute.IsRestore())
        {
            await Navigator.PostActionAsync(() => BusyState.Using(async () =>
            {
                if (await dialog.Confirm("Cancel ?", ok: "Yes", cancel: "No"))
                {
                    await Navigator.ForwardAsync(ViewId.Menu);
                }
            }));
        }
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.NavigationMenu);

    protected override Task OnNotifyFunction1() => OnNotifyBackAsync();
}

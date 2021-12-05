namespace LegoDetect.FormsApp.Modules.Navigation.Navigate;

using System.Threading.Tasks;

using Smart.Forms.ViewModels;
using Smart.Navigation;

using LegoDetect.FormsApp.Components.Dialog;

public class NavigateInitializeViewModel : AppViewModelBase
{
    private readonly IApplicationDialog dialog;

    public NavigateInitializeViewModel(
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
            await Navigator.PostActionAsync(() => BusyState.Using(InitializeAsync));
        }
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.NavigationMenu);

    protected override Task OnNotifyFunction1() => OnNotifyBackAsync();

    protected async Task InitializeAsync()
    {
        using (dialog.Loading("Initialize"))
        {
            await Task.Delay(5000);
        }
    }
}

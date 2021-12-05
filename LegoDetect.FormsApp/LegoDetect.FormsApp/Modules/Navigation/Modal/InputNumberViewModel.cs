namespace LegoDetect.FormsApp.Modules.Navigation.Modal;

using System.Threading.Tasks;
using System.Windows.Input;

using Smart.ComponentModel;

using LegoDetect.FormsApp.Models.Input;

using XamarinFormsComponents.Popup;

public class InputNumberViewModel : AppDialogViewModelBase, IPopupResult<string>, IPopupInitialize<NumberInputParameter>
{
    public NotificationValue<string> Title { get; } = new();

    public NumberInputModel Input { get; } = new();

    public string Result { get; private set; } = string.Empty;

    public ICommand ClearCommand { get; }
    public ICommand PopCommand { get; }
    public ICommand PushCommand { get; }

    public ICommand CloseCommand { get; }
    public ICommand CommitCommand { get; }

    public InputNumberViewModel()
    {
        ClearCommand = MakeDelegateCommand(() => Input.Clear());
        PopCommand = MakeDelegateCommand(() => Input.Pop());
        PushCommand = MakeDelegateCommand<string>(x => Input.Push(x));

        CloseCommand = MakeAsyncCommand(Close);
        CommitCommand = MakeAsyncCommand(Commit);
    }

    public void Initialize(NumberInputParameter parameter)
    {
        Title.Value = parameter.Title;
        Input.Text = parameter.Value;
        Input.MaxLength = parameter.MaxLength;
    }

    private async Task Close() => await PopupNavigator.PopAsync();

    private async Task Commit()
    {
        Result = Input.Text;

        await PopupNavigator.PopAsync();
    }
}

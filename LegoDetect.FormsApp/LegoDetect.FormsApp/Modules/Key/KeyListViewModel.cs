namespace LegoDetect.FormsApp.Modules.Key;

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Smart.Collections.Generic;
using Smart.ComponentModel;
using Smart.Navigation;

using LegoDetect.FormsApp.Models.Entity;

public class KeyListViewModel : AppViewModelBase
{
    public ObservableCollection<DataEntity> Items { get; } = new();

    public NotificationValue<string> Selected { get; } = new();

    public ICommand SelectCommand { get; }
    public ICommand DeleteCommand { get; }

    public KeyListViewModel(ApplicationState applicationState)
        : base(applicationState)
    {
        Items.AddRange(Enumerable.Range(1, 20).Select(x => new DataEntity { Id = x, Name = $"Name-{x}" }));

        SelectCommand = MakeDelegateCommand<DataEntity>(x => Selected.Value = $"Select id=[{x.Id}]");
        DeleteCommand = MakeDelegateCommand<DataEntity>(x => Selected.Value = $"Delete id=[{x.Id}]");
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.KeyMenu);

    protected override Task OnNotifyFunction1() => OnNotifyBackAsync();
}

namespace LegoDetect.FormsApp.Modules.Database;

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Smart.ComponentModel;
using Smart.Navigation;

using LegoDetect.FormsApp.Components.Dialog;
using LegoDetect.FormsApp.Models.Entity;
using LegoDetect.FormsApp.Services;

public class DatabaseViewModel : AppViewModelBase
{
    private readonly IApplicationDialog dialogs;

    private readonly DataService dataService;

    public NotificationValue<int> BulkDataCount { get; } = new();

    public ICommand InsertCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand QueryCommand { get; }

    public ICommand BulkInsertCommand { get; }
    public ICommand DeleteAllCommand { get; }
    public ICommand QueryAllCommand { get; }

    public DatabaseViewModel(
        ApplicationState applicationState,
        IApplicationDialog dialogs,
        DataService dataService)
        : base(applicationState)
    {
        this.dialogs = dialogs;
        this.dataService = dataService;

        InsertCommand = MakeAsyncCommand(InsertAsync);
        UpdateCommand = MakeAsyncCommand(UpdateAsync);
        DeleteCommand = MakeAsyncCommand(DeleteAsync);
        QueryCommand = MakeAsyncCommand(QueryAsync);

        BulkInsertCommand = MakeAsyncCommand(BulkInsertAsync, () => BulkDataCount.Value == 0).Observe(BulkDataCount);
        DeleteAllCommand = MakeAsyncCommand(DeleteAllAsync, () => BulkDataCount.Value > 0).Observe(BulkDataCount);
        QueryAllCommand = MakeAsyncCommand(QueryAllAsync);
    }

    public override async void OnNavigatingTo(INavigationContext context)
    {
        BulkDataCount.Value = await dataService.CountBulkDataAsync();
    }

    protected override Task OnNotifyBackAsync() => Navigator.ForwardAsync(ViewId.Menu);

    protected override Task OnNotifyFunction1() => OnNotifyBackAsync();

    private async Task InsertAsync()
    {
        var ret = await dataService.InsertDataAsync(new DataEntity { Id = 1L, Name = "Data-1", CreateAt = DateTime.Now });

        if (ret)
        {
            await dialogs.Information("Inserted");
        }
        else
        {
            await dialogs.Information("Key duplicate");
        }
    }

    private async Task UpdateAsync()
    {
        var effect = await dataService.UpdateDataAsync(1L, "Updated");

        await dialogs.Information($"Effect={effect}");
    }

    private async Task DeleteAsync()
    {
        var effect = await dataService.DeleteDataAsync(1L);

        await dialogs.Information($"Effect={effect}");
    }

    private async Task QueryAsync()
    {
        var entity = await dataService.QueryDataAsync(1L);

        if (entity != null)
        {
            await dialogs.Information($"Name={entity.Name}\r\nDate={entity.CreateAt:yyyy/MM/dd HH:mm:ss}");
        }
        else
        {
            await dialogs.Information("Not found");
        }
    }

    private async Task BulkInsertAsync()
    {
        var list = Enumerable.Range(1, 10000)
            .Select(x => new BulkDataEntity
            {
                Key1 = $"{x / 1000:D2}",
                Key2 = $"{x % 1000:D2}",
                Key3 = "0",
                Value1 = 1,
                Value2 = 2,
                Value3 = 3,
                Value4 = 4,
                Value5 = 5
            })
            .ToList();

        var watch = new Stopwatch();

        using (dialogs.Loading("Bulk insert"))
        {
            watch.Start();

            await Task.Run(() => dataService.InsertBulkDataEnumerable(list));

            watch.Stop();
        }

        BulkDataCount.Value = await dataService.CountBulkDataAsync();

        await dialogs.Information($"Inserted\r\nElapsed={watch.ElapsedMilliseconds}");
    }

    private async Task DeleteAllAsync()
    {
        await dataService.DeleteAllBulkDataAsync();

        BulkDataCount.Value = await dataService.CountBulkDataAsync();
    }

    private async Task QueryAllAsync()
    {
        var watch = new Stopwatch();

        using (dialogs.Loading("Query all"))
        {
            watch.Start();

            await Task.Run(() => dataService.QueryAllBulkDataList());

            watch.Stop();
        }

        await dialogs.Information($"Query\r\nElapsed={watch.ElapsedMilliseconds}");
    }
}

namespace LegoDetect.FormsApp.Services;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

using Smart.Data;
using Smart.Data.Mapper;
using Smart.Data.Mapper.Builders;

using LegoDetect.FormsApp.Helpers.Data;
using LegoDetect.FormsApp.Models.Entity;

public class DataServiceOptions
{
    [AllowNull]
    public string Path { get; set; }
}

public class DataService
{
    private readonly DataServiceOptions options;

    private readonly DelegateDbProvider provider;

    public DataService(DataServiceOptions options)
    {
        this.options = options;

        var connectionString = $"Data Source={options.Path}";
        provider = new DelegateDbProvider(() => new SqliteConnection(connectionString));
    }

    public void DeleteAll()
    {
        if (File.Exists(options.Path))
        {
            File.Delete(options.Path);
        }
    }

    public async ValueTask PrepareAsync()
    {
        if (File.Exists(options.Path))
        {
            return;
        }

        await provider.UsingAsync(async con =>
        {
            await con.ExecuteAsync("PRAGMA AUTO_VACUUM=1");
            await con.ExecuteAsync(SqlHelper.MakeCreate<DataEntity>());
            await con.ExecuteAsync(SqlHelper.MakeCreate<BulkDataEntity>());
            await con.ExecuteAsync(SqlHelper.MakeCreate<WorkEntity>());
        });

        await InsertWorkEnumerableAsync(new[]
        {
            new WorkEntity { Id = 1, Name = "Sample-1" },
            new WorkEntity { Id = 2, Name = "Sample-2" },
            new WorkEntity { Id = 3, Name = "Sample-3" },
            new WorkEntity { Id = 4, Name = "Sample-4" }
        });
    }

    //--------------------------------------------------------------------------------
    // CRUD
    //--------------------------------------------------------------------------------

    public async ValueTask<bool> InsertDataAsync(DataEntity entity)
    {
        return await provider.UsingAsync(async con =>
        {
            try
            {
                await con.ExecuteAsync(SqlInsert<DataEntity>.Values(), entity);
                return true;
            }
            catch (SqliteException e)
            {
                if (e.SqliteErrorCode == SQLitePCL.raw.SQLITE_CONSTRAINT)
                {
                    return false;
                }
                throw;
            }
        });
    }

    public ValueTask<int> UpdateDataAsync(long id, string name) =>
        provider.UsingAsync(con => con.ExecuteAsync(SqlUpdate<DataEntity>.Set("Name = @Name", "Id = @Id"), new { Id = id, Name = name }));

    public ValueTask<int> DeleteDataAsync(long id) =>
        provider.UsingAsync(con => con.ExecuteAsync(SqlDelete<DataEntity>.ByKey(), new { Id = id }));

    public ValueTask<DataEntity?> QueryDataAsync(long id) =>
        provider.UsingAsync(con => con.QueryFirstOrDefaultAsync<DataEntity>(SqlSelect<DataEntity>.ByKey(), new { Id = id }));

    //--------------------------------------------------------------------------------
    // Bulk
    //--------------------------------------------------------------------------------

    public ValueTask<int> CountBulkDataAsync() =>
        provider.UsingAsync(con => con.ExecuteScalarAsync<int>(SqlCount<BulkDataEntity>.All()));

    public void InsertBulkDataEnumerable(IEnumerable<BulkDataEntity> source)
    {
        provider.UsingTx((con, tx) =>
        {
            foreach (var entity in source)
            {
                con.Execute(SqlInsert<BulkDataEntity>.Values(), entity, tx);
            }

            tx.Commit();
        });
    }

    public ValueTask<int> DeleteAllBulkDataAsync() =>
        provider.UsingAsync(con => con.ExecuteAsync("DELETE FROM BulkData"));

    public List<BulkDataEntity> QueryAllBulkDataList() =>
        provider.Using(con => con.QueryList<BulkDataEntity>(SqlSelect<BulkDataEntity>.All()));

    //--------------------------------------------------------------------------------
    // Work
    //--------------------------------------------------------------------------------

    public ValueTask<List<WorkEntity>> QueryWorkListAsync() =>
        provider.Using(con => con.QueryListAsync<WorkEntity>(SqlSelect<WorkEntity>.All()));

    public ValueTask<WorkEntity?> QueryWorkAsync(int id) =>
        provider.Using(con =>
            con.QueryFirstOrDefaultAsync<WorkEntity>(SqlSelect<WorkEntity>.ByKey(), new { Id = id }));

    public ValueTask InsertWorkEnumerableAsync(IEnumerable<WorkEntity> source)
    {
        return provider.UsingTxAsync(async (con, tx) =>
        {
            foreach (var entity in source)
            {
                await con.ExecuteAsync(SqlInsert<WorkEntity>.Values(), entity, tx);
            }

            await tx.CommitAsync();
        });
    }

    public ValueTask InsertWorkAsync(string name) =>
        provider.UsingAsync(async con =>
        {
            var maxId = await con.ExecuteScalarAsync<int>("SELECT MAX(Id) FROM Work");
            await con.ExecuteAsync(SqlInsert<WorkEntity>.Values(), new WorkEntity { Id = maxId + 1, Name = name });
        });

    public ValueTask<int> UpdateWorkAsync(WorkEntity entity) =>
        provider.UsingAsync(con => con.ExecuteAsync(SqlUpdate<WorkEntity>.Set("Name = @Name", "Id = @Id"), entity));

    public ValueTask<int> DeleteWorkAsync(long id) =>
        provider.UsingAsync(con => con.ExecuteAsync(SqlDelete<WorkEntity>.ByKey(), new { Id = id }));
}

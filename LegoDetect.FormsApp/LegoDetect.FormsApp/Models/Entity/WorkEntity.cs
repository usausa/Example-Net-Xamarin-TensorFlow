namespace LegoDetect.FormsApp.Models.Entity;

using System.Diagnostics.CodeAnalysis;

using Smart.Data.Mapper.Attributes;

public class WorkEntity
{
    [PrimaryKey]
    public long Id { get; set; }

    [AllowNull]
    public string Name { get; set; }
}

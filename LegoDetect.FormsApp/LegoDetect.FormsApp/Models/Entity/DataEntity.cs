namespace LegoDetect.FormsApp.Models.Entity;

using System;
using System.Diagnostics.CodeAnalysis;

using Smart.Data.Mapper.Attributes;

public class DataEntity
{
    [PrimaryKey]
    public long Id { get; set; }

    [AllowNull]
    public string Name { get; set; }

    public DateTime CreateAt { get; set; }
}

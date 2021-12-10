namespace LegoDetect.FormsApp.Models;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "Ignore")]
public struct Bounds
{
    public float Left { get; set; }

    public float Top { get; set; }

    public float Width { get; set; }

    public float Height { get; set; }
}

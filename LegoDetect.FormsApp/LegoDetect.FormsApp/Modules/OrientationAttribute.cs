namespace LegoDetect.FormsApp.Modules;

using System;

using LegoDetect.FormsApp.Components.Device;

[AttributeUsage(AttributeTargets.Class)]
public sealed class OrientationAttribute : Attribute
{
    public Orientation Orientation { get; }

    public OrientationAttribute(Orientation orientation)
    {
        Orientation = orientation;
    }
}

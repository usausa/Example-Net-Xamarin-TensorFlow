namespace LegoDetect.FormsApp.Modules;

using Smart.Navigation;

public static class Parameters
{
    private const string Image = nameof(Image);

    public static NavigationParameter MakeImage(byte[] data) =>
        new NavigationParameter().SetValue(Image, data);

    public static byte[] GetImage(this INavigationParameter parameter) =>
        parameter.GetValue<byte[]>(Image);
}

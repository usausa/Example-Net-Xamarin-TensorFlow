namespace LegoDetect.FormsApp.Shell;

using Smart.ComponentModel;

public interface IShellControl
{
    NotificationValue<bool> TitleVisible { get; }

    NotificationValue<string> Title { get; }

    NotificationValue<bool> FunctionVisible { get; }

    NotificationValue<string> Function1Text { get; }

    NotificationValue<string> Function2Text { get; }

    NotificationValue<bool> Function1Enabled { get; }

    NotificationValue<bool> Function2Enabled { get; }
}

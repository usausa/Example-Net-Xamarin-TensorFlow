namespace LegoDetect.FormsApp.Models.Input;

public class NumberInputParameter
{
    public string Title { get; }

    public string Value { get; }

    public int MaxLength { get; }

    public NumberInputParameter(string title, string value, int maxLength)
    {
        Title = title;
        Value = value;
        MaxLength = maxLength;
    }
}

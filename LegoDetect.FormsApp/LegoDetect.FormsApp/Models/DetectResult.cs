namespace LegoDetect.FormsApp.Models;

using System.Drawing;

public class DetectResult
{
    public string Label { get; }

    public float Score { get; }

    public RectangleF Position { get; }

    public DetectResult(string label, float score, RectangleF position)
    {
        Label = label;
        Score = score;
        Position = position;
    }
}

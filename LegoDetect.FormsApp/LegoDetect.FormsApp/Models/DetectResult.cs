namespace LegoDetect.FormsApp.Models;

using System.Drawing;

public class DetectResult
{
    public string Label { get; }

    public float Score { get; }

    public Rectangle Rectangle { get; }

    public DetectResult(string label, float score, Rectangle rectangle)
    {
        Label = label;
        Score = score;
        Rectangle = rectangle;
    }
}

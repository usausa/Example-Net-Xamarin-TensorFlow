namespace LegoDetect.FormsApp.Services;

using LegoDetect.FormsApp.Models;

public interface IObjectDetectService
{
    Task<DetectResult[]> DetectAsync(byte[] bytes);
}

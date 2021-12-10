namespace LegoDetect.FormsApp.Droid.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Android.Content;
using Android.Graphics;
using Android.OS;

using Java.IO;
using Java.Nio;
using Java.Nio.Channels;

using LegoDetect.FormsApp.Models;
using LegoDetect.FormsApp.Services;

using Xamarin.TensorFlow.Lite;

public class ObjectDetectService : IObjectDetectService
{
    private readonly string[] labels;

    private readonly Interpreter interpreter;

    private readonly int width;
    private readonly int height;

    private readonly int detections;

    private readonly int[] pixels;
    private readonly ByteBuffer imageBuffer;

    public ObjectDetectService(Context context)
    {
        var sr = new StreamReader(context.Assets!.Open("labels.txt"));
        labels = sr.ReadToEnd().Split('\n');

        var assetDescriptor = context.Assets!.OpenFd("model.tflite");
        var inputStream = new FileInputStream(assetDescriptor.FileDescriptor);
        var mappedByteBuffer = inputStream.Channel!.Map(FileChannel.MapMode.ReadOnly, assetDescriptor.StartOffset, assetDescriptor.DeclaredLength);
        interpreter = new Interpreter(mappedByteBuffer, new Interpreter.Options());

        var inputTensor = interpreter.GetInputTensor(0);
        var inputShape = inputTensor.Shape();
        width = inputShape[1];
        height = inputShape[2];

        var outputTensor = interpreter.GetOutputTensor(0);
        var outputShape = outputTensor.Shape();
        detections = outputShape[0];

        pixels = new int[width * height];
        imageBuffer = ByteBuffer.AllocateDirect(4 * width * height * 3);
        imageBuffer.Order(ByteOrder.NativeOrder()!);
    }

    public Task<DetectResult[]> DetectAsync(byte[] bytes)
    {
        return Task.Run(() =>
        {
            Trace.BeginSection("Detect");

            // ProcessBitmap
            Trace.BeginSection("ProcessBitmap");

            var bitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length)!;
            var resized = Bitmap.CreateScaledBitmap(bitmap, width, height, true)!;
            resized.GetPixels(pixels, 0, resized.Width, 0, 0, resized.Width, resized.Height);

            imageBuffer.Rewind();
            var offset = 0;
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var value = pixels[offset++];
                    imageBuffer.PutFloat(value >> 16 & 0xFF);
                    imageBuffer.PutFloat(value >> 8 & 0xFF);
                    imageBuffer.PutFloat(value & 0xFF);
                }
            }

            bitmap.Recycle();
            resized.Recycle();

            Trace.EndSection();

            // Feed
            Trace.BeginSection("Feed");

            Java.Lang.Object[] inputs = { imageBuffer };

            var outputs = new Dictionary<Java.Lang.Integer, Java.Lang.Object>();

            var detectedBoxes = new float[detections][];
            for (var i = 0; i < detections; i++)
            {
                detectedBoxes[i] = new float[4];
            }
            var detectedClasses = new long[detections];
            var detectedScores = new float[detections];

            var p0 = Java.Lang.Object.FromArray(detectedBoxes)!;
            var p1 = Java.Lang.Object.FromArray(detectedClasses)!;
            var p2 = Java.Lang.Object.FromArray(detectedScores)!;

            outputs.Add(new Java.Lang.Integer(0), p0);
            outputs.Add(new Java.Lang.Integer(1), p1);
            outputs.Add(new Java.Lang.Integer(2), p2);

            Trace.EndSection();

            // Run
            Trace.BeginSection("Run");

            interpreter.RunForMultipleInputsOutputs(inputs, outputs);

            Trace.EndSection();

            // PostProcess
            Trace.BeginSection("PostProcess");

            detectedBoxes = p0.ToArray<float[]>()!;
            detectedClasses = p1.ToArray<long>()!;
            detectedScores = p2.ToArray<float>()!;

            var results = new DetectResult[detections];
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = new DetectResult(
                    labels[(int)detectedClasses[i]],
                    detectedScores[i],
                    new Bounds
                    {
                        Left = detectedBoxes[i][1] * bitmap.Width,
                        Top = detectedBoxes[i][0] * bitmap.Height,
                        Width = detectedBoxes[i][3] * bitmap.Width,
                        Height = detectedBoxes[i][2] * bitmap.Height
                    });
            }

            Trace.EndSection();

            Trace.EndSection();

            return results;
        });
    }
}

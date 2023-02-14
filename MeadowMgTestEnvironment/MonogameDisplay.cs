using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;

namespace MeadowMgTestEnvironment;

public class MonogameDisplay : IGraphicsDisplay
{
    private readonly MonogameApp _monogameApp;
    
    public ColorMode ColorMode { get; }
    public ColorMode SupportedColorModes => ColorMode;
    public int Width { get; }
    public int Height { get; }
    public IPixelBuffer PixelBuffer { get; }

    internal MonogameDisplay(int width, int height, MonogameApp monogameApp)
    {
        _monogameApp = monogameApp;
        Width = width;
        Height = height;
        
        PixelBuffer = new BufferRgb565(width, height);
        ColorMode = ColorMode.Format16bppRgb565;
    }
    
    public void Show()
    {
        // TODO: make more flexible
        // Convert pixel buffer to Rgba8888;
        var targetBuffer = new byte[Width * Height * 4];
        var sourceBuffer = PixelBuffer.Buffer;
        var sourceIndex = 0;
        var targetIndex = 0;
        while (sourceIndex < sourceBuffer.Length)
        {
            var pixel = (sourceBuffer[sourceIndex] << 8) | sourceBuffer[sourceIndex + 1];
            var red = (byte)(((pixel >> 11) & 0b11111) * (255f/31));
            var green = (byte)(((pixel >> 5) & 0b111111) * (255f/63));
            var blue = (byte)((pixel & 0b11111) * (255f/31));

            targetBuffer[targetIndex] = red;
            targetBuffer[targetIndex + 1] = green;
            targetBuffer[targetIndex + 2] = blue;
            targetBuffer[targetIndex + 3] = 255;

            sourceIndex += 2;
            targetIndex += 4;
        }
        
        _monogameApp.RgbaBufferQueue.Enqueue(targetBuffer);
    }

    public void Show(int left, int top, int right, int bottom)
    {
        throw new NotImplementedException();
    }

    public void Clear(bool updateDisplay = false)
    {
        PixelBuffer.Fill(Color.Black);
    }

    public void Fill(Color fillColor, bool updateDisplay = false)
    {
        PixelBuffer.Fill(fillColor);
    }

    public void Fill(int x, int y, int width, int height, Color fillColor)
    {
        PixelBuffer.Fill(x, y, width, height, fillColor);
    }

    public void DrawPixel(int x, int y, Color color)
    {
        throw new NotImplementedException();
    }

    public void DrawPixel(int x, int y, bool enabled)
    {
        throw new NotImplementedException();
    }

    public void InvertPixel(int x, int y)
    {
        throw new NotImplementedException();
    }

    public void WriteBuffer(int x, int y, IPixelBuffer displayBuffer)
    {
        throw new NotImplementedException();
    }
}
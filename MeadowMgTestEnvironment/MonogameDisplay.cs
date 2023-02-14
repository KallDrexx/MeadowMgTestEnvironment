using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;

namespace MeadowMgTestEnvironment;

public class MonogameDisplay : IGraphicsDisplay
{
    private readonly MonogameApp _monogameApp;
    private readonly TextureTransferer _textureTransferer;
    
    public ColorMode ColorMode { get; }
    public ColorMode SupportedColorModes => ColorMode;
    public int Width { get; }
    public int Height { get; }
    public IPixelBuffer PixelBuffer { get; }

    internal MonogameDisplay(int width, 
        int height, 
        MonogameApp monogameApp,
        TextureTransferer textureTransferer)
    {
        _monogameApp = monogameApp;
        _textureTransferer = textureTransferer;
        Width = width;
        Height = height;
        
        PixelBuffer = new BufferRgb565(width, height);
        ColorMode = ColorMode.Format16bppRgb565;
    }
    
    public void Show()
    {
        _textureTransferer.PushToTexture(PixelBuffer);
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
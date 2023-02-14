using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;

namespace MeadowMgTestEnvironment;

public class MonogameDisplay : IGraphicsDisplay
{
    private readonly TextureTransferer _textureTransferer;
    
    public ColorMode ColorMode { get; }
    public ColorMode SupportedColorModes => ColorMode;
    public int Width { get; }
    public int Height { get; }
    public IPixelBuffer PixelBuffer { get; }

    internal MonogameDisplay(int width, 
        int height, 
        TextureTransferer textureTransferer,
        ColorMode colorMode)
    {
        _textureTransferer = textureTransferer;
        Width = width;
        Height = height;
        
        ColorMode = colorMode;
        PixelBuffer = GetBufferForColorMode(colorMode, width, height);
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
        PixelBuffer.SetPixel(x, y, color);
    }

    public void DrawPixel(int x, int y, bool enabled)
    {
        if (PixelBuffer is Buffer1bpp buffer)
        {
            buffer.SetPixel(x, y, enabled);
        }
        else
        {
            var message = "Setting a pixel to enabled/disabled is only possible on " +
                          $"1bpp color modes, but this is currently in {ColorMode}";

            throw new InvalidOperationException(message);
        }
    }

    public void InvertPixel(int x, int y)
    {
        PixelBuffer.InvertPixel(x, y);
    }

    public void WriteBuffer(int x, int y, IPixelBuffer displayBuffer)
    {
        PixelBuffer.WriteBuffer(x, y, displayBuffer);
    }

    private static IPixelBuffer GetBufferForColorMode(ColorMode colorMode, int width, int height)
    {
        return colorMode switch
        {
            ColorMode.Format1bpp => new Buffer1bpp(width, height),
            ColorMode.Format4bppGray => new BufferGray4(width, height),
            ColorMode.Format8bppGray => new BufferGray8(width, height),
            ColorMode.Format8bppRgb332 => new BufferRgb332(width, height),
            ColorMode.Format12bppRgb444 => new BufferRgb444(width, height),
            ColorMode.Format16bppRgb565 => new BufferRgb565(width, height),
            ColorMode.Format24bppRgb888 => new BufferRgb888(width, height),
            ColorMode.Format32bppRgba8888 => new BufferRgb8888(width, height),
            _ => throw new NotSupportedException($"Color mode {colorMode} is not supported"),
        };
    }
}
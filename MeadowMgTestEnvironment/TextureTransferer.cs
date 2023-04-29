using Meadow.Foundation.Graphics.Buffers;
using Microsoft.Xna.Framework.Graphics;

namespace MeadowMgTestEnvironment;

/// <summary>
/// Used to push pixel data from a pixel buffer to a Monogame Texture2d
/// buffer, which can then be rendered inside monogame.
/// </summary>
internal class TextureTransferer
{
    private readonly object _padlock = new();
    private readonly BufferRgba8888 _buffer;

    public TextureTransferer(int width, int height)
    {
        _buffer = new BufferRgba8888(width, height);
    }

    /// <summary>
    /// Converts the pixel data into RGBA8888 (to be monogame compatible) and
    /// saves the pixel data into storage.
    /// </summary>
    public void PushToTexture(IPixelBuffer newData)
    {
        lock (_padlock)
        {
            if (newData.Width != _buffer.Width ||
                newData.Height != _buffer.Height)
            {
                var message = $"Expected buffer with dimensions of {_buffer.Width}x{_buffer.Height} " +
                              $"but received buffer with dimensions {newData.Width}x{newData.Height}";

                throw new InvalidOperationException(message);
            }

            _buffer.WriteBuffer(0, 0, newData);
        }
    }

    public void PushToTexture(IPixelBuffer frameBuffer, int left, int top, int right, int bottom)
    {
        left = Math.Max(left, 0);
        right = Math.Min(right, frameBuffer.Width);
        top = Math.Max(top, 0);
        bottom = Math.Min(bottom, frameBuffer.Height);
        var width = right - left;
        var height = bottom - top;

        if (width <= 0 || height <= 0)
        {
            // out of bounds
            return;
        }
        
        // We need a contiguous buffer that's *just* the pixels we want to write
        var tempBuffer = new BufferRgb565(height, width);

        for (var row = 0; row < height; row++)
        for (var col = 0; col < width; col++)
        {
            var sourceIndex = (top + row) * frameBuffer.Width * 2 + (left + col) * 2;
            var targetIndex = row * width * 2 + col * 2;

            tempBuffer.Buffer[targetIndex] = frameBuffer.Buffer[sourceIndex];
            tempBuffer.Buffer[targetIndex + 1] = frameBuffer.Buffer[sourceIndex + 1];
        }

        lock (_padlock)
        {
            _buffer.WriteBuffer(left, top, tempBuffer);
        }
    }

    /// <summary>
    /// Applies the currently saved RGBA8888 saved pixel data to the passed
    /// in monogame texture.
    /// </summary>
    /// <param name="texture"></param>
    public void SetTextureData(Texture2D texture)
    {
        lock (_padlock)
        {
            texture.SetData(_buffer.Buffer);
        }
    }
}
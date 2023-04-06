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
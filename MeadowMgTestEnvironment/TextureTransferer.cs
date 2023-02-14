using Meadow.Foundation.Graphics.Buffers;
using Microsoft.Xna.Framework.Graphics;

namespace MeadowMgTestEnvironment;

internal class TextureTransferer
{
    private readonly object _padlock = new();
    private readonly BufferRgb8888 _buffer;

    public TextureTransferer(int width, int height)
    {
        _buffer = new BufferRgb8888(width, height);
    }

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

    public void SetTextureData(Texture2D texture)
    {
        lock (_padlock)
        {
            texture.SetData(_buffer.Buffer);
        }
    }
}
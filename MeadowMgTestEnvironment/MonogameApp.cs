using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MeadowMgTestEnvironment;

internal class MonogameApp : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly int _textureWidth, _textureHeight;
    private readonly InputTracker _inputTracker;
    private readonly TextureTransferer _textureTransferer;
    private SpriteBatch _spriteBatch = null!;
    private Texture2D _texture = null!;
    
    public MonogameApp(int textureWidth, 
        int textureHeight, 
        InputTracker inputTracker,
        TextureTransferer textureTransferer)
    {
        _inputTracker = inputTracker;
        _graphics = new GraphicsDeviceManager(this);
        
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _textureWidth = textureWidth;
        _textureHeight = textureHeight;
        _textureTransferer = textureTransferer;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _texture = new Texture2D(_graphics.GraphicsDevice, _textureWidth, _textureHeight);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        
        _inputTracker.CheckTrackedKeys();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _textureTransferer.SetTextureData(_texture);
        
        _spriteBatch.Begin();
        _spriteBatch.Draw(_texture,
            ComputeDestinationRectangle(),
            new Rectangle(0, 0, _textureWidth, _textureHeight),
            Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private Rectangle ComputeDestinationRectangle()
    {
        var textureAspectRatio = (float)_textureWidth / _textureHeight;
        var viewportAspectRatio = (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
        var startX = 0;
        var startY = 0;
        int height, width;

        if (textureAspectRatio > viewportAspectRatio)
        {
            // texture has a wider aspect ratio than the viewport, texture is width constrained.
            width = GraphicsDevice.Viewport.Width;
            height = (int)Math.Round(width * textureAspectRatio);
            startY = (GraphicsDevice.Viewport.Height - height) / 2;
        }
        else
        {
            // Viewport has a wider aspect ratio than the texture, texture is height constrained.
            height = GraphicsDevice.Viewport.Height;
            width = (int)Math.Round(height * textureAspectRatio);
            startX = (GraphicsDevice.Viewport.Width - width) / 2;
        }

        return new Rectangle(startX, startY, width, height);
    }
}
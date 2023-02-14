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
            new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
            new Rectangle(0, 0, _textureWidth, _textureHeight),
            Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
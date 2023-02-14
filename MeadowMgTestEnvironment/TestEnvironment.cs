using Microsoft.Xna.Framework.Input;

namespace MeadowMgTestEnvironment;

public class TestEnvironment
{
    private readonly InputTracker _inputTracker = new();

    public MonogameDisplay Display { get; }

    public TestEnvironment(int width, int height)
    {
        var textureTransferer = new TextureTransferer(width, height);
        var monogameApp = new MonogameApp(width, height, _inputTracker, textureTransferer);
        Display = new MonogameDisplay(width, height, monogameApp, textureTransferer);
        
        new Thread(() => monogameApp.Run()).Start();
        monogameApp.Exiting += (_, _) => Environment.Exit(0);
    }

    public void BindKey(Keys key, Action onPress, Action onRelease)
    {
        _inputTracker.RegisterKey(key, onPress, onRelease);
    }
}
using Microsoft.Xna.Framework.Input;

namespace MeadowMgTestEnvironment;

public class TestEnvironment
{
    private readonly InputTracker _inputTracker = new();
    private readonly MonogameApp _monogameApp;
    
    public MonogameDisplay Display { get; }

    public TestEnvironment(int width, int height)
    {
        _monogameApp = new MonogameApp(width, height, _inputTracker);
        Display = new MonogameDisplay(width, height, _monogameApp);
        
        new Thread(() => _monogameApp.Run()).Start();
    }

    public void BindKey(Keys key, Action onPress, Action onRelease)
    {
        _inputTracker.RegisterKey(key, onPress, onRelease);
    }
}
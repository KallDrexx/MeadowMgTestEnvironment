using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Microsoft.Xna.Framework.Input;

namespace MeadowMgTestEnvironment;

/// <summary>
/// Provides a Monogame based test environment to emulate input mechanisms and display units
/// for a meadow application.
/// </summary>
public class TestEnvironment
{
    private readonly InputTracker _inputTracker = new();

    public MonogameDisplay Display { get; }

    /// <summary>
    /// Creates a new test environment with the resolution of the meadow display to mimic.
    /// This should match the same resolution the physical device will render its
    /// display at. The graphical display will be scaled to the size of the window
    /// (maintaining aspect ratio).
    /// </summary>
    public TestEnvironment(int meadowWidth, int meadowHeight)
    {
        var textureTransferer = new TextureTransferer(meadowWidth, meadowHeight);
        var monogameApp = new MonogameApp(meadowWidth, 
            meadowHeight, 
            _inputTracker, 
            textureTransferer);
        
        Display = new MonogameDisplay(meadowWidth, meadowHeight, textureTransferer, ColorMode.Format16bppRgb565);
        
        new Thread(() => monogameApp.Run()).Start();
        monogameApp.Exiting += (_, _) => Environment.Exit(0);
    }

    /// <summary>
    /// Creates a digital input port for the specified keyboard key
    /// </summary>
    public IDigitalInterruptPort CreatePortForKey(Keys key)
    {
        return _inputTracker.RegisterKey(key);
    }
}
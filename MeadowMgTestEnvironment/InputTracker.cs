using System.Collections.Concurrent;
using Meadow.Hardware;
using Microsoft.Xna.Framework.Input;

namespace MeadowMgTestEnvironment;

internal class InputTracker
{
    private readonly ConcurrentDictionary<Keys, KeyboardPort> _trackedKeys = new();

    public IDigitalInterruptPort RegisterKey(Keys key)
    {
        var port = new KeyboardPort();
        _trackedKeys[key] = port;

        return port;
    }

    public void CheckTrackedKeys()
    {
        var keyboardState = Keyboard.GetState();
        var allKeys = _trackedKeys.Keys;
        foreach (var key in allKeys)
        {
            if (!_trackedKeys.TryGetValue(key, out var port))
            {
                continue;
            }

            port.State = keyboardState.IsKeyDown(key);
        }
    }
}
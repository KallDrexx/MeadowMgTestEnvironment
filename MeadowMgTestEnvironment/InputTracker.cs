using System.Collections.Concurrent;
using Microsoft.Xna.Framework.Input;

namespace MeadowMgTestEnvironment;

internal class InputTracker
{
    private readonly ConcurrentDictionary<Keys, TrackedKey> _trackedKeys = new();

    public void RegisterKey(Keys key, Action onPress, Action onDepress)
    {
        _trackedKeys[key] = new TrackedKey(onPress, onDepress);
    }

    public void CheckTrackedKeys()
    {
        var keyboardState = Keyboard.GetState();
        var allKeys = _trackedKeys.Keys;
        foreach (var key in allKeys)
        {
            if (!_trackedKeys.TryGetValue(key, out var state))
            {
                continue;
            }
            
            var isDown = keyboardState.IsKeyDown(key);
            if (isDown != state.IsCurrentlyPressed)
            {
                state.IsCurrentlyPressed = isDown;
                if (isDown)
                {
                    state.OnPress();
                }
                else
                {
                    state.OnDepress();
                }
            }
        }
    }

    private class TrackedKey
    {
        public Action OnPress { get; }
        public Action OnDepress { get; }
        public bool IsCurrentlyPressed { get; set; }

        public TrackedKey(Action onPress, Action onDepress)
        {
            OnPress = onPress;
            OnDepress = onDepress;
        }
    }
}
# Meadow Monogame Test Environment

When developing applications for [Wilderness Labs devices](https://www.wildernesslabs.co/), the dev/test cycle can be time consuming when every build needs to be deployed before testing.

This library allows using the Monogame framework to allow using your development PC to run the main application code immediately. It provides mechanisms to hook a Monogame display into your meadow application (using the `IGraphicsDisplay` interface) to show the same graphical output as would show on the meadow device. It also allows using Monogame's input state management to use keyboard buttons to trigger input states in the application.

## Example

Below is an example of using the Monogame test environment to validate rendering via the Meadow MicroGraphics API:

```csharp
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using MeadowMgTestEnvironment;
using Microsoft.Xna.Framework.Input;

// Create the Monogame test environment that mimics a 240x240 pixel display
var environment = new TestEnvironment(240, 240)
{
    Display =
    {
        // Make sure each `Show()` call on the display sleeps for 16ms, so that 
        // the while loop only executes only 60 times a second.
        SleepAfterShow = TimeSpan.FromMilliseconds(16),
    }
};

// Create a new MicroGraphics instance, and have it push its pixel data
// to the test environment's `IGraphicsDisplay` instance, so it renders
// to the running Monogame window.
var renderer = new MicroGraphics(environment.Display)
{
    CurrentFont = new Font8x8()
};

var spacePressed = false;

// When the user presses the space bar, set `spacePressed` to true.
// Once the user releases the space bar, set it back to false.
environment.BindKey(Keys.Space,
    () => spacePressed = true,
    () => spacePressed = false);

while (true)
{
    renderer.Clear(Color.Black);
    
    renderer.PenColor = Color.White;
    renderer.DrawText(10, 10, DateTime.Now.ToString("h:mm:ss.ffffff"));
    if (spacePressed)
    {
        renderer.DrawText(10, 200, "Space pressed!");
    }
    
    renderer.Show();
}
```
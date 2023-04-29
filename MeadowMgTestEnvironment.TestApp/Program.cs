using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Buttons;
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
var spacePort = environment.CreatePortForKey(Keys.Space);
var spaceButton = new PushButton(spacePort);
spaceButton.PressStarted += (_, _) => spacePressed = true;
spaceButton.PressEnded += (_, _) => spacePressed = false;

renderer.Clear(Color.Black);
renderer.Show();

while (true)
{
    renderer.Clear(Color.Red);
    renderer.DrawRoundedRectangle(0, 0, 240, 239, 5, Color.Red);
    renderer.DrawCircle(50, 50, 25, Color.Aqua);
    
    renderer.PenColor = Color.White;
    renderer.DrawText(10, 10, DateTime.Now.ToString("h:mm:ss.ffffff"));
    if (spacePressed)
    {
        renderer.DrawText(10, 200, "Space pressed!");
    }
    
    renderer.Show(40, 40, 55, 55);
}

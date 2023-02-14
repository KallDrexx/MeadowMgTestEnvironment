using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using MeadowMgTestEnvironment;
using Microsoft.Xna.Framework.Input;

var environment = new TestEnvironment(240, 240);
var renderer = new MicroGraphics(environment.Display)
{
    CurrentFont = new Font8x8()
};

var spacePressed = false;

environment.BindKey(Keys.Space,
    () => spacePressed = true,
    () => spacePressed = false);

while (true)
{
    renderer.Clear(Color.Black);
    renderer.DrawText(10, 10, DateTime.Now.ToString("h:mm:ss.ffffff"));
    if (spacePressed)
    {
        renderer.DrawText(10, 200, "Space pressed!");
    }
    
    renderer.Show();
    
    Thread.Sleep(16);
}

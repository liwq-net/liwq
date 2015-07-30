using liwq;
using Microsoft.Xna.Framework;

public class App : Application
{
    public App(Game game, GraphicsDeviceManager graphics)
        : base(game, graphics)
    {

        //#if WINDOWS
        //        preferredWidth = 960;
        //        preferredHeight = 540;
        //#endif
        //        graphics.PreferredBackBufferWidth = preferredWidth;
        //        graphics.PreferredBackBufferHeight = preferredHeight;
    }

    public override bool ApplicationDidFinishLaunching()
    {
        //var resPolicy = CCResolutionPolicy.ShowAll; // This will stretch out your game
        //CCDrawManager.SetDesignResolutionSize(1280, 720, resPolicy);
        //pDirector.DisplayStats = true;
        //pDirector.AnimationInterval = 1.0 / 60;

        liwq.test.TestScene scene = new liwq.test.TestScene();
        this.RunWithScene(scene);
        return true;
    }

}
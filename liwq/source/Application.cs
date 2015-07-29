using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;

namespace liwq
{
    public class Application : DrawableGameComponent
    {
        static public Application SharedApplication { get; protected set; }

        public GraphicsDeviceManager GraphicsDeviceManager { get; protected set; }
        public SpriteBatch SpriteBatch { get; protected set; }

        public Application(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            this.GraphicsDeviceManager = graphics;
            Microsoft.Xna.Framework.Input.Touch.TouchPanel.EnabledGestures = GestureType.Tap;
            this.ScreenScaleFactor = 1.0f;

            this.Game.Activated += (s, e) => { this.ApplicationWillEnterForeground(); };
            this.Game.Deactivated += (s, e) => { this.ApplicationDidEnterBackground(); };
            this.Game.Exiting += (s, e) => { };

            //·´¾â³Ý
            graphics.PreparingDeviceSettings += (s, e) =>
            {
                e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 4;
            };
        }

        //---------------------------------------------------------------------
        //DrawableGameComponent

        public override void Initialize()
        {
            SharedApplication = this;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
            this.ApplicationDidFinishLaunching();
        }

        public override void Update(GameTime gameTime)
        {
            //if (Director.SharedDirector.IsPaused == false)
            //{
            //    CCScheduler.sharedScheduler().tick((float)gameTime.ElapsedGameTime.TotalSeconds);
            //}
            if (this.RunningScene != null)
            {
                this.RunningScene.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (this._animationStoped == false)
            {
                base.Draw(gameTime);
                if (this.RunningScene != null)
                {
                    this.RunningScene.Draw(gameTime);
                }
            }
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        //---------------------------------------------------------------------
        //properties

        /// <summary>
        ///  = 1/FPS
        /// </summary>
        public double AnimationInterval
        {
            set { this.Game.TargetElapsedTime = TimeSpan.FromSeconds(value); }
            get { return this.Game.TargetElapsedTime.Milliseconds / 1000.0; }
        }

        public bool IsPaused { get; protected set; }

        public DisplayOrientation Orientation
        {
            set
            {
                int w = GraphicsDevice.Viewport.Width;
                int h = GraphicsDevice.Viewport.Height;
                if (w > h) { int temp = h; h = w; w = temp; }
                switch (value)
                {
                    case DisplayOrientation.LandscapeLeft:
                        GraphicsDeviceManager.PreferredBackBufferWidth = h;
                        GraphicsDeviceManager.PreferredBackBufferHeight = w;
                        GraphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft;
                        GraphicsDeviceManager.ApplyChanges();
                        break;
                    case DisplayOrientation.LandscapeRight:
                        GraphicsDeviceManager.PreferredBackBufferWidth = h;
                        GraphicsDeviceManager.PreferredBackBufferHeight = w;
                        GraphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeRight;
                        GraphicsDeviceManager.ApplyChanges();
                        break;
                    default:
                        GraphicsDeviceManager.PreferredBackBufferWidth = w;
                        GraphicsDeviceManager.PreferredBackBufferHeight = h;
                        GraphicsDeviceManager.SupportedOrientations = DisplayOrientation.Portrait;
                        GraphicsDeviceManager.ApplyChanges();
                        break;
                }
            }
        }

        public Size Size { get { return new Size(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); } }

        public float ScreenScaleFactor { get; set; }

        public Size DesignSize
        {
            get { return SharedApplication.Size; }
        }

        public Size DisplaySize
        {
            get { return new Size(this.DesignSize.Width * this.ScreenScaleFactor, this.DesignSize.Height * this.ScreenScaleFactor); }
        }

        //---------------------------------------------------------------------

        protected bool _animationStoped;
        public void StopAnimation() { this._animationStoped = true; }
        public void StartAnimation() { this._animationStoped = false; }

        public virtual bool ApplicationDidFinishLaunching() { return false; }
        public virtual void ApplicationDidEnterBackground() { }
        public virtual void ApplicationWillEnterForeground() { }

        //---------------------------------------------------------------------
        //scene manager
        protected List<Node> _scenesStack = new List<Node>();

        public Node RunningScene { get; protected set; }

        public void PushScene(Node scene)
        {
            this._scenesStack.Add(scene);
            this.RunningScene = scene;
        }

        public void PopScene()
        {
            if (this._scenesStack.Count > 0)
            {
                this._scenesStack.RemoveAt(this._scenesStack.Count - 1);
            }
            if (this._scenesStack.Count == 0)
            {
                //½áÊø
            }
        }

        public void RunWithScene(Node scene)
        {
            if (this._scenesStack.Count > 0)
                this.PopScene();
            this.PushScene(scene);
        }
    }
}

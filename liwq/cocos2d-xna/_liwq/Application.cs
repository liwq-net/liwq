/* 2dx复杂，不容易修改扩展,liwq改进包括
 * 简化引擎，把多余的去掉，把必须的补回来，简化代码编写，优化性能，添加WPF风格UI
 * 1、重构所有代码，统一使用UI左上角坐标
 * 2、Director 与 AppDelegate合并一个"大总管"
 * 3、Texture，Sprite，SpriteFrame 合并
 * 4、优化贴图性能，添加贴图动态合并功能（合成一张大贴图共用，发挥batch效能）。基于场景（node)的texture缓存，node释放，贴图释放
 * 5、废掉原来的menu等UI系统，废掉Layer，只基于Node以及上层的WPF风格UI（矢量控件）
 * 6、添加矢量字体以及高速的点阵字体（矢量字体选用stb ttf或者xmlreader方式的svg字体）
 * 7、保留 actions，particle system，scenes_transitions
 * 8、废弃xnb模式资源管理模式，支持ogg，png，gif,jpg，压缩贴图资源
 * 9、添加事件驱动模式（省电）
 * 10、actions 改为begin end连续函数控制
 * 11、支持xaml设计，支持metro风格控件
 * 12、.net风格的事件绑定模式
 * 12、3D模型支持（后期）
 * 13、XNAVG支持（后期）
 * 14、适配手机特有功能（震动，gps，webview，水平仪，相机调用）（后期)
 * 
 * 
 * 
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;

namespace cocos2d
{
    public abstract class Application : Microsoft.Xna.Framework.DrawableGameComponent
    {
        static public Application SharedApplication { get; protected set; }

        public Microsoft.Xna.Framework.Game Game { get; protected set; }
        public Microsoft.Xna.Framework.GraphicsDeviceManager GraphicsDeviceManager { get; protected set; }
        public SpriteBatch SpriteBatch { get; protected set; }
        public BasicEffect BasicEffect { get; protected set; }
        public Matrix WorldMatrix { get; set; }
        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }

        public Application(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            this.Game = game;
            this.GraphicsDeviceManager = graphics;
            Microsoft.Xna.Framework.Input.Touch.TouchPanel.EnabledGestures = GestureType.Tap;
        }

        //---------------------------------------------------------------------
        //DrawableGameComponent methods

        public override void Initialize()
        {
            SharedApplication = this;
            this.initInstance();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);
            this.BasicEffect = new BasicEffect(GraphicsDevice);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            base.LoadContent();
            this.applicationDidFinishLaunching();
        }

        public override void Update(GameTime gameTime)
        {
            this.ProcessTouch();
            if (Director.SharedDirector.IsPaused == false)
            {
                CCScheduler.sharedScheduler().tick((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.BasicEffect.View = ViewMatrix;
            this.BasicEffect.World = WorldMatrix;
            this.BasicEffect.Projection = ProjectionMatrix;
            Director.SharedDirector.MainLoop(gameTime);
            base.Draw(gameTime);
        }

        //---------------------------------------------------------------------
        //properties

        public float ScreenScaleFactor { get; set; }

        /// <summary>Callback by CCDirector for limit FPS</summary>
        public double AnimationInterval
        {
            set { Game.TargetElapsedTime = TimeSpan.FromSeconds(value); }
            get { return this.Game.TargetElapsedTime.Milliseconds / 1000.0; }
        }

        public CCSize Size { get { return new CCSize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); } }

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

        //---------------------------------------------------------------------
        //AppDelegate Methods

        /// <summary>Implement for initialize OpenGL instance, set source path, etc...</summary>
        public virtual bool initInstance() { return true; }

        /// <summary>Implement CCDirector and CCScene init code here.</summary>
        /// <returns>
        ///     return true    Initialize success, app continue.
        ///     return false   Initialize failed, app terminate.
        /// </returns>
        public virtual bool applicationDidFinishLaunching() { return false; }

        /// <summary>The function be called when the application enter background</summary>
        public virtual void applicationDidEnterBackground() { }

        /// <summary>The function be called when the application enter foreground</summary>
        public virtual void applicationWillEnterForeground() { }


        #region Touch Methods

        protected LinkedList<CCTouch> _touchLink = new LinkedList<CCTouch>();
        protected Dictionary<int, LinkedListNode<CCTouch>> _touchMap = new Dictionary<int, LinkedListNode<CCTouch>>();

        // http://www.cocos2d-x.org/boards/17/topics/10777
        public void ClearTouches()
        {
            this._touchLink.Clear();
            this._touchMap.Clear();
        }

        public IEGLTouchDelegate TouchDelegate { set; protected get; }

        private void ProcessTouch()
        {
            if (this.TouchDelegate != null)
            {
                TouchCollection touchCollection = TouchPanel.GetState();

                List<CCTouch> newTouches = new List<CCTouch>();
                List<CCTouch> movedTouches = new List<CCTouch>();
                List<CCTouch> endedTouches = new List<CCTouch>();

                //todo 为什么不直接用 GraphicsDevice.Viewport
                Rectangle viewPort = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

                foreach (TouchLocation touch in touchCollection)
                {
                    switch (touch.State)
                    {
                        case TouchLocationState.Pressed:
                            if (viewPort.Contains((int)touch.Position.X, (int)touch.Position.Y))
                            {
                                this._touchLink.AddLast(new CCTouch(touch.Id, touch.Position.X - viewPort.Left / ScreenScaleFactor, touch.Position.Y - viewPort.Top / ScreenScaleFactor));
                                this._touchMap[touch.Id] = this._touchLink.Last;
                                newTouches.Add(this._touchLink.Last.Value);
                            }
                            break;

                        case TouchLocationState.Moved:
                            if (this._touchMap.ContainsKey(touch.Id))
                            {
                                movedTouches.Add(this._touchMap[touch.Id].Value);
                                this._touchMap[touch.Id].Value.SetTouchInfo(
                                    touch.Id,
                                    touch.Position.X - viewPort.Left / ScreenScaleFactor,
                                    touch.Position.Y - viewPort.Top / ScreenScaleFactor
                                    );
                            }
                            break;

                        case TouchLocationState.Released:
                            if (this._touchMap.ContainsKey(touch.Id))
                            {
                                endedTouches.Add(this._touchMap[touch.Id].Value);
                                this._touchLink.Remove(this._touchMap[touch.Id]);
                                this._touchMap.Remove(touch.Id);
                            }
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (newTouches.Count > 0) { this.TouchDelegate.touchesBegan(newTouches, null); }
                if (movedTouches.Count > 0) { this.TouchDelegate.touchesMoved(movedTouches, null); }
                if (endedTouches.Count > 0) { this.TouchDelegate.touchesEnded(endedTouches, null); }
            }
        }

        private CCTouch getTouchBasedOnID(int nID)
        {
            if (this._touchMap.ContainsKey(nID))
            {
                LinkedListNode<CCTouch> curTouch = this._touchMap[nID];
                //If ID's match...
                if (curTouch.Value.view() == nID)
                {
                    //return the corresponding touch
                    return curTouch.Value;
                }
            }
            //If we reached here, we found no touches
            //matching the specified id.
            return null;
        }

        #endregion
    }
}

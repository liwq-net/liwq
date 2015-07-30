using System;
using liwq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace liwq
{
    public class Sprite : Node
    {
        public Texture2D Texture2D { get; protected set; }
        public Rect SourceRectangle { get; protected set; }
        public bool IsRotate { get; protected set; }

        public Sprite(Texture2D texture2d, Rect? rect = null, bool isRotated = false)
        {
            this.Texture2D = texture2d;
            if (rect == null)
                this.SourceRectangle = new Rect(0, 0, this.Texture2D.Width, this.Texture2D.Height);
            else
                this.SourceRectangle = rect.Value;
            this.IsRotate = isRotated;
        }
        public Sprite(byte[] data, int width, int height, Rect? rect = null, bool isRotated = false)
        {
            this.Texture2D = new Texture2D(Application.SharedApplication.GraphicsDevice, width, height, false, SurfaceFormat.Color);
            this.Texture2D.SetData<byte>(data);
            if (rect == null)
                this.SourceRectangle = new Rect(0, 0, this.Texture2D.Width, this.Texture2D.Height);
            else
                this.SourceRectangle = rect.Value;
            this.IsRotate = isRotated;
        }
        public Sprite(string assetName, Rect? rect = null, bool isRotated = false)
        {
            this.Texture2D = Application.SharedApplication.Game.Content.Load<Texture2D>(assetName);
            if (rect == null)
                this.SourceRectangle = new Rect(0, 0, this.Texture2D.Width, this.Texture2D.Height);
            else
                this.SourceRectangle = rect.Value;
            this.IsRotate = isRotated;
        }

        public Sprite Clone()
        {
            return new Sprite(this.Texture2D, this.SourceRectangle, this.IsRotate);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (this.Texture2D != null)
            {
                Application.SharedApplication.SpriteBatch.Begin();
                Application.SharedApplication.SpriteBatch.Draw(
                    this.Texture2D,
                    new Vector2(this.Position.X, this.Position.Y),
                    this.SourceRectangle,
                    Color.White,
                    this.Rotation,
                    new Vector2(this.AnchorPoint.X, this.AnchorPoint.Y),
                    new Vector2(this.ScaleX, this.ScaleY),
                    new SpriteEffects(),
                    0
                    );
                Application.SharedApplication.SpriteBatch.End();
            }
        }
    }
}
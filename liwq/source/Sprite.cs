using System;
using liwq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace liwq
{
    public class Sprite : Node
    {
        public Texture2D Texture2D { get; protected set; }

        //在贴图中的引用位置，以及是否旋转
        public Rect SourceRectangle { get; protected set; }
        public bool IsRotate { get; protected set; }
        //打包工具会把图片周围多余的透明色去掉。
        //于是裁剪后的起始点为CropPosition,渲染时应该 (-CropPosition.X,-CropPosition.Y)
        //没被裁剪的原图大小为UnCropSize
        public Size UnCropSize { get; protected set; }
        public Point CropPosition { get; protected set; }

        public bool FlipX { get; set; }
        public bool FlipY { get; set; }

        private void setTextureProperty(Rect? rect = null, bool isRotated = false, Size? unCropSize = null, Point? cropPosition = null)
        {
            if (rect == null)
                this.SourceRectangle = new Rect(0, 0, this.Texture2D.Width, this.Texture2D.Height);
            else
                this.SourceRectangle = rect.Value;
            this.IsRotate = isRotated;
            if (unCropSize != null)
                this.UnCropSize = unCropSize.Value;
            if (cropPosition != null)
                this.CropPosition = cropPosition.Value;
        }
        public Sprite(Texture2D texture2d, Rect? rect = null, bool isRotated = false, Size? unCropSize = null, Point? cropPosition = null)
        {
            this.Texture2D = texture2d;
            this.setTextureProperty(rect, isRotated, unCropSize, cropPosition);
        }
        public Sprite(byte[] data, int width, int height, Rect? rect = null, bool isRotated = false, Size? unCropSize = null, Point? cropPosition = null)
        {
            this.Texture2D = new Texture2D(Application.SharedApplication.GraphicsDevice, width, height, false, SurfaceFormat.Color);
            this.Texture2D.SetData<byte>(data);
            this.setTextureProperty(rect, isRotated, unCropSize, cropPosition);
        }
        public Sprite(string assetName, Rect? rect = null, bool isRotated = false, Size? unCropSize = null, Point? cropPosition = null)
        {
            this.Texture2D = Application.SharedApplication.Game.Content.Load<Texture2D>(assetName);
            this.setTextureProperty(rect, isRotated, unCropSize, cropPosition);
        }

        public Sprite Clone()
        {
            return new Sprite(this.Texture2D, this.SourceRectangle, this.IsRotate, this.UnCropSize, this.CropPosition);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (this.Texture2D != null)
            {
                Application.SharedApplication.SpriteBatch.Begin();
                Application.SharedApplication.SpriteBatch.Draw(
                    this.Texture2D,
                    new Vector2(this.Position.X - this.CropPosition.X, this.Position.Y - this.CropPosition.Y),
                    this.SourceRectangle,
                    Color.White,
                    (this.Rotation + (this.IsRotate == true ? -90 : 0)) * (float)(Math.PI / 180),
                    new Vector2(this.AnchorPoint.X, this.AnchorPoint.Y),
                    new Vector2(this.ScaleX, this.ScaleY),
                    (this.FlipX == true ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (this.FlipY == true ? SpriteEffects.FlipVertically : SpriteEffects.None),
                    this.ZOrder
                    );
                Application.SharedApplication.SpriteBatch.End();
            }
        }
    }
}
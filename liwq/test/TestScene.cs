
using Microsoft.Xna.Framework.Graphics;
namespace liwq.test
{
    public class TestScene : Node
    {
        public TestScene()
        {
            string textureInfo = System.IO.File.ReadAllText("Content/pack.xml");
            Texture2D texture = App.SharedApplication.Game.Content.Load<Texture2D>("pack");
            SpriteFactory.SharedSpriteFactory.AddSpritePack(textureInfo, texture);

            //Sprite button = new Sprite("button");
            //button.AnchorPoint = Point.AnchorCenter;
            //button.Rotation = 90;
            //button.Position = Application.SharedApplication.DisplaySize.Center;
            //this.AddChild(button);

            Sprite cannon = SpriteFactory.SharedSpriteFactory["player-new-badge.png"];
            cannon.Position = Application.SharedApplication.DisplaySize.Center;
            this.AddChild(cannon);
        }
    }
}

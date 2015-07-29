
namespace liwq.test
{
    public class TestScene : Node
    {
        public TestScene()
        {
            Sprite sp = new Sprite("button", new Rect(20, 20, 32, 32));
            sp.AnchorPoint = Point.AnchorCenter;
            sp.Rotation = 45;
            sp.Position = Application.SharedApplication.DisplaySize.Center;
            this.AddChild(sp);
        }
    }
}

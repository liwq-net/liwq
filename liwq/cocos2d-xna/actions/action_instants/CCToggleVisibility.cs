using System;
namespace cocos2d
{

    /** @brief Toggles the visibility of a node
    */
    public class CCToggleVisibility : CCActionInstant
    {
        public CCToggleVisibility()
        {
        }

        ~CCToggleVisibility()
        {
        }

        public static new CCToggleVisibility action()
        {
            CCToggleVisibility pRet = new CCToggleVisibility();

            return pRet;
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            pTarget.Visible = !pTarget.Visible;
        }

    }
}
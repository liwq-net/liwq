using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public interface IEGLTouchDelegate
    {
        void touchesBegan(List<CCTouch> touches, CCEvent pEvent);
        void touchesMoved(List<CCTouch> touches, CCEvent pEvent);
        void touchesEnded(List<CCTouch> touches, CCEvent pEvent);
        void touchesCancelled(List<CCTouch> touches, CCEvent pEvent);
    }
}

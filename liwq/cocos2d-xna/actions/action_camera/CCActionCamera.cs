using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{

    /// <summary>Base class for CCCamera actions</summary>
    public class CCActionCamera : CCActionInterval
    {
        protected float _centerXOrig;
        protected float _centerYOrig;
        protected float _centerZOrig;

        protected float _eyeXOrig;
        protected float _eyeYOrig;
        protected float _eyeZOrig;

        protected float _upXOrig;
        protected float _upYOrig;
        protected float _upZOrig;


        public CCActionCamera()
        {
            this._centerXOrig = 0;
            this._centerYOrig = 0;
            this._centerZOrig = 0;

            this._eyeXOrig = 0;
            this._eyeYOrig = 0;
            this._eyeZOrig = 0;

            this._upXOrig = 0;
            this._upYOrig = 0;
            this._upZOrig = 0;
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);

            CCCamera camera = pTarget.Camera;
            camera.getCenterXYZ(out _centerXOrig, out _centerYOrig, out _centerZOrig);
            camera.getEyeXYZ(out _eyeXOrig, out _eyeYOrig, out _eyeZOrig);
            camera.getUpXYZ(out _upXOrig, out _upYOrig, out _upZOrig);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCReverseTime.actionWithAction(this);
        }
    }
}
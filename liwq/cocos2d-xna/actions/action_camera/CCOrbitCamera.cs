using System;

namespace cocos2d
{
    /// <summary>CCOrbitCamera action Orbits the camera around the center of the screen using spherical coordinates</summary>
    public class CCOrbitCamera : CCActionCamera
    {
        protected float _radius;
        protected float _deltaRadius;
        protected float _angleZ;
        protected float _deltaAngleZ;
        protected float _angleX;
        protected float _deltaAngleX;
        protected float _radZ;
        protected float _radDeltaZ;
        protected float _radX;
        protected float _radDeltaX;

        public CCOrbitCamera()
        {
            this._radius = 0.0f;
            this._deltaRadius = 0.0f;
            this._angleZ = 0.0f;
            this._deltaAngleZ = 0.0f;
            this._angleX = 0.0f;
            this._deltaAngleX = 0.0f;
            this._radZ = 0.0f;
            this._radDeltaZ = 0.0f;
            this._radX = 0.0f;
            this._radDeltaX = 0.0f;
        }

        /// <summary>initializes a CCOrbitCamera action with radius, delta-radius,  z, deltaZ, x, deltaX </summary>
        public CCOrbitCamera(float t, float radius, float deltaRadius, float angleZ, float deltaAngleZ, float angleX, float deltaAngleX)
        {
            if (initWithDuration(t))
            {
                _radius = radius;
                _deltaRadius = deltaRadius;
                _angleZ = angleZ;
                _deltaAngleZ = deltaAngleZ;
                _angleX = angleX;
                _deltaAngleX = deltaAngleX;
                _radDeltaZ = ccMacros.CC_DEGREES_TO_RADIANS(deltaAngleZ);
                _radDeltaX = ccMacros.CC_DEGREES_TO_RADIANS(deltaAngleX);
            }
        }

        /// <summary>
        /// positions the camera according to spherical coordinates 
        /// </summary>
        public void sphericalRadius(out float newRadius, out float zenith, out float azimuth)
        {
            float ex, ey, ez, cx, cy, cz, x, y, z;
            float r; // radius
            float s;

            CCCamera pCamera = Target.Camera;
            pCamera.getEyeXYZ(out ex, out  ey, out ez);
            pCamera.getCenterXYZ(out cx, out  cy, out  cz);

            x = ex - cx;
            y = ey - cy;
            z = ez - cz;

            r = (float)Math.Sqrt((float)Math.Pow(x, 2) + (float)Math.Pow(y, 2) + (float)Math.Pow(z, 2));
            s = (float)Math.Sqrt((float)Math.Pow(x, 2) + (float)Math.Pow(y, 2));
            if (s == 0.0f)
                s = ccMacros.FLT_EPSILON;
            if (r == 0.0f)
                r = ccMacros.FLT_EPSILON;

            zenith = (float)Math.Acos(z / r);
            if (x < 0)
                azimuth = (float)Math.PI - (float)Math.Sin(y / s);
            else
                azimuth = (float)Math.Sin(y / s);

            newRadius = r / CCCamera.getZEye();
        }

        public override void StartWithTarget(Node pTarget)
        {
            startWithTargetUsedByCCOrbitCamera(pTarget);
            //CCActionInterval::startWithTarget(pTarget);

            float r, zenith, azimuth;
            this.sphericalRadius(out r, out  zenith, out azimuth);

            if (float.IsNaN(_radius))
                _radius = r;
            if (float.IsNaN(_angleZ))
                _angleZ = ccMacros.CC_RADIANS_TO_DEGREES(zenith);
            if (float.IsNaN(_angleX))
                _angleX = ccMacros.CC_RADIANS_TO_DEGREES(azimuth);

            _radZ = ccMacros.CC_DEGREES_TO_RADIANS(_angleZ);
            _radX = ccMacros.CC_DEGREES_TO_RADIANS(_angleX);
        }

        public override void Update(float dt)
        {
            float r = (_radius + _deltaRadius * dt) * CCCamera.getZEye();
            float za = _radZ + _radDeltaZ * dt;
            float xa = _radX + _radDeltaX * dt;
            float i = (float)Math.Sin(za) * (float)Math.Cos(xa) * r + _centerXOrig;
            float j = (float)Math.Sin(za) * (float)Math.Sin(xa) * r + _centerYOrig;
            float k = (float)Math.Cos(za) * r + _centerZOrig;
            Target.Camera.setEyeXYZ(i, j, k);
        }

    }
}

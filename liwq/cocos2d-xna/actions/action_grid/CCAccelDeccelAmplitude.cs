using System;

namespace cocos2d
{
    public class CCAccelDeccelAmplitude : CCActionInterval
    {
        /// <summary>
        /// initializes the action with an inner action that has the amplitude property, and a duration time
        /// </summary>
        public bool initWithAction(CCAction pAction, float duration)
        {
            if (base.initWithDuration(duration))
            {
                m_fRate = 1.0f;
                m_pOther = pAction as CCActionInterval;

                return true;
            }

            return false;
        }

        public override void StartWithTarget(Node pTarget)
        {
            base.StartWithTarget(pTarget);
            m_pOther.StartWithTarget(pTarget);
        }

        public override void Update(float time)
        {
            float f = time * 2;

            if (f > 1)
            {
                f -= 1;
                f = 1 - f;
            }

            ((CCAccelDeccelAmplitude)(m_pOther)).setAmplitudeRate((float)Math.Pow(f, m_fRate));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCAccelDeccelAmplitude.actionWithAction(m_pOther.Reverse(), Duration);
        }

        /// <summary>
        /// creates the action with an inner action that has the amplitude property, and a duration time
        /// </summary>
        public static CCAccelDeccelAmplitude actionWithAction(CCAction pAction, float duration)
        {
            CCAccelDeccelAmplitude pRet = new CCAccelDeccelAmplitude();

            if (pRet.initWithAction(pAction, duration))
            {
                return pRet;
            }

            return null;
        }

        protected float m_fRate;

        /// <summary>
        /// get or set amplitude rate
        /// </summary>
        public float Rate
        {
            get { return m_fRate; }
            set { m_fRate = value; }
        }

        protected CCActionInterval m_pOther;
    }
}

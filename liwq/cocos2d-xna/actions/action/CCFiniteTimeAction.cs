namespace cocos2d
{
    public class CCFiniteTimeAction : CCAction
    {
        /// <summary>duration in seconds</summary>
        public float Duration { get; set; }

        public virtual CCFiniteTimeAction Reverse()
        {
            CCLog.Log("cocos2d: FiniteTimeAction#reverse: Implement me");
            return null;
        }
    }
}

using System.Diagnostics;

namespace cocos2d
{
    /// <summary>
    /// @brief CCFollow is an action that "follows" a node.
    /// Eg:
    /// layer->runAction(CCFollow::actionWithTarget(hero));
    /// Instead of using CCCamera as a "follower", use this action instead.
    /// @since v0.99.2
    /// </summary>
    public class CCFollow : CCAction
    {
        public CCFollow(Node followedNode, CCRect rect)
        {
            Debug.Assert(followedNode != null);

            _FollowedNode = followedNode;
            BoundarySet = true;
            _boundaryFullyCovered = false;

            CCSize winSize = Director.SharedDirector.DesignSize;
            _fullScreenSize = new CCPoint(winSize.Width, winSize.Height);
            _halfScreenSize = CCPointExtension.ccpMult(_fullScreenSize, 0.5f);

            _leftBoundary = -((rect.origin.x + rect.size.Width) - _fullScreenSize.x);
            _rightBoundary = -rect.origin.x;
            _leftBoundary = -rect.origin.y;
            _bottomBoundary = -((rect.origin.y + rect.size.Height) - _fullScreenSize.y);

            if (_rightBoundary < _leftBoundary)
            {
                // screen width is larger than world's boundary width
                //set both in the middle of the world
                _rightBoundary = _leftBoundary = (_leftBoundary + _rightBoundary) / 2;
            }
            if (_topBoundary < _bottomBoundary)
            {
                // screen width is larger than world's boundary width
                //set both in the middle of the world
                _topBoundary = _bottomBoundary = (_topBoundary + _bottomBoundary) / 2;
            }

            if ((_topBoundary == _bottomBoundary) && (_leftBoundary == _rightBoundary))
            {
                _boundaryFullyCovered = true;
            }
        }

        public CCFollow(Node followedNode)
        {
            Debug.Assert(followedNode != null);

            _FollowedNode = followedNode;
            BoundarySet = false;
            _boundaryFullyCovered = false;

            CCSize winSize = Director.SharedDirector.DesignSize;
            _fullScreenSize = new CCPoint(winSize.Width, winSize.Height);
            _halfScreenSize = CCPointExtension.ccpMult(_fullScreenSize, 0.5f);
        }

        //public override CCObject copyWithZone(CCZone zone)
        //{
        //    CCZone tempZone = zone;
        //    CCFollow ret = null;
        //    if (tempZone != null && tempZone.m_pCopyObject != null)
        //    {
        //        ret = (CCFollow)tempZone.m_pCopyObject;
        //    }
        //    else
        //    {
        //        ret = new CCFollow();
        //        tempZone = new CCZone(ret);
        //    }

        //    base.copyWithZone(tempZone);
        //    ret.Tag = this.Tag;

        //    return ret;
        //}

        public override void Step(float dt)
        {
            if (BoundarySet)
            {
                // whole map fits inside a single screen, no need to modify the position - unless map boundaries are increased
                if (_boundaryFullyCovered)
                {
                    return;
                }

                CCPoint tempPos = CCPointExtension.ccpSub(_halfScreenSize, _FollowedNode.Position);
                Target.Position = CCPointExtension.ccp(CCPointExtension.clampf(tempPos.x, _leftBoundary, _rightBoundary),
                                                          CCPointExtension.clampf(tempPos.y, _bottomBoundary, _topBoundary));
            }
            else
            {
                Target.Position = CCPointExtension.ccpSub(_halfScreenSize, _FollowedNode.Position);
            }
        }

        public override bool IsDone()
        {
            return !_FollowedNode.IsRunning;
        }

        public override void Stop()
        {
            Target = null;
            base.Stop();
        }

        /// <summary>
        /// creates the action with no boundary set
        /// </summary>
        public static CCFollow actionWithTarget(Node followedNode)
        {
            return new CCFollow(followedNode);
        }

        /// <summary>
        /// creates the action with a set boundary
        /// </summary>
        public static CCFollow actionWithTarget(Node followedNode, CCRect rect)
        {
            return new CCFollow(followedNode, rect);
        }

        // node to follow
        protected Node _FollowedNode;

        /// <summary>whether camera should be limited to certain area</summary>
        public bool BoundarySet { get; set; }

        /// <summary>if screen size is bigger than the boundary - update not needed</summary>
        protected bool _boundaryFullyCovered;

        // fast access to the screen dimensions
        protected CCPoint _halfScreenSize;
        protected CCPoint _fullScreenSize;

        // world boundaries
        protected float _leftBoundary;
        protected float _rightBoundary;
        protected float _topBoundary;
        protected float _bottomBoundary;
    }
}

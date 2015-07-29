using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace liwq
{
    public class Node : IDisposable
    {
        public int ID { get; set; }
        public virtual Node Parent { get; set; }
        public virtual bool Visible { get; set; }

        public virtual float PositionX { get; set; }
        public virtual float PositionY { get; set; }
        public virtual Point Position
        {
            get { return new Point(this.PositionX, this.PositionY); }
            set
            {
                this.PositionX = value.X;
                this.PositionY = value.Y;
            }
        }

        public virtual float ScaleX { get; set; }
        public virtual float ScaleY { get; set; }
        public virtual float Scale
        {
            get { return this.ScaleX; }
            set
            {
                this.ScaleX = value;
                this.ScaleY = value;
            }
        }

        public virtual float Rotation { get; set; }

        //public virtual float SkewX { get; set; }
        //public virtual float SkewY { get; set; }

        public virtual Point AnchorPoint { get; set; }
        public virtual Size ContentSize { get; set; }
        public int ZOrder { get; set; }
        public object UserData { get; set; }

        public Node()
        {
            this.Scale = 1.0f;
            this.Visible = true;
        }

        ~Node() { this.Dispose(); }

        public void Dispose()
        {
            this.StopAllActions();
            this.unscheduleAllSelectors();
            if (this.Children != null)
            {
                foreach (Node child in this.Children)
                {
                    child.Parent = null;
                    child.Dispose();
                }
            }
            GC.SuppressFinalize(this);
        }

        #region children

        /// <summary>Array of childrens </summary>
        public List<Node> Children { get; protected set; }

        public Node this[int id]
        {
            get
            {
                if (this.Children != null)
                {
                    foreach (Node child in this.Children)
                    {
                        if (child.ID == id)
                            return child;
                    }
                }
                return null;
            }
        }

        protected void internalAddChild(Node node, int zOrder)
        {
            if (this.Children == null) this.Children = new List<Node>();

            Node last = this.Children.Count > 0 ? this.Children[this.Children.Count - 1] : null;
            if (last == null || last.ZOrder <= zOrder)
            {
                this.Children.Add(node);
            }
            else
            {
                int index = 0;
                foreach (Node child in this.Children)
                {
                    if (child.ZOrder > zOrder)
                    {
                        this.Children.Insert(index, node);
                        break;
                    }
                    ++index;
                }
            }
            node.ZOrder = zOrder;
        }

        protected void internalRemoveChild(Node child, bool dispose)
        {
            child.Parent = null;
            if (dispose == true)
                child.Dispose();
            this.Children.Remove(child);
        }

        public virtual void AddChild(Node child, int zOrder = 0, int id = 0)
        {
            if (child.Parent != null) throw new Exception("child is already added.");
            this.internalAddChild(child, zOrder);
            child.ID = id;
            child.Parent = this;
        }

        public virtual void RemoveChild(Node child, bool dispose)
        {
            this.internalRemoveChild(child, dispose);
        }
        public void RemoveFromParent(bool dispose)
        {
            this.Parent.RemoveChild(this, dispose);
        }

        public virtual void RemoveAllChildren(bool dispose)
        {
            if (this.Children != null)
            {
                foreach (Node child in this.Children)
                    this.RemoveChild(child, dispose);
            }
        }

        public virtual void ReorderChild(Node child, int zOrder)
        {
            this.Children.Remove(child);
            this.internalAddChild(child, zOrder);
        }

        #endregion


        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime)
        {
            if (this.Visible == true && this.Children != null)
            {
                foreach (Node node in this.Children)
                {
                    node.Draw(gameTime);
                }
            }
        }


        #region actions

        /// <summary>
        /// Executes an action, and returns the action that is executed.
        /// The node becomes the action's target.
        /// @warning Starting from v0.8 actions don't retain their target anymore.
        /// @return 
        /// </summary>
        /// <returns>An Action pointer</returns>
        public CCAction RunAction(CCAction action)
        {
            //Debug.Assert(action != null, "Argument must be non-nil");
            //CCActionManager.sharedManager().addAction(action, this, !IsRunning);
            //return action;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes all actions from the running action list
        /// </summary>
        public void StopAllActions()
        {
            //CCActionManager.sharedManager().removeAllActionsFromTarget(this);
        }

        /// <summary>
        /// Removes an action from the running action list
        /// </summary>
        public void StopAction(CCAction action)
        {
            //CCActionManager.sharedManager().removeAction(action);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes an action from the running action list given its tag
        /// @since v0.7.1
        /// </summary>
        public void StopActionByTag(int tag)
        {
            //Debug.Assert(tag != (int)NodeTag.kCCNodeTagInvalid, "Invalid tag");
            //CCActionManager.sharedManager().removeActionByTag(tag, this);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an action from the running action list given its tag
        /// </summary>
        /// <returns>the Action the with the given tag</returns>
        public CCAction GetActionByTag(int tag)
        {
            //Debug.Assert((int)tag != (int)NodeTag.kCCNodeTagInvalid, "Invalid tag");
            //return CCActionManager.sharedManager().getActionByTag((uint)tag, this);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the numbers of actions that are running plus the ones that are schedule to run (actions in actionsToAdd and actions arrays). 
        /// Composable actions are counted as 1 action. Example:
        /// If you are running 1 Sequence of 7 actions, it will return 1.
        /// If you are running 7 Sequences of 2 actions, it will return 7.
        /// </summary>
        public uint NumberOfRunningActions()
        {
            //return CCActionManager.sharedManager().numberOfRunningActionsInTarget(this);
            throw new NotImplementedException();
        }

        #endregion

        #region timers/Schedule

        ///@todo
        //check whether a selector is scheduled
        // bool isScheduled(SEL_SCHEDULE selector);

        /// <summary>
        /// schedules the "update" method. It will use the order number 0. This method will be called every frame.
        /// Scheduled methods with a lower order value will be called before the ones that have a higher order value.
        /// Only one "update" method could be scheduled per node.
        /// @since v0.99.3
        /// </summary>
        public void scheduleUpdate()
        {
            //scheduleUpdateWithPriority(0);
            throw new NotImplementedException();
        }

        [Obsolete("Use scheduleUpdate() instead")]
        public void sheduleUpdate()
        {
            //scheduleUpdateWithPriority(0);
            throw new NotImplementedException();
        }

        /// <summary>
        /// schedules the "update" selector with a custom priority. This selector will be called every frame.
        /// Scheduled selectors with a lower priority will be called before the ones that have a higher value.
        /// Only one "update" selector could be scheduled per node (You can't have 2 'update' selectors).
        /// @since v0.99.3
        /// </summary>
        /// <param name="priority"></param>
        public void scheduleUpdateWithPriority(int priority)
        {
            //CCScheduler.sharedScheduler().scheduleUpdateForTarget(this, priority, !IsRunning);
            throw new NotImplementedException();
        }

        /// <summary>
        ///  unschedules the "update" method.
        /// @since v0.99.3
        /// </summary>
        public void unscheduleUpdate()
        {
            //CCScheduler.sharedScheduler().unscheduleUpdateForTarget(this);
            throw new NotImplementedException();
        }

        /// <summary>
        /// schedules a selector.
        /// The scheduled selector will be ticked every frame
        /// </summary>
        /// <param name="selector"></param>
        public void schedule(System.Action selector)
        {
            //this.schedule(selector, 0);
            throw new NotImplementedException();
        }

        /// <summary>
        /// schedules a custom selector with an interval time in seconds.
        ///If time is 0 it will be ticked every frame.
        ///If time is 0, it is recommended to use 'scheduleUpdate' instead.
        ///If the selector is already scheduled, then the interval parameter 
        ///will be updated without scheduling it again.
        /// </summary>
        public void schedule(System.Action selector, float interval)
        {
            //CCScheduler.sharedScheduler().scheduleSelector(selector, this, interval, !IsRunning);
            throw new NotImplementedException();
        }

        /// <summary>
        /// unschedules a custom selector.
        /// </summary>
        public void unschedule(System.Action selector)
        {
            //// explicit nil handling
            //if (selector != null)
            //{
            //    CCScheduler.sharedScheduler().unscheduleSelector(selector, this);
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// unschedule all scheduled selectors: custom selectors, and the 'update' selector.
        /// Actions are not affected by this method.
        /// @since v0.99.3
        /// </summary>
        public void unscheduleAllSelectors()
        {
            //CCScheduler.sharedScheduler().unscheduleAllSelectorsForTarget(this);
        }

        [Obsolete("use unscheduleAllSelectors()")]
        public void unsheduleAllSelectors()
        {
            //unscheduleAllSelectors();
            throw new NotImplementedException();
        }

        /// <summary>
        /// resumes all scheduled selectors and actions.
        /// Called internally by onEnter
        /// </summary>
        public void resumeSchedulerAndActions()
        {
            //CCScheduler.sharedScheduler().resumeTarget(this);
            //CCActionManager.sharedManager().resumeTarget(this);
            throw new NotImplementedException();
        }

        /// <summary>
        /// pauses all scheduled selectors and actions.
        /// Called internally by onExit
        /// </summary>
        public void pauseSchedulerAndActions()
        {
            //CCScheduler.sharedScheduler().pauseTarget(this);
            //CCActionManager.sharedManager().pauseTarget(this);
            throw new NotImplementedException();
        }

        #endregion

    }
}

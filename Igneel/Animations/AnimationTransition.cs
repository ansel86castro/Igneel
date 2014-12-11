using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Animations
{
    public class AnimationTransition:IDynamic
    {
        AnimationPlayback currentPlayback;
        AnimationPlayback nextPlayback;
        float transitionTime;
        float time;
        public TransitionBlend CurrentBlend = TransitionBlend.Out();
        public TransitionBlend NextBlend = TransitionBlend.In();
        bool transitionComplete;
        bool transitionStarted;

        public AnimationTransition()
        {
            transitionTime = -1;
        }
        public AnimationTransition(float transitionTime = -1)
        {
            this.transitionTime = transitionTime;
        }
        public AnimationTransition(AnimationPlayback current, AnimationPlayback next, float transitionTime)
            :this(transitionTime)
        {
            currentPlayback = current;
            nextPlayback = next;
        }

        public float TransitionTime { get { return transitionTime; } set { transitionTime = value; } }
        public float Time { get { return time; } }
        public AnimationPlayback CurrentPlayback { get { return currentPlayback; } set { currentPlayback = value; } }
        public AnimationPlayback NextPlayback { get { return nextPlayback; } set { nextPlayback = value; } }

        public bool CanPerformTransition
        {
            get
            {

                return time <= transitionTime;
            }
        }

        public bool TransitionStarted { get { return transitionStarted; } }
        public bool TransitionComplete { get { return !transitionStarted && transitionComplete; } }

        public void Update(float deltaT)
        {
            if (time <= transitionTime)
            {
                transitionStarted = true;
                transitionComplete = false;

                float s = time / transitionTime;

                CurrentBlend.UpdateBlendings(s);
                currentPlayback.Update(deltaT, CurrentBlend.Blend, CurrentBlend.Velocity);

                NextBlend.UpdateBlendings(s);
                nextPlayback.Update(deltaT, NextBlend.Blend, NextBlend.Velocity);

                time += deltaT;                           
            }
            else
            {
                time = 0;
                transitionStarted = false;
                transitionComplete = true;                
            }
        }

        public void Reset()
        {
            ResetTime();
            currentPlayback.Reset();
            nextPlayback.Reset();
        }

        public void ResetTime()
        {
            time = 0;
            transitionStarted = false;
            transitionComplete = false;
        }
    }

    public struct TransitionBlend
    {
        public float Blend;
        public float Velocity;
        public float StartBlend;
        public float EndBlend;
        public float StartVelocity;
        public float EndVelocity;

        public TransitionBlend(float startBlend, float startVelocity)
        {
            Blend = 1.0f;
            StartBlend = startBlend;
            Velocity = 1.0f;
            StartVelocity = startVelocity;
            EndBlend = 1.0f - StartBlend;
            EndVelocity = 1.0f - StartVelocity;
        }

        public TransitionBlend(float startBlend, float endBlend, float startVelocity, float endVelocity)
        {
            this.StartBlend = startBlend;
            this.EndBlend = endBlend;
            this.StartVelocity = startVelocity;
            this.EndVelocity = endVelocity;
            Blend = 0;
            Velocity = 0;
        }

        public static TransitionBlend In()
        {
            return new TransitionBlend(0, 0);
        }

        public static TransitionBlend Out()
        {
            return new TransitionBlend(1, 1);
        }

        public void UpdateBlendings(float s)
        {
            Blend = Numerics.Lerp(StartBlend, 1 - StartBlend, s);
            Velocity = Numerics.Lerp(StartVelocity, 1 - StartVelocity, s);
        }
    }
}

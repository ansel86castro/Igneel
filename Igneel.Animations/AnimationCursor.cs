using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Animations
{    
    public enum AnimationLooping
    {
        None,
        Secuential,
        PinPon
    };

    partial class Animation
    {
        [Serializable]
        public class Cursor
        {
            private float time;
            private float playVelocity = 1;
            private float playingDir = 1;
            private bool animationEndFlag;
            private AnimationLooping looping;
            private float startTime = -1;
            private float endTime = -1;
            private bool timeRestart;
            public event Action<Animation, Cursor> AnimationEnd;

            [NonSerialized]
            Animation animation;

            public Cursor(Animation animation)
            {
                if (animation == null) throw new ArgumentNullException("animation");
                this.animation = animation;
                animation.cursors.Add(this);
            }

            public static Cursor Create(Animation animation)
            {
                return new Cursor(animation);
            }

            public bool TimeRestart
            {
                get { return timeRestart; }
            }

            public Animation Animation { get { return animation; } internal set { animation = value; } }

            public float Time { get { return time; } set { time = value; } }

            public float PlayVelocity { get { return playVelocity; } set { playVelocity = value; } }

            public float PlayDirection { get { return playingDir; } set { playingDir = value; } }

            public AnimationLooping Looping
            {
                get { return looping; }
                set
                {
                    looping = value;
                }
            }

            public float StartTime { get { return startTime; } set { startTime = value; } }

            public float EndTime { get { return endTime; } set { endTime = value; } }

            public bool Stoped { get { return animationEndFlag; } }

            public void OnAnimationEnd()
            {
                animationEndFlag = true;
                if (AnimationEnd != null)
                    AnimationEnd(animation, this);
            }

            public virtual void Reset()
            {
                time = startTime < 0 ? 0 : startTime;
                if (looping == AnimationLooping.PinPon)
                {
                    playingDir = Math.Abs(playingDir);
                }

                animationEndFlag = false;
            }

            public virtual void UpdateTime(float elapsed)
            {
                time += playVelocity * playingDir * elapsed;
            }

            public void ValidateTime()
            {
                var lastKeyValue = animation.LastKeyValue;
                timeRestart = false;

                if (endTime < 0) endTime = lastKeyValue;
                if (startTime < 0) startTime = 0;

                if (time == 0 || startTime == endTime)
                    time = startTime;
                else if (time > endTime)
                {
                    switch (looping)
                    {
                        case AnimationLooping.Secuential:
                            time = startTime + (time - endTime);
                            timeRestart = true;
                            break;
                        case AnimationLooping.PinPon:
                            time = Math.Max(0, endTime - (time - endTime));
                            playingDir = -playingDir;
                            break;
                        case AnimationLooping.None:
                            time = endTime;
                            OnAnimationEnd();                          
                            break;
                    }
                }
                else if (time < startTime)
                {
                    switch (looping)
                    {
                        case AnimationLooping.Secuential:
                            time = endTime - (startTime - time);
                            timeRestart = true;
                            break;
                        case AnimationLooping.PinPon:
                            time = startTime + (startTime - time);
                            playingDir = -playingDir;
                            break;
                        case AnimationLooping.None:
                            time = startTime;
                            OnAnimationEnd();                            
                            break;
                    }
                }
            }          
           
        }
    }
}

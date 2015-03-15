using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Collections;


namespace Igneel.Animations
{
    public struct PlaybackDesc
    {
        public Animation Animation;
        public Animation.Cursor Cursor;
        public float Blend;
        public float Velocity;

        public PlaybackDesc(Animation animation, Animation.Cursor cursor = null, float blend = 1.0f, float velocity = 1.0f)
        {
            this.Animation = animation;
            this.Cursor = cursor ?? animation.GetCursor(0);
            this.Blend = blend;
            this.Velocity = velocity;
        }
    }

    public class AnimationPlayback:IEnumerable<PlaybackDesc>,IDynamic
    {
        LinkedList<PlaybackDesc> animations = new LinkedList<PlaybackDesc>();
        
        public AnimationPlayback()
        {

        }

        public AnimationPlayback(Animation animation, Animation.Cursor cursor = null, float blend = 1.0f, float velocity = 1.0f)
        {
            AddAnimation(animation, cursor, blend, velocity);
        }

        public AnimationPlayback(Animation animation, float startTime, float duration, AnimationLooping loop, float blend = 1.0f, float velocity = 1.0f)
        {
            AddAnimation(animation, startTime, duration, loop, blend, velocity);
        }

        public int NbAnimations { get { return animations.Count; } }

        public PlaybackDesc FirstPlayback { get { return animations.First.Value; } }
       
        public AnimationPlayback AddAnimation(Animation animation, Animation.Cursor cursor = null, float blend = 1.0f, float velocity = 1.0f)
        {
            var desc = new PlaybackDesc(animation, cursor, blend, velocity);
            animations.AddLast(desc);
            return this;
        }

        public AnimationPlayback AddAnimation(Animation animation, float startTime, float duration, AnimationLooping loop, float blend = 1.0f, float velocity = 1.0f)
        {
            Animation.Cursor cursor = new Animation.Cursor(animation)
            {
                StartTime = startTime,
                EndTime = startTime + duration,
                Looping = loop
            };
            PlaybackDesc b = new PlaybackDesc(animation, cursor, blend, velocity);
            animations.AddLast(b);
            return this;
        }

        public bool RemoveAnimation(Animation animation)
        {
            var node = animations.GetLinkedNode(x => x.Animation == animation);
            if (node != null)
            {
                animations.Remove(node);
                return true;
            }
            return false;
        }

        public bool SetPlayback(PlaybackDesc desc)
        {
            var node = animations.GetLinkedNode(x => x.Animation == desc.Animation);
            if (node != null)
            {
                node.Value = desc;
                return true;
            }
            return false;
        }

        public bool GetPlayback(Animation animation, out PlaybackDesc desc)
        {
            var node = animations.GetLinkedNode(x => x.Animation == animation);
            if (node != null)
            {
               desc = node.Value;
               return true;
            }
            desc = new PlaybackDesc();
            return false;
        }

        public Animation.Cursor GetCursor(Animation animation)
        {
            PlaybackDesc desc;
            GetPlayback(animation, out desc);
            return desc.Cursor;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(animations.GetEnumerator());
        }

        IEnumerator<PlaybackDesc> IEnumerable<PlaybackDesc>.GetEnumerator()
        {
            return new Enumerator(animations.GetEnumerator());
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(animations.GetEnumerator());
        }

        public void Update(float deltaT, float blend, float velocity)
        {
            foreach (var item in animations)
            {
                var cursor = item.Cursor;
                var animation = item.Animation;

                cursor.PlayVelocity = item.Velocity * velocity;
                float blending = item.Blend * blend;
                animation.Update(deltaT, cursor, blending, blending >= 0);
            }
        }

        public void Update(float deltaT)
        {
            foreach (var item in animations)
            {
                var cursor = item.Cursor;
                var animation = item.Animation;

                cursor.PlayVelocity = item.Velocity;
                float blending = item.Blend;
                animation.Update(deltaT, cursor, blending, blending >= 0);
            }
        }

        public void Reset()
        {
            foreach (var item in animations)
            {
                item.Cursor.Reset();
            }
        }

        public struct Enumerator : IEnumerator<PlaybackDesc>
        {
            LinkedList<PlaybackDesc>.Enumerator listEnumarator;

            public Enumerator(LinkedList<PlaybackDesc>.Enumerator enumerator)
            {
                listEnumarator = enumerator;
            }

            public PlaybackDesc Current
            {
                get { return listEnumarator.Current; }
            }

            public void Dispose()
            {
                listEnumarator.Dispose();
            }

            object System.Collections.IEnumerator.Current
            {
                get { return listEnumarator.Current; }
            }

            public bool MoveNext()
            {
                return listEnumarator.MoveNext();
            }

            void System.Collections.IEnumerator.Reset()
            {
                ((IEnumerator<PlaybackDesc>)listEnumarator).Reset();
            }
        }
    }
}

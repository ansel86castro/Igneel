using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Animations
{
    public class SecuenceNode : INameable
    {
        LinkedList<SecuenceTransition> transitions = new LinkedList<SecuenceTransition>();
        List<PlaybackDesc> animations = new  List<PlaybackDesc>();
        SecuenceTransition currentTransition;
        string name;
        int id;

        public event Action<SecuenceNode, float> UpdateBegin;
        public event Action<SecuenceNode, float> UpdateEnd;
        public event Action<SecuenceNode> Enter;
        public event Action<SecuenceNode> Leave;

        public SecuenceNode(string name, int id = 0, IEnumerable<PlaybackDesc> animations = null)
        {
            this.name = name;
            this.id = id;
            if (animations != null)
            {
                foreach (var item in animations)
                {
                    this.animations.Add(item);
                }
            }
        }

        public SecuenceNode(string name, int id = 0, params PlaybackDesc[] animations)
            : this(name, id, (IEnumerable<PlaybackDesc>)animations)
        {

        }

        public SecuenceNode(string name, Animation animation, float startTime, float duration, AnimationLooping loop, float blend = 1.0f)
            :this(name)
        {
            Playing(animation, startTime, duration, loop, blend);
        }

        public List<PlaybackDesc> Animations { get { return animations; } }

        public string Name { get { return name; } set { name = value; } }

        public int Id { get { return id; } set { id = value; } }

        public SecuenceTransition CurrentTransition { get { return currentTransition; } }

        public void AddTransition(SecuenceTransition transition)
        {
            transition.SourceNode = this;
            transitions.AddLast(transition);
        }

        public bool RemoveTransition(SecuenceTransition transition)
        {
            return transitions.Remove(transition);
        }

        public bool RemoveTransitions(SecuenceNode node)
        {
            List<SecuenceTransition> tempTransitions = new List<SecuenceTransition>();

            foreach (var item in transitions)
            {
                if (item.DestNode == node)
                    tempTransitions.Add(item);
            }

            foreach (var item in tempTransitions)
            {
                transitions.Remove(item);
            }

            return tempTransitions.Count > 0;
        }       

        public SecuenceNode TransitionTo(SecuenceTransition transition)
        {
            transitions.AddLast(transition);
            return this;
        }

        public SecuenceNode BeforeUpdate(Action<SecuenceNode, float> action)
        {
            this.UpdateBegin = action;
            return this;
        }

        public SecuenceNode AfterUpdate(Action<SecuenceNode, float> action)
        {
            this.UpdateEnd = action;
            return this;
        }

        public SecuenceNode Activating(Action<SecuenceNode> action)
        {
            this.Enter = action;
            return this;
        }

        public SecuenceNode Deactivating(Action<SecuenceNode> action)
        {
            this.Leave = action;
            return this;
        }

        public void OnBeforeUpdate(float deltaT)
        {
            if (UpdateBegin != null)
                UpdateBegin(this, deltaT);
        }

        public void OnAfterUpdate(float deltaT)
        {
            if (UpdateEnd != null)
                UpdateEnd(this, deltaT);
        }

        public void OnActivating()
        {
            if (Enter != null)
                Enter(this);
        }

        public void OnDeactivating()
        {
            foreach (var item in animations)
            {
                item.Cursor.Reset();
            }

            if (Leave != null)
                Leave(this);           
        }

        public void UpdateAnimations(float deltaT, float blend, float velocity)
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

        public SecuenceNode Update(float deltaT)
        {           
            if (currentTransition == null)
            {
                foreach (var tr in transitions)
                {
                    if (tr.IsTriggered())
                        currentTransition = tr;
                }
            }

            SecuenceNode next = null;            
            if (currentTransition != null)
            {
                //it's in transition
                if (!currentTransition.Blend(deltaT))
                {                            
                    //the transition has ended
                    //move to the next state
                    next = currentTransition.DestNode;
                    currentTransition = null; 
                }                
            }
            else
            {
                OnBeforeUpdate(deltaT);

                //its playing the state and there aren't any transitions triggered
                UpdateAnimations(deltaT, animations.Count == 1 ? -1 : 1, 1);

                OnAfterUpdate(deltaT);                
            }

            return next;
        }

        public SecuenceNode Playing(Animation animation, Animation.Cursor cursor = null, float blend = 1.0f)
        {
            PlaybackDesc b = new PlaybackDesc(animation, cursor, blend);
            animations.Add(b);
            return this;
        }

        public SecuenceNode Playing(Animation animation, float startTime, float duration, AnimationLooping loop, float blend = 1.0f)
        {
            Animation.Cursor cursor = new Animation.Cursor(animation)
            {
                StartTime = startTime,
                EndTime = startTime + duration,
                Looping = loop
            };
            PlaybackDesc b = new PlaybackDesc(animation, cursor, blend);
            animations.Add(b);
            return this;
        }

        public void Reset()
        {
            foreach (var item in animations)
            {
                item.Cursor.Reset();
            }
        }

        public float PlayDirection
        {
            get { return animations[0].Cursor.PlayDirection; }
            set
            {
                foreach (var item in animations)
                {
                    item.Cursor.PlayDirection = value;
                }
            }
        }

        public float PlayVelocity
        {
            get { return animations[0].Cursor.PlayVelocity; }
            set
            {
                foreach (var item in animations)
                {
                    item.Cursor.PlayVelocity = value;
                }
            }
        }

        public AnimationLooping Looping
        {
            get { return animations[0].Cursor.Looping; }
            set
            {
                foreach (var item in animations)
                {
                    item.Cursor.Looping = value;
                }
            }
        }

        public override string ToString()
        {
            return name ?? base.ToString();
        }
    }
  
}

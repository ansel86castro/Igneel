using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Animations
{
    public class SecuenceTransition:INameable
    {            
        SecuenceNode sourceNode;
        SecuenceNode destNode;
        float transitionTime;
        float time;
        Func<SecuenceTransition, bool> trigger;
        string name;
        public TransitionBlend SourceBlend = TransitionBlend.Out();
        public TransitionBlend DestBlend = TransitionBlend.In();

        public event Action<SecuenceTransition, float> Blended;
        public event Action<SecuenceTransition> Leaved;

        public SecuenceTransition(float duration = -1)
        {
            this.transitionTime = duration;
        }        

        public SecuenceTransition(SecuenceNode source, SecuenceNode dest, float duration, Func<SecuenceTransition, bool> trigger)
        {
            this.sourceNode = source;
            this.destNode = dest;
            this.transitionTime = duration;
            this.trigger = trigger;
        }

        public SecuenceTransition(SecuenceNode dest, float duration, Func<SecuenceTransition, bool> trigger = null)
            : this(null, dest, duration, trigger)
        {

        }

        public string Name { get { return name; } set { name = value; } }

        public Func<SecuenceTransition, bool> Trigger { get { return trigger; } set { trigger = value; } }

        public SecuenceNode SourceNode { get { return sourceNode; } set { sourceNode = value; } }

        public SecuenceNode DestNode { get { return destNode; } set { destNode = value; } }

        public float Duration { get { return transitionTime; } set { transitionTime = value; } }

        public bool IsBlending
        {
            get
            {

                return time <= transitionTime;
            }
        }

        public bool IsTriggered()
        {
            return trigger(this);
        }       

        public void OnBlended(float deltaT)
        {
            if (Blended != null)
                Blended(this, deltaT);
        }

        public void OnLeaved()
        {
            if (Leaved != null)
                Leaved(this);
        }

        public bool Blend(float deltaT)
        {            
            if (time <= transitionTime)
            {
                float s = time / transitionTime;

                SourceBlend.UpdateBlendings(s);
                sourceNode.UpdateAnimations(deltaT, SourceBlend.Blend, SourceBlend.Velocity);

                DestBlend.UpdateBlendings(s);
                destNode.UpdateAnimations(deltaT, DestBlend.Blend, DestBlend.Velocity);

                time += deltaT;

                OnBlended(deltaT);

                return true;
            }
            else
            {
                time = 0;             
                return false;
            }
        }

        public SecuenceTransition BlendSourceWith(TransitionBlend blending)
        {
            SourceBlend = blending;
            return this;
        }

        public SecuenceTransition BlendDestinationWith(TransitionBlend blending)
        {
            DestBlend = blending;
            return this;
        }

        public SecuenceTransition WithBlendings(TransitionBlend source, TransitionBlend destination)
        {
            SourceBlend = source;
            DestBlend = destination;
            return this;
        }

        public SecuenceTransition FiredWhen(Func<SecuenceTransition, bool> trigger)
        {
            this.trigger = trigger;
            return this;
        }

        public SecuenceTransition DuringTime(float time)
        {
            this.transitionTime = time;
            return this;
        }

        public SecuenceTransition WhenBlending(Action<SecuenceTransition, float> action)
        {
            this.Blended = action;
            return this;
        }

        public override string ToString()
        {
            return name ?? base.ToString();
        }
       
    }

  
    
}

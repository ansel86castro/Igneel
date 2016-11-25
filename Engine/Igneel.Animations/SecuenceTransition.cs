using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Animations
{
    public class SecuenceTransition:INameable
    {            
        SecuenceNode _sourceNode;
        SecuenceNode _destNode;
        float _transitionTime;
        float _time;
        Func<SecuenceTransition, bool> _trigger;
        string _name;
        public TransitionBlend SourceBlend = TransitionBlend.Out();
        public TransitionBlend DestBlend = TransitionBlend.In();

        public event Action<SecuenceTransition, float> Blended;
        public event Action<SecuenceTransition> Leaved;

        public SecuenceTransition(float duration = -1)
        {
            this._transitionTime = duration;
        }        

        public SecuenceTransition(SecuenceNode source, SecuenceNode dest, float duration, Func<SecuenceTransition, bool> trigger)
        {
            this._sourceNode = source;
            this._destNode = dest;
            this._transitionTime = duration;
            this._trigger = trigger;
        }

        public SecuenceTransition(SecuenceNode dest, float duration, Func<SecuenceTransition, bool> trigger = null)
            : this(null, dest, duration, trigger)
        {

        }

        public string Name { get { return _name; } set { _name = value; } }

        public Func<SecuenceTransition, bool> Trigger { get { return _trigger; } set { _trigger = value; } }

        public SecuenceNode SourceNode { get { return _sourceNode; } set { _sourceNode = value; } }

        public SecuenceNode DestNode { get { return _destNode; } set { _destNode = value; } }

        public float Duration { get { return _transitionTime; } set { _transitionTime = value; } }

        public bool IsBlending
        {
            get
            {

                return _time <= _transitionTime;
            }
        }

        public bool IsTriggered()
        {
            return _trigger(this);
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
            if (_time <= _transitionTime)
            {
                float s = _time / _transitionTime;

                SourceBlend.UpdateBlendings(s);
                _sourceNode.UpdateAnimations(deltaT, SourceBlend.Blend, SourceBlend.Velocity);

                DestBlend.UpdateBlendings(s);
                _destNode.UpdateAnimations(deltaT, DestBlend.Blend, DestBlend.Velocity);

                _time += deltaT;

                OnBlended(deltaT);

                return true;
            }
            else
            {
                _time = 0;             
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
            this._trigger = trigger;
            return this;
        }

        public SecuenceTransition DuringTime(float time)
        {
            this._transitionTime = time;
            return this;
        }

        public SecuenceTransition WhenBlending(Action<SecuenceTransition, float> action)
        {
            this.Blended = action;
            return this;
        }

        public override string ToString()
        {
            return _name ?? base.ToString();
        }
       
    }

  
    
}

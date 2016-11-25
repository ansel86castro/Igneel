using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Collections;

namespace Igneel.IA
{
    public class Automata:IDynamic,IDynamicNotificable
    {
        ObservedDictionary<string, State> _states;
        State _current;
        public event UpdateEventHandler UpdateEvent;

        public Automata()
        {
            _states = new ObservedDictionary<string, State>(null,
                x => 
                {
                    foreach (var item in _states)
                    {
                        if (item != x)
                        {
                            item.RemoveTransition(x);
                        }
                    }
                },
                x => x.Name
                );
        }

        public State Current { get { return _current; } }

        public ObservedDictionary<string, State> States { get { return _states; } }

        public Automata AddState(string name, Action<float> update)
        {
            _states.Add(new DelegateState(name, update));
            return this;
        }

        public Automata AddTransition(string targetState, string nextState, Predicate<State> condition)
        {
            var target = _states[targetState];
            var next = _states[nextState];
            target.AddTransition(next, condition);
            return this;
        }

        public Automata RemoveTransition(string targetState, string nextState)
        {
            var target = _states[targetState];
            var next = _states[nextState];
            target.RemoveTransition(next);
            return this;
        }

        public virtual void Update(float elapsedTime)
        {
            if (_current == null && _states.Count > 0)
                _current = _states[0];

            if (_current != null)
                _current = _current.Update(elapsedTime);

            OnUpdate(elapsedTime);
        }

        private void OnUpdate(float elapsetTime)
        {
            if (UpdateEvent != null)
                UpdateEvent(this, elapsetTime);
        }
    }
}

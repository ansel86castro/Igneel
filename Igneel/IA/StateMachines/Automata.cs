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
        ObservedDictionary<string, State> states;
        State current;
        public event UpdateEventHandler UpdateEvent;

        public Automata()
        {
            states = new ObservedDictionary<string, State>(null,
                x => 
                {
                    foreach (var item in states)
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

        public State Current { get { return current; } }

        public ObservedDictionary<string, State> States { get { return states; } }

        public Automata AddState(string name, Action<float> update)
        {
            states.Add(new DelegateState(name, update));
            return this;
        }

        public Automata AddTransition(string targetState, string nextState, Predicate<State> condition)
        {
            var target = states[targetState];
            var next = states[nextState];
            target.AddTransition(next, condition);
            return this;
        }

        public Automata RemoveTransition(string targetState, string nextState)
        {
            var target = states[targetState];
            var next = states[nextState];
            target.RemoveTransition(next);
            return this;
        }

        public virtual void Update(float elapsedTime)
        {
            if (current == null && states.Count > 0)
                current = states[0];

            if (current != null)
                current = current.Update(elapsedTime);

            OnUpdate(elapsedTime);
        }

        private void OnUpdate(float elapsetTime)
        {
            if (UpdateEvent != null)
                UpdateEvent(this, elapsetTime);
        }
    }
}

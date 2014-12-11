using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Collections;

namespace Igneel.IA
{
    public abstract class State:IEnumerable<StateTransition>
    {
        private string name;      
        private LinkedList<StateTransition> transitions = new LinkedList<StateTransition>();

        public State(string name)
        {
            this.name = name;         
        }

        public string Name { get { return name; } }

        public event EventHandler Activate;

        public event EventHandler Deactivate;       

        public abstract void DoAction(float elapsedTime);

        private void OnDeactivate()
        {
            if (Deactivate != null)
                Deactivate(this, EventArgs.Empty);
        }

        private void OnActivate()
        {
            if (Activate != null)
                Activate(this, EventArgs.Empty);
        }

        public State DoTransitions()
        {
            State newState = this;
            foreach (var t in transitions)
            {
                if (t.Evaluate())
                {
                    newState = t.Target;
                    OnDeactivate();
                    newState.OnActivate();
                    break;
                }
            }
            return newState;
        }

        public State Update(float elapsedTime)
        {
            DoAction(elapsedTime);
            return DoTransitions();
        }

        public void AddTransition(State target, Predicate<State> condition)
        {
            transitions.AddLast(new StateTransition(target , condition));
        }

        public bool RemoveTransition(State target)
        {
            var node = transitions.GetLinkedNode(x => x.Target == target);
            if (node != null)
            {
                transitions.Remove(node);
                return true;
            }
            return false;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(transitions.GetEnumerator());
        }

        IEnumerator<StateTransition> IEnumerable<StateTransition>.GetEnumerator()
        {
            return transitions.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return transitions.GetEnumerator();
        }

        public struct Enumerator : IEnumerator<StateTransition>
        {
            LinkedList<StateTransition>.Enumerator listEnumarator;

            public Enumerator(LinkedList<StateTransition>.Enumerator enumerator)
            {
                listEnumarator = enumerator;
            }

            public StateTransition Current
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
                ((IEnumerator<StateTransition>)listEnumarator).Reset();
            }
        }

        public override string ToString()
        {
            return name??base.ToString();
        }
    }

    public struct StateTransition
    {
        public State Target;
        public Predicate<State> Condition;

        public StateTransition(State target, Predicate<State> condition)
        {
            this.Target = target;
            this.Condition = condition;
        }

        public bool Evaluate()
        {
            if (Condition != null)
                return Condition(Target);
            return false;
        }

        public override string ToString()
        {
            return Target.Name;
        }
    }

    public class DelegateState : State
    {
        private Action<float> update;
        public DelegateState(string name, Action<float> updateCallback)
            :base(name)
        {
            this.update = updateCallback;
        }

        public override void DoAction(float elapsedTime)
        {
            update(elapsedTime);
        }
    }


}

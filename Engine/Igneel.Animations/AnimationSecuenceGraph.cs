using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Collections;

namespace Igneel.Animations
{
    public class SecuenceStateMachine:IDynamic
    {
        ObservedDictionary<string, SecuenceNode> _nodes = new ObservedDictionary<string, SecuenceNode>(null, null, x => x.Name);
        SecuenceNode _currentNode;
      
        public SecuenceStateMachine()
        {

        }

        public SecuenceStateMachine(IEnumerable<SecuenceNode> nodes)
        {
            if (nodes != null)
                this._nodes.AddRange(nodes);
        }

        public ObservedDictionary<string, SecuenceNode> Nodes { get { return _nodes; } }           

        public SecuenceStateMachine WithState(string name, Action<SecuenceNode> action)
        {
            SecuenceNode node = new SecuenceNode(name);
            WithState(node);
            action(node);
            return this;
        }

        public SecuenceStateMachine WithState(SecuenceNode node)
        {          
            this._nodes.Add(node);
            node.Id = _nodes.Count - 1;
            return this;
        }
        public SecuenceStateMachine WithTransition(string sourceNode, string destNode, SecuenceTransition transition)
        {
            transition.SourceNode = _nodes[sourceNode];
            transition.DestNode = _nodes[destNode];

            transition.Name = sourceNode + "->" + destNode;

            transition.SourceNode.AddTransition(transition);

            return this;
        }

        public void Update(float elapsedTime)
        {
            if (_nodes.Count == 0) return;

            if (_currentNode == null)
                _currentNode = _nodes[0];

            SecuenceNode next = null;
            while ((next = _currentNode.Update(elapsedTime)) != null)
            {
                _currentNode.OnDeactivating();
                next.OnActivating();

                _currentNode = next;
            }
        }
    }   
}

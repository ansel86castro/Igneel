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
        ObservedDictionary<string, SecuenceNode> nodes = new ObservedDictionary<string, SecuenceNode>(null, null, x => x.Name);
        SecuenceNode currentNode;
      
        public SecuenceStateMachine()
        {

        }

        public SecuenceStateMachine(IEnumerable<SecuenceNode> nodes)
        {
            if (nodes != null)
                this.nodes.AddRange(nodes);
        }

        public ObservedDictionary<string, SecuenceNode> Nodes { get { return nodes; } }           

        public SecuenceStateMachine WithState(string name, Action<SecuenceNode> action)
        {
            SecuenceNode node = new SecuenceNode(name);
            WithState(node);
            action(node);
            return this;
        }

        public SecuenceStateMachine WithState(SecuenceNode node)
        {          
            this.nodes.Add(node);
            node.Id = nodes.Count - 1;
            return this;
        }
        public SecuenceStateMachine WithTransition(string sourceNode, string destNode, SecuenceTransition transition)
        {
            transition.SourceNode = nodes[sourceNode];
            transition.DestNode = nodes[destNode];

            transition.Name = sourceNode + "->" + destNode;

            transition.SourceNode.AddTransition(transition);

            return this;
        }

        public void Update(float elapsedTime)
        {
            if (nodes.Count == 0) return;

            if (currentNode == null)
                currentNode = nodes[0];

            SecuenceNode next = null;
            while ((next = currentNode.Update(elapsedTime)) != null)
            {
                currentNode.OnDeactivating();
                next.OnActivating();

                currentNode = next;
            }
        }
    }   
}

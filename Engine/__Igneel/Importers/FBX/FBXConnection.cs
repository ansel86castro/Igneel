using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Importers.FBX
{
    public enum ConnectionType { OO, OP }

    public class FBXBindingManager
    {       
        FBXObject objects;
        FBXDocument doc;
        List<FBXDeclarationNode> connections;
        SortedList<string, FBXNode> nodes = new SortedList<string, FBXNode>();     
        FBXObject connectionObject;

        FBXNode sceneGraph;

        public FBXNode SceneGraph { get { return sceneGraph; } }

        public FBXBindingManager(FBXDocument doc)
        {
            this.doc = doc;
            this.objects = doc.GetDeclaration<FBXObject>(FBXDocument.Objects);
            this.connectionObject = doc.GetObject("Connections");
            this.connections = connectionObject.Declarations["Connect"];                              

            LoadConnectionHeirarchys();
        }
    
        public IList<FBXNode> Nodes { get { return nodes.Values; } }
     
        public FBXObject ConnectionObject { get { return connectionObject; } }

        public FBXNode GetNodeById(string id)
        {
            FBXNode n;
            nodes.TryGetValue(id, out n);
            return n;
        }        

        private void LoadConnectionHeirarchys()
        {
            sceneGraph = CreateNode("Model::Scene");                      
        }

        private FBXNode CreateNode(string id)
        {
            if (nodes.ContainsKey(id))            
                return nodes[id];
            
            FBXNode node = new FBXNode(id);
            node.Target = objects.GetObjectById(id);            
            nodes.Add(id, node);

            foreach (var entry in GetConnectEntries(id))
            {
                FBXNode child;
                if (!nodes.TryGetValue(entry.ObjectID, out child))                
                    child = CreateNode(entry.ObjectID);

                var binding = new FBXBinding { ConType = entry.ConType, Value = child, Property = entry.Property };

                child.ConnectedTo.Add(new FBXBinding{ ConType = binding.ConType, Value = node, Property = binding.Property });
                node.Connections.Add(binding);
            }

            return node;
        }

        private IEnumerable<ConnectionEntry> GetConnectEntries(string id)
        {
            foreach (FBXListProperty connect in connections)
            {
                if (connect.Values[2] == id)
                {
                    yield return new ConnectionEntry
                    {
                        ConType = connect.Values[0] == "OO" ? ConnectionType.OO : ConnectionType.OP,
                        ObjectID = connect.Values[1],
                        TargetID = connect.Values[2],
                        Property = connect.Values[0] == "OP" ? connect.Values[3] : null
                    };
                }
            }
        }
       
    }


    public struct ConnectionEntry
    {
        public ConnectionType ConType { get; set; }

        public string TargetID { get; set; }

        public string ObjectID { get; set; }

        public string Property { get; set; }
    }

    public class FBXNode
    {
        string targetId;
        FBXObject target;

        List<FBXBinding> connections;

        List<FBXBinding> connectedTo;  
      
        public FBXNode(string targetFullName)
        {
            this.targetId = targetFullName;
            this.connections = new List<FBXBinding>();
            this.connectedTo = new List<FBXBinding>();
        }

        public bool IsRoot { get { return connectedTo.Count == 0; } }      

        public bool HaveConnections { get { return connections.Count == 0; } }

        public string TargetId { get { return targetId; } set { targetId = value; } }

        public FBXObject Target { get { return target; } set { target = value; } }

        public List<FBXBinding> ConnectedTo { get { return connectedTo; } }        

        public List<FBXBinding> Connections { get { return connections; } }

        public bool IsDirectChildOf(FBXNode node)
        {
            for (int i = 0; i < connectedTo.Count; i++)
            {
                if (connectedTo[i].Value == node)
                    return true;
            }
            return false;
        }

        public FBXNode FindFirstConnection(Func<FBXNode, bool> matchPredicated)
        {
            if (matchPredicated(this)) return this;

            for (int i = 0; i < connections.Count; i++)
            {
                FBXNode node = connections[i].Value.FindFirstConnection(matchPredicated);
                if (node != null)
                    return node;
            }

            return null;
        }

        public IEnumerable<FBXNode> FindConnections(Func<FBXNode, bool> matchPredicated)
        {
            if (matchPredicated(this))
                yield return this;

            for (int i = 0; i < connections.Count; i++)
            {
                foreach (var n in connections[i].Value.FindConnections(matchPredicated))
                {
                    yield return n;
                }
            }
        }

        public FBXBinding FindBinding(Func<FBXBinding, bool> matchPredicated)
        {           
            for (int i = 0; i < connections.Count; i++)
            {
                if (matchPredicated(connections[i]))
                    return connections[i];
                else
                {
                    var binding = connections[i].Value.FindBinding(matchPredicated);
                    if (binding != null)
                        return binding;
                }
            }

            return null;
        }

        public FBXNode FindFirstConnectedTo(Func<FBXNode, bool> matchPredicated)
        {
            if (matchPredicated(this)) return this;

            for (int i = 0; i < connectedTo.Count; i++)
            {
                FBXNode node = connectedTo[i].Value.FindFirstConnectedTo(matchPredicated);
                if (node != null)
                    return node;
            }

            return null;
        }     

        public IEnumerable<FBXNode> FindConnectedsTo(Func<FBXNode, bool> matchPredicated)
        {
            if (matchPredicated(this))
                yield return this;

            for (int i = 0; i < connections.Count; i++)
            {
                foreach (var n in connections[i].Value.FindConnectedsTo(matchPredicated))
                {
                    yield return n;
                }
            }
        }

        public IEnumerable<FBXBinding> FindBindings(Func<FBXBinding, bool> matchPredicated)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                if (matchPredicated(connections[i]))
                    yield return connections[i];
                foreach (var item in connections[i].Value.FindBindings(matchPredicated))
                {
                    yield return item;
                }
            }            
        }

        public override string ToString()
        {
            return targetId;
        }
        
    }

    public class FBXBinding
    {
        public ConnectionType ConType { get; set; }     

        public string Property { get; set; }

        public FBXNode Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

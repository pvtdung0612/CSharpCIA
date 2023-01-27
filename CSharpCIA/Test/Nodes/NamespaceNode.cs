using CSharpCIA.CSharpCIA.Nodes.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.Test.Nodes
{
    public class NamespaceNode : Node
    {
        public NamespaceNode(uint id, string name, string sourcePath, List<Connection> connections) : base(id, name, sourcePath, connections)
        {
        }

        public override Type Type => typeof(NamespaceNode);

        public List<Node> childrens;
    }
}

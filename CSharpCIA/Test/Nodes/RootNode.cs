using CSharpCIA.CSharpCIA.Nodes.Builders;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.Test.Nodes
{
    public class RootNode : Node
    {
        public RootNode(uint id, string name, string sourcePath, string body, List<Node> childrens, List<Connection> connections) : base(id, name, sourcePath, connections)
        {
            this.body = body;
            this.childrens = childrens;
        }

        public override Type Type => typeof(RootNode);

        public List<Node> childrens;
        public string body;
    }
}

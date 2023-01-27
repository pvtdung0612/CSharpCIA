using CSharpCIA.CSharpCIA.Nodes.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.Test.Nodes
{
    public class MethodNode : Node
    {
        private List<MODIFIERS> modifiers;
        private string body;
        private string uniqueName;

        public string Body { get => body; set => body = value; }

        public override Type Type => typeof(MethodNode);

        public string UniqueName { get => uniqueName; set => uniqueName = value; }

        public MethodNode(uint id, string name, string sourcePath, List<Connection> connections, string uniqueName, string body) : base(id, name, sourcePath, connections)
        {
            UniqueName = uniqueName;
            Body = body;
        }
    }
}

using CSharpCIA.src.node.builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.src.node
{
    public class MethodNode : Node
    {
        private List<MODIFIERS> modifiers;
        private string body;

        public List<MODIFIERS> Modifiers { get => modifiers; set => modifiers = value; }
        public string Body { get => body; set => body = value; }

        public override Type Type => throw new NotImplementedException();

        public MethodNode(uint id, string name, string sourceFile, List<Connection> connections) : base(id, name, sourceFile, connections)
        {
        }
    }
}

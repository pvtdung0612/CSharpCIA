using CSharpCIA.src.node.builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.src.node
{
    public class FieldNode : Node
    {
        private List<MODIFIERS> modifiers;

        public List<MODIFIERS> Modifiers { get => modifiers; set => modifiers = value; }

        public override Type Type => throw new NotImplementedException();

        public FieldNode(uint id, string name, string sourceFile, List<Connection> connections) : base(id, name, sourceFile, connections)
        {
        }
    }
}

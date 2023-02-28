using CSharpCIA.CSharpCIA.Nodes;

namespace CSharpCIA.CSharpCIA.Builders
{

    public struct Dependency
    {
        private Node caller;
        private Node callee;
        private string type;

        public string Type { get => type; set => type = value; }
        public Node Caller { get => caller; set => caller = value; }
        public Node Callee { get => callee; set => callee = value; }

        public override string ToString()
        {
            return Type.ToString() + " " + Callee;
        }
    }
}
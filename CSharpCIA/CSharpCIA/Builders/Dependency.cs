using CSharpCIA.CSharpCIA.Nodes;

namespace CSharpCIA.CSharpCIA.Builders
{

    public struct Dependency
    {
        private string caller;
        private string callee;
        private string type;

        public string Type { get => type; set => type = value; }
        public string Caller { get => caller; set => caller = value; }
        public string Callee { get => callee; set => callee = value; }

        public override string ToString()
        {
            return Type + "[" + Caller + "]";
        }
    }
}
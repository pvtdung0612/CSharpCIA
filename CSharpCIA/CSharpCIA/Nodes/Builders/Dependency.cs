namespace CSharpCIA.CSharpCIA.Nodes.Builders
{

    public struct Dependency
    {
        private string caller;
        private string callee;
        private DEPENDENCY_TYPE type;

        public DEPENDENCY_TYPE Type { get => type; set => type = value; }
        public string Caller { get => caller; set => caller = value; }
        public string Callee { get => callee; set => callee = value; }
    }
}
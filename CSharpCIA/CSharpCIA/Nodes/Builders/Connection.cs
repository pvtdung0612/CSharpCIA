namespace CSharpCIA.CSharpCIA.Nodes.Builders
{
    public enum CONNECTIONS
    {
        INVOKE,
        USE,
        MEMBER,
        INHERIT,
        OVERRIDE,
    }

    public class Connection
    {
        private uint otherNode;
        private CONNECTIONS type;

        public uint OtherNode { get => otherNode; set => otherNode = value; }
        public CONNECTIONS Type { get => type; set => type = value; }
    }
}
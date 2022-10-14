using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpCIA.src.node.builder;

namespace CSharpCIA.src.node
{
    public abstract class Node
    {
        private uint id;
        private string name;
        private string sourceFile;
        private List<Connection> connections;

        public uint Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string SourceFile { get => sourceFile; init => sourceFile = value; }
        public List<Connection> Connections { get => connections; set => connections = value; }

        public abstract Type Type { get; }

        protected Node(uint id, string name, string sourceFile, List<Connection> connections)
        {
            Id = id;
            Name = name;
            SourceFile = sourceFile;
            this.connections = connections;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpCIA.CSharpCIA.Nodes.Builders;

namespace CSharpCIA.Test.Nodes
{
    public abstract class Node
    {
        private uint id;
        private string name;
        private string sourcePath;
        private List<Connection> connections;

        public abstract Type Type { get; }
        public uint Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string SourcePath { get => sourcePath; set => sourcePath = value; }

        protected Node(uint id, string name, string sourcePath, List<Connection> connections)
        {
            Id = id;
            Name = name;
            SourcePath = sourcePath;
            this.connections = connections;
        }
    }
}

using CSharpCIA.CSharpCIA.Nodes.Builders;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.Test.Nodes
{
    public class PropertyNode : Node
    {
        private List<Accessor> accessors;

        public PropertyNode(uint id, string name, string sourcePath, List<Connection> connections) : base(id, name, sourcePath, connections)
        {
        }

        public override Type Type => typeof(PropertyNode);
    }

    public class Accessor
    {
        public enum ACCESSOR
        {
            GET,
            SET,
            INIT
        }
        public ACCESSOR Type { get => Type; init => Type = value; }
        public string Body { get; set; }

        public Accessor(ACCESSOR type, string body)
        {
            Type = type;
            Body = body;
        }
    }
}

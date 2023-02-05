using CSharpCIA.CSharpCIA.Nodes.Builders;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.CSharpCIA.Nodes
{
    public class PropertyNode : Node
    {
        private List<Accessor> accessors;

        public PropertyNode(uint id, string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode) : base(id, simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode)
        {
        }

        public override NODE_TYPE Type => NODE_TYPE.PROPERTY;
    }

    // 3864 bỏ
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

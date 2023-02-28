using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CSharpCIA.CSharpCIA.Builders;

namespace CSharpCIA.CSharpCIA.Nodes
{
    public class PropertyNode : Node
    {
        private List<Accessor> accessors;

        public PropertyNode(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode) : base(simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode)
        {
        }

        public override string Type => NODE_TYPE.PROPERTY.ToString();
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

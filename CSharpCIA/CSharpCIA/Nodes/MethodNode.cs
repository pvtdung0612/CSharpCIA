using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CSharpCIA.CSharpCIA.Builders;

namespace CSharpCIA.CSharpCIA.Nodes
{
    public class MethodNode : Node
    {
        private string? body;
        public string? Body { get => body; set => body = value; }
        public override string Type => NODE_TYPE.METHOD.ToString();
        public MethodNode(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode, string body) : base(simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode)
        {
            this.Body = body;
        }
    }
}

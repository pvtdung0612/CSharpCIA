using CSharpCIA.CSharpCIA.Nodes.Builders;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.CSharpCIA.Nodes
{
    public class MethodNode : Node
    {
        private string? body;
        public string? Body { get => body; set => body = value; }
        public override string Type => NODE_TYPE.METHOD.ToString();
        public MethodNode(uint id, string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode, string body) : base(id, simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode)
        {
            this.Body = body;
        }
    }
}

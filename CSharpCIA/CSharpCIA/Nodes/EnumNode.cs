using CSharpCIA.CSharpCIA.Nodes.Builders;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSharpCIA.CSharpCIA.Nodes
{
    internal class EnumNode : Node
    {
        public EnumNode(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode) : base(simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode)
        {
        }

        public override string Type => NODE_TYPE.ENUM.ToString();
    }
}

using CSharpCIA.CSharpCIA.Nodes.Builders;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpCIA.CSharpCIA.Nodes.Builders;

namespace CSharpCIA.CSharpCIA.Nodes
{
    public class ClassNode : Node
    {
        public override string Type => NODE_TYPE.CLASS.ToString();
        public ClassNode(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode) : base(simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode)
        {

        }
    }
}

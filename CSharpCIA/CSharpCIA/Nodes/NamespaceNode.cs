using CSharpCIA.CSharpCIA.Nodes.Builders;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.CSharpCIA.Nodes
{
    public class NamespaceNode : Node
    {

        public override string Type => NODE_TYPE.NAMESPACE.ToString();

        public NamespaceNode(uint id, string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode) : base(id, simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode)
        {
        }

    }
}

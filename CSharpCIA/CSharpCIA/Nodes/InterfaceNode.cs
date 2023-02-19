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
    public class InterfaceNode : Node
    {
        public override string Type => NODE_TYPE.INTERFACE.ToString();
        public InterfaceNode(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode) : base(simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode)
        {

        }
    }
}

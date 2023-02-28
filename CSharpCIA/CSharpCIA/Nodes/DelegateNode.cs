using CSharpCIA.CSharpCIA.Builders;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.CSharpCIA.Nodes
{
    internal class DelegateNode : Node
    {
        public DelegateNode(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode) : base(simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode)
        {
        }

        public override string Type => NODE_TYPE.DELEGATE.ToString();
    }
}

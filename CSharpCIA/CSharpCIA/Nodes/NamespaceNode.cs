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
    public class NamespaceNode : Node
    {
        private List<string> allOriginNames;
        private List<string> allSourcePaths;

        public override string Type => NODE_TYPE.NAMESPACE.ToString();

        public List<string> AllOriginNames { 
            get => allOriginNames; 
            set => allOriginNames = value;
        }
        public List<string> AllSourcePaths { 
            get => allSourcePaths; 
            set => allSourcePaths = value;
        }

        public NamespaceNode(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode) : base(simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode)
        {
            AllOriginNames = new List<string> { originName };
            AllSourcePaths = new List<string> { sourcePath };
        }
    }
}

using CSharpCIA.CSharpCIA.Nodes.Builders;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.CSharpCIA.Nodes
{
    public class RootNode : Node
    {
        public override NODE_TYPE Type => NODE_TYPE.ROOT;

        public List<Node> childrens;

        public List<SyntaxTree> trees;

        public RootNode(uint id, string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode) : base(id, simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode)
        {
            this.childrens = new List<Node>();
            this.trees = new List<SyntaxTree>();
        }
    }
}

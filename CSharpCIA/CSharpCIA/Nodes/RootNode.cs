using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CSharpCIA.CSharpCIA.Builders;

namespace CSharpCIA.CSharpCIA.Nodes
{
    public class RootNode : Node
    {
        public RootNode(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode) : base(simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode)
        {
            this.childrens = new List<Node>();
            this.trees = new List<SyntaxTree>();
        }

        public override string Type => NODE_TYPE.ROOT.ToString();

        public List<Node> childrens;

        [JsonIgnore]
        public List<SyntaxTree> trees;


    }
}

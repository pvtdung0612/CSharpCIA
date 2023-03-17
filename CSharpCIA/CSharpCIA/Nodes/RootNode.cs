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

        public override string Type => NODE_TYPE.ROOT.ToString();

        public List<Node> childrens;

        [JsonIgnore]
        public List<SyntaxTree> trees;

        public RootNode(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode
            ) : base(simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode)
        {
            this.childrens = new List<Node>();
            this.trees = new List<SyntaxTree>();
        }

        public override bool isIdentical(Node node)
        {
            if (node == null || !node.Type.Equals(NODE_TYPE.ROOT.ToString())) return false;

            var other = (RootNode)node;

            // Compare binding name
            if (!this.BindingName.Equals(other.BindingName)) return false;

            return true;
        }
    }
}

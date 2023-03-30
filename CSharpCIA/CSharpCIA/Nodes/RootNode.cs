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

        [JsonIgnore]
        public List<SyntaxTree> trees;

        public RootNode(string simpleName, string qualifiedName, string originName, string sourcePath, string syntax,
            SyntaxTree syntaxTree, SyntaxNode syntaxNode, string id = "") 
            : base(simpleName, qualifiedName, originName, sourcePath, syntax, syntaxTree, syntaxNode, id)
        {
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

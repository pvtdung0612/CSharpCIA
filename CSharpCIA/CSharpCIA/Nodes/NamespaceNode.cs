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

        private List<string>? attributes;
        private List<string>? modifiers;

        public NamespaceNode(string simpleName, string qualifiedName, string originName, string sourcePath, string syntax,
            SyntaxTree syntaxTree, SyntaxNode syntaxNode
            , List<string>? attributes, List<string>? modifiers, string id = "") 
            : base(simpleName, qualifiedName, originName, sourcePath, syntax, syntaxTree, syntaxNode, id)
        {
            AllOriginNames = new List<string> { originName };
            AllSourcePaths = new List<string> { sourcePath };
            Attributes = attributes;
            Modifiers = modifiers;
        }

        public override string Type => NODE_TYPE.NAMESPACE.ToString();

        public List<string> AllOriginNames { 
            get => allOriginNames; 
            set => allOriginNames = value;
        }
        public List<string> AllSourcePaths { 
            get => allSourcePaths; 
            set => allSourcePaths = value;
        }
        public List<string>? Attributes { get => attributes; set => attributes = value; }
        public List<string>? Modifiers { get => modifiers; set => modifiers = value; }

        public override bool isIdentical(Node node)
        {
            if (node == null || !node.Type.Equals(NODE_TYPE.NAMESPACE.ToString())) return false;

            var other = (NamespaceNode)node;

            // Compare binding name
            if (!this.BindingName.Equals(other.BindingName)) return false;

            // Compare attribute
            if (this.Attributes is not null && other.Attributes is null) return false;
            if (this.Attributes is null && other.Attributes is not null) return false;
            if (this.Attributes is not null && other.Attributes is not null)
            {
                if (this.Attributes.Count != other.Attributes.Count)
                    return false;
                else
                {
                    for (int i = 0; i < this.Attributes.Count; i++)
                    {
                        if (!this.Attributes[i].Equals(other.Attributes[i]))
                            return false;
                    }
                }
            }

            // Compare modifiers
            if (this.Modifiers is not null && other.Modifiers is null) return false;
            if (this.Modifiers is null && other.Modifiers is not null) return false;
            if (this.Modifiers is not null && other.Modifiers is not null)
            {
                if (this.Modifiers.Count != other.Modifiers.Count)
                    return false;
                else
                {
                    for (int i = 0; i < this.Modifiers.Count; i++)
                    {
                        if (!this.Modifiers[i].Equals(other.Modifiers[i]))
                            return false;
                    }
                }
            }

            return true;
        }
    }
}

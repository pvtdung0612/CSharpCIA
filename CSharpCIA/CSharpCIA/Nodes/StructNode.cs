using CSharpCIA.CSharpCIA.Builders;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.CSharpCIA.Nodes
{
    internal class StructNode : Node
    {
        private List<string>? attributes;
        private List<string>? modifiers;
        private List<string>? bases;

        public StructNode(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode
, List<string>? attributes, List<string>? modifiers, List<string>? bases, string id = null) 
            : base(simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode, id)
        {
            Attributes = attributes;
            Modifiers = modifiers;
            Bases = bases;
        }

        public override string Type => NODE_TYPE.STRUCT.ToString();

        public List<string>? Attributes { get => attributes; set => attributes = value; }
        public List<string>? Modifiers { get => modifiers; set => modifiers = value; }
        public List<string>? Bases { get => bases; set => bases = value; }

        public override bool isIdentical(Node node)
        {
            if (node == null || !node.Type.Equals(NODE_TYPE.STRUCT.ToString())) return false;

            var other = (StructNode)node;

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

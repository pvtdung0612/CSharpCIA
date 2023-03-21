using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CSharpCIA.CSharpCIA.Builders;

namespace CSharpCIA.CSharpCIA.Nodes
{
    public class PropertyNode : Node
    {
        private List<Accessor> accessors;

        private List<string>? attributes;
        private List<string>? modifiers;

        public PropertyNode(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode
, List<string>? attributes, List<string>? modifiers, string id = null) 
            : base(simpleName, qualifiedName, originName, sourcePath, syntaxTree, syntaxNode, id)
        {
            Attributes = attributes;
            Modifiers = modifiers;
        }

        public override string Type => NODE_TYPE.PROPERTY.ToString();

        public List<string>? Attributes { get => attributes; set => attributes = value; }
        public List<string>? Modifiers { get => modifiers; set => modifiers = value; }

        public override bool isIdentical(Node node)
        {
            if (node == null || !node.Type.Equals(NODE_TYPE.PROPERTY.ToString())) return false;

            var other = (PropertyNode)node;

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

    // 3864 bỏ
    public class Accessor
    {
        public enum ACCESSOR
        {
            GET,
            SET,
            INIT
        }
        public ACCESSOR Type { get => Type; init => Type = value; }
        public string Body { get; set; }

        public Accessor(ACCESSOR type, string body)
        {
            Type = type;
            Body = body;
        }
    }
}

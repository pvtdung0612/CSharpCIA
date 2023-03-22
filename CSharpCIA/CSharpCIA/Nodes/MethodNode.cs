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
    public class MethodNode : Node
    {
        private List<string>? attributes;
        private List<string>? modifiers;
        private bool isContructor;
        private List<string>? parameters;
        private string? body;
        private string returnType;

        public MethodNode(string simpleName, string qualifiedName, string originName, string sourcePath, string syntax,
            SyntaxTree syntaxTree, SyntaxNode syntaxNode,
            List<string> attributes, List<string> modifiers,
            List<string> parameters, string? body, string returnType, bool isContructor = false, string id = "")
            : base(simpleName, qualifiedName, originName, sourcePath, syntax, syntaxTree, syntaxNode, id)
        {
            Body = body;
            Parameters = parameters;
            Attributes = attributes;
            Modifiers = modifiers;
            ReturnType = returnType;
            IsContructor = isContructor;
        }

        public string? Body { get => body; set => body = value; }
        public override string Type => NODE_TYPE.METHOD.ToString();

        public List<string>? Attributes { get => attributes; set => attributes = value; }
        public List<string>? Modifiers { get => modifiers; set => modifiers = value; }
        public List<string>? Parameters { get => parameters; set => parameters = value; }
        public string ReturnType { get => returnType; set => returnType = value; }
        public bool IsContructor { get => isContructor; set => isContructor = value; }

        public override bool isIdentical(Node node)
        {
            if (node == null || !node.Type.Equals(NODE_TYPE.METHOD.ToString())) return false;

            MethodNode other = (MethodNode)node;

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

            // Compare parameters
            if (this.Parameters is not null && other.Parameters is null) return false;
            if (this.Parameters is null && other.Parameters is not null) return false;
            if (this.Parameters is not null && other.Parameters is not null)
            {
                if (this.Parameters.Count != other.Parameters.Count)
                    return false;
                else
                {
                    for (int i = 0; i < this.Parameters.Count; i++)
                    {
                        if (!this.Parameters[i].Equals(other.Parameters[i]))
                            return false;
                    }
                }
            }

            // Compare body
            if (this.Body is not null && other.Body is null) return false;
            if (this.Body is null && other.Body is not null) return false;
            if (this.Body is not null && other.Body is not null)
            {
                if (!this.Body.Equals(other.Body)) return false;
            }

            // Compare return type
            if (this.ReturnType is not null && other.ReturnType is null) return false;
            if (this.ReturnType is null && other.ReturnType is not null) return false;
            if (this.ReturnType is not null && other.ReturnType is not null)
            {
                if (!this.ReturnType.Equals(other.ReturnType)) return false;
            }

            return true;
        }
    }
}

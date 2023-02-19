using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CSharpCIA.CSharpCIA.Nodes.Builders;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace CSharpCIA.CSharpCIA.Nodes
{
    public abstract class Node
    {
        private Guid id;
        private string simpleName; // Ex: MethodName
        private string qualifiedName; // Ex: MethodName(int, int)
        private string originName; // Ex: Directory/File.cs/Namespace/Class/Method(int, int)
        private string bindingName; // For binding Node, equal: originName remove sourcePath, Ex: Namespace/Class/Method(int, int) 
        private string sourcePath; // Ex: Directory/Class
        private SyntaxTree syntaxTree; // For get tree of the file and find path binding
        private SyntaxNode syntaxNode; // For get
        private List<Dependency> dependencies;

        protected Node(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode)
        {
            this.id = Guid.NewGuid();
            this.SimpleName = simpleName;
            this.QualifiedName = qualifiedName;
            this.OriginName = originName;
            this.SourcePath = sourcePath;
            this.SyntaxTree = syntaxTree;
            this.SyntaxNode = syntaxNode;
            this.dependencies = new List<Dependency>();
        }

        public abstract string Type { get; }
        public Guid Id { get => id; }
        public string SimpleName { get => simpleName; set => simpleName = value; }
        public string QualifiedName { get => qualifiedName; set => qualifiedName = value; }
        public string OriginName
        {
            get => originName;
            set
            {
                originName = value;
                if (!String.IsNullOrEmpty(sourcePath))
                {
                    // Node is Root
                    if (originName.Equals(sourcePath))
                    {
                        bindingName = sourcePath;
                    }
                    // Node is not Root
                    else
                    {
                        bindingName = originName.Remove(0, sourcePath.Length + 1);
                    }
                }
            }
        }
        public string SourcePath
        {
            get => sourcePath;
            set
            {
                sourcePath = value;
                if (!String.IsNullOrEmpty(originName) && !String.IsNullOrEmpty(sourcePath))
                {
                    // Node is Root
                    if (originName.Equals(sourcePath))
                    {
                        bindingName = sourcePath;
                    }
                    // Node is not Root
                    else
                    {
                        bindingName = originName.Remove(0, sourcePath.Length + 1);
                    }
                }
            }
        }
        public string BindingName { get => bindingName; }

        [JsonIgnore]
        public SyntaxTree SyntaxTree { get => syntaxTree; set => syntaxTree = value; }
        [JsonIgnore]
        public SyntaxNode SyntaxNode { get => syntaxNode; set => syntaxNode = value; }
        public List<Dependency> Dependencies { get => dependencies; set => dependencies = value; }
    
        public override string ToString()
        {
            return BindingName;
        }
    }
}

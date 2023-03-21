using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Net;

namespace CSharpCIA.CSharpCIA.Nodes
{
    public abstract class Node
    {
        private string id;
        private ulong impact;
        private string simpleName; // Ex: MethodName
        private string qualifiedName; // Ex: MethodName(int, int)
        private string originName; // Ex: Directory/File.cs/Namespace/Class/Method(int, int)
        private string bindingName; // For binding Node, equal: originName remove sourcePath, Ex: Namespace/Class/Method(int, int) 
        private string sourcePath; // Ex: Directory/Class
        private SyntaxTree syntaxTree; // For get tree of the file and find path binding
        private SyntaxNode syntaxNode; // For get syntaxNode from roslyn 

        protected Node(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode, string id = null)
        {
            if (String.IsNullOrEmpty(id)) {
                this.id = Guid.NewGuid().ToString();
            } else
            {
                this.id = id;
            }
            this.SimpleName = simpleName;
            this.QualifiedName = qualifiedName;
            this.OriginName = originName;
            this.SourcePath = sourcePath;
            this.SyntaxTree = syntaxTree;
            this.SyntaxNode = syntaxNode;
        }

        public abstract string Type { get; }
        public string Id { get => id; }
        public ulong Impact { get => impact; set => impact = value; }
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
                        bindingName = "Root";
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
                        bindingName = "Root";
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

        public abstract bool isIdentical(Node node);

        public override bool Equals(object? obj)
        {
            if (obj is not null && obj is Node)
            {
                Node node = obj as Node;
                if (node.Id.Equals(this.Id))
                    return true;
                else
                    return false;
            } else
                return false;
        }

        public override string ToString()
        {
            return Type + "-" + bindingName;
        }
    }
}

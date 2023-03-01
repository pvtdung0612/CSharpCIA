using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace CSharpCIA.CSharpCIA.Nodes
{
    public abstract class Node
    {
        protected Guid id;
        protected string simpleName; // Ex: MethodName
        protected string qualifiedName; // Ex: MethodName(int, int)
        protected string originName; // Ex: Directory/File.cs/Namespace/Class/Method(int, int)
        protected string bindingName; // For binding Node, equal: originName remove sourcePath, Ex: Namespace/Class/Method(int, int) 
        protected string sourcePath; // Ex: Directory/Class
        protected SyntaxTree syntaxTree; // For get tree of the file and find path binding
        protected SyntaxNode syntaxNode; // For get syntaxNode from roslyn 

        protected Node(string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode)
        {
            this.id = Guid.NewGuid();
            this.SimpleName = simpleName;
            this.QualifiedName = qualifiedName;
            this.OriginName = originName;
            this.SourcePath = sourcePath;
            this.SyntaxTree = syntaxTree;
            this.SyntaxNode = syntaxNode;
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
            return simpleName + "-" + Id.ToString();
        }
    }
}

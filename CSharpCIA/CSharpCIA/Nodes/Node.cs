using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpCIA.CSharpCIA.Nodes.Builders;
using Microsoft.CodeAnalysis;

namespace CSharpCIA.CSharpCIA.Nodes
{
    public abstract class Node
    {
        private uint id;
        private string simpleName;
        private string qualifiedName;
        private string originName;
        private string sourcePath;
        private SyntaxTree syntaxTree;
        private SyntaxNode syntaxNode;

        protected Node(uint id, string simpleName, string qualifiedName, string originName, string sourcePath, SyntaxTree syntaxTree, SyntaxNode syntaxNode)
        {
            this.id = id;
            this.simpleName = simpleName;
            this.qualifiedName = qualifiedName;
            this.originName = originName;
            this.sourcePath = sourcePath;
            this.syntaxTree = syntaxTree;
            this.syntaxNode = syntaxNode;
        }

        public abstract NODE_TYPE Type { get; }
        public uint Id { get => id; set => id = value; }
        public string SimpleName { get => simpleName; set => simpleName = value; }
        public string QualifiedName { get => qualifiedName; set => qualifiedName = value; }
        public string OriginName { get => originName; set => originName = value; }
        public string SourcePath { get => sourcePath; set => sourcePath = value; }
        public SyntaxTree SyntaxTree { get => syntaxTree; set => syntaxTree = value; }
        public SyntaxNode SyntaxNode { get => syntaxNode; set => syntaxNode = value; }
    }
}

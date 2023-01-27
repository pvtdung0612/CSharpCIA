using CSharpCIA.CSharpCIA.Nodes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using CSharpCIA.CSharpCIA.Nodes.Builders;
using System.Data;
using System.Xml.Linq;

namespace CSharpCIA.Test.API
{
    public class Parser
    {
        public RootNode ParserFile(string filePath)
        {
            RootNode root = null;
            uint countId = 0;
            if (File.Exists(filePath))
            {
                // Read File
                string fileContent = File.ReadAllText(filePath);

                // Init Root
                root = new RootNode(countId, Path.GetFileName(filePath), filePath, fileContent, new List<Node>(), new List<Connection>());
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(fileContent);
                CompilationUnitSyntax compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot();

                // Get Childrens
                foreach (var item in compilationUnitSyntax.Members)
                {
                    root.childrens.AddRange(ParserNode(ref countId, item, filePath));
                }
            }
            return root;
        }

        public List<Node> ParserNode(ref uint countId, SyntaxNode child, string sourcePath)
        {
            List<Node> transferNodes = null;

            if (child != null)
            {
                transferNodes = new List<Node>();
                if (child is NamespaceDeclarationSyntax)
                {
                    countId++;
                    NamespaceDeclarationSyntax namespaceDeclarationSyntax = (NamespaceDeclarationSyntax)child;
                    NamespaceNode namespaceNode = new NamespaceNode(countId, namespaceDeclarationSyntax.Name.ToString(), sourcePath, null);
                    transferNodes.Add(namespaceNode);
                    foreach (var item in namespaceDeclarationSyntax.Members)
                    {
                        transferNodes.AddRange(ParserNode(ref countId, item, sourcePath));
                    }
                }
                else if (child is ClassDeclarationSyntax)
                {
                    countId++;
                    ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)child;
                    ClassNode classNode = new ClassNode(countId, classDeclaration.Identifier.ToString(), sourcePath, null);
                    transferNodes.Add(classNode);
                    foreach (var item in classDeclaration.Members)
                    {
                        transferNodes.AddRange(ParserNode(ref countId, item, sourcePath));
                    }
                }
                else if (child is FieldDeclarationSyntax)
                {
                    FieldDeclarationSyntax fieldDeclarationSyntax = (FieldDeclarationSyntax)child;
                    foreach (var variable in fieldDeclarationSyntax.Declaration.Variables)
                    {
                        countId++;
                        FieldNode fieldNode = new FieldNode(countId, variable.Identifier.ToString(), sourcePath, null);
                        transferNodes.Add(fieldNode);
                    }
                }
                else if (child is PropertyDeclarationSyntax)
                {
                    countId++;
                    PropertyDeclarationSyntax propertyDeclarationSyntax = (PropertyDeclarationSyntax)child;
                    PropertyNode propertyNode = new PropertyNode(countId, propertyDeclarationSyntax.Identifier.ToString(), sourcePath, null);
                    transferNodes.Add(propertyNode);
                }
                else if (child is MethodDeclarationSyntax)
                {
                    countId++;
                    MethodDeclarationSyntax methodDeclarationSyntax = (MethodDeclarationSyntax)child;

                    // Make uniqueName for MethodNode
                    Boolean check = false;
                    string uniqueName = methodDeclarationSyntax.Identifier.ToString() + "(";
                    foreach (var param in methodDeclarationSyntax.ParameterList.Parameters)
                    {
                        uniqueName += param.Type + ", ";
                        check = true;
                    }
                    if (check) uniqueName = uniqueName.Remove(uniqueName.Length - 2);
                    uniqueName += ")";

                    MethodNode methodNode = new MethodNode(countId, methodDeclarationSyntax.Identifier.ToString(), sourcePath, null, uniqueName, methodDeclarationSyntax.Body.ToString());
                    transferNodes.Add(methodNode);
                }
            }

            return transferNodes;
        }
    }
}
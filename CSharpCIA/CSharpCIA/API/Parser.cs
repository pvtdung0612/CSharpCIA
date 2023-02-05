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
using CSharpCIA.CSharpCIA.Helpers;
using CSharpCIA.CSharpCIA.Nodes.Builders;
using System.IO;

namespace CSharpCIA.CSharpCIA.API
{
    public class Parser
    {
        /// <summary>
        /// This parse Node of Root
        /// </summary>
        /// <param name="path">Path is Directory Path or File Path of Root</param>
        /// <returns>Root</returns>
        public RootNode ParseNode(string path)
        {
            uint countId = 0;
            RootNode root = new RootNode(countId, "Root", "Root", path, path, null, null);

            foreach (var filePath in FileHelper.GetSourceFiles(path))
            {
                Console.WriteLine("Parse File Name: " + filePath);
                if (File.Exists(filePath))
                {
                    // Parser File
                    string fileContent = File.ReadAllText(filePath);
                    SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(fileContent);
                    root.trees.Add(syntaxTree);
                    CompilationUnitSyntax compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot();

                    // Get Childrens
                    foreach (var item in compilationUnitSyntax.Members)
                    {
                        root.childrens.AddRange(ParseNode(ref countId, item, filePath, filePath));
                    }
                }
            }
            return root;
        }
        private List<Node> ParseNode(ref uint countId, SyntaxNode child, string sourcePath, string parentPath)
        {
            List<Node> transferNodes = null;

            if (child != null)
            {
                transferNodes = new List<Node>();
                if (child is NamespaceDeclarationSyntax)
                {
                    countId++;
                    NamespaceDeclarationSyntax namespaceDeclarationSyntax = (NamespaceDeclarationSyntax)child;

                    // Config new transfer Node
                    string originName = parentPath + Path.DirectorySeparatorChar + namespaceDeclarationSyntax.Name.ToString().Replace('.', Path.DirectorySeparatorChar);

                    NamespaceNode namespaceNode = new NamespaceNode(countId, namespaceDeclarationSyntax.Name.ToString(), namespaceDeclarationSyntax.Name.ToString(), originName, sourcePath, child.SyntaxTree, child);
                    transferNodes.Add(namespaceNode);
                    foreach (var item in namespaceDeclarationSyntax.Members)
                    {
                        transferNodes.AddRange(ParseNode(ref countId, item, sourcePath, originName));
                    }
                }
                else if (child is ClassDeclarationSyntax)
                {
                    countId++;
                    ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)child;

                    // Config new transfer Node
                    string originName = parentPath + Path.DirectorySeparatorChar + classDeclaration.Identifier.ToString();

                    ClassNode classNode = new ClassNode(countId, classDeclaration.Identifier.ToString(), classDeclaration.Identifier.ToString(), originName, sourcePath, child.SyntaxTree, child);
                    transferNodes.Add(classNode);
                    foreach (var item in classDeclaration.Members)
                    {
                        transferNodes.AddRange(ParseNode(ref countId, item, sourcePath, originName));
                    }
                }
                else if (child is FieldDeclarationSyntax)
                {
                    FieldDeclarationSyntax fieldDeclarationSyntax = (FieldDeclarationSyntax)child;
                    foreach (var variable in fieldDeclarationSyntax.Declaration.Variables)
                    {
                        // Config new transfer Node
                        string originName = parentPath + Path.DirectorySeparatorChar + variable.Identifier.ToString();

                        countId++;
                        FieldNode fieldNode = new FieldNode(countId, variable.Identifier.ToString(), variable.Identifier.ToString(), originName, sourcePath, child.SyntaxTree, child);
                        transferNodes.Add(fieldNode);
                    }
                }
                else if (child is PropertyDeclarationSyntax)
                {
                    countId++;
                    PropertyDeclarationSyntax propertyDeclarationSyntax = (PropertyDeclarationSyntax)child;

                    // Config new transfer Node
                    string originName = parentPath + Path.DirectorySeparatorChar + propertyDeclarationSyntax.Identifier.ToString();

                    PropertyNode propertyNode = new PropertyNode(countId, propertyDeclarationSyntax.Identifier.ToString(), propertyDeclarationSyntax.Identifier.ToString(), originName, sourcePath, child.SyntaxTree, child);
                    transferNodes.Add(propertyNode);
                }
                else if (child is MethodDeclarationSyntax)
                {
                    countId++;
                    MethodDeclarationSyntax methodDeclarationSyntax = (MethodDeclarationSyntax)child;


                    // Config new transfer Node
                    // Make qualifiedName for MethodNode
                    Boolean check = false;
                    string simpleName = methodDeclarationSyntax.Identifier.ToString();
                    string qualifiedName = simpleName + "(";
                    foreach (var param in methodDeclarationSyntax.ParameterList.Parameters)
                    {
                        qualifiedName += param.Type + ", ";
                        check = true;
                    }
                    if (check) qualifiedName = qualifiedName.Remove(qualifiedName.Length - 2);
                    qualifiedName += ")";
                    string originName = parentPath + Path.DirectorySeparatorChar + qualifiedName;

                    MethodNode methodNode = new MethodNode(countId, methodDeclarationSyntax.Identifier.ToString(), qualifiedName, originName, sourcePath, child.SyntaxTree, child, methodDeclarationSyntax.Body.ToString());
                    transferNodes.Add(methodNode);
                }
                //else if (child is GlobalStatementSyntax)
                //{
                //    countId++;
                //    MethodDeclarationSyntax methodDeclarationSyntax = (MethodDeclarationSyntax)child;


                //     Config new transfer Node
                //     Make qualifiedName for MethodNode
                //    Boolean check = false;
                //    string simpleName = methodDeclarationSyntax.Identifier.ToString();
                //    string qualifiedName = simpleName + "(";
                //    foreach (var param in methodDeclarationSyntax.ParameterList.Parameters)
                //    {
                //        qualifiedName += param.Type + ", ";
                //        check = true;
                //    }
                //    if (check) qualifiedName = qualifiedName.Remove(qualifiedName.Length - 2);
                //    qualifiedName += ")";
                //    string originName = parentPath + Path.DirectorySeparatorChar + qualifiedName;

                //    MethodNode methodNode = new MethodNode(countId, methodDeclarationSyntax.Identifier.ToString(), qualifiedName, originName, sourcePath, child.SyntaxTree, child, methodDeclarationSyntax.Body.ToString());
                //    transferNodes.Add(methodNode);
                //}
            }

            return transferNodes;
        }

        public Tuple<List<Dependency>, RootNode> Parse(string path)
        {
            List<Dependency> dependencies = null;
            RootNode root = ParseNode(path);

            if (root is not null)
            {
                var compilation = CSharpCompilation.Create("compiler")
                    .AddReferences(MetadataReference.CreateFromFile(typeof(string).Assembly.Location));

                var libs = FileHelper.GetAllDllFile(path);
                foreach (var l in libs)
                {
                    Console.WriteLine($"DLL: {l.ToString()}");
                    compilation = compilation.AddReferences(MetadataReference.CreateFromFile(l));
                }

                foreach (var tree in root.trees)
                    compilation = compilation.AddSyntaxTrees(tree);

                dependencies = ParseDependency(root, compilation);
            }

            Tuple<List<Dependency>, RootNode> tuple = Tuple.Create(dependencies, root);
            return tuple;
        }

        /// <summary>
        /// This parse Dependency between two Node in Childrens of Root
        /// </summary>
        /// <param name="path">Path is Directory Path or File Path of Root</param>
        /// <returns>List Dependency of root</returns>
        public List<Dependency> ParseDependency(RootNode root, CSharpCompilation compilation)
        {
            List<Dependency> dependencies = new List<Dependency>();

            if (compilation is not null)
            {
                // Parse Method 
                foreach (Node child in root.childrens.FindAll(n => n.Type is NODE_TYPE.METHOD))
                {
                    MethodNode methodNode = (MethodNode)child;
                    ParseMethodDependency(methodNode, compilation, root, dependencies);
                }
            }

            return dependencies;
        }

        private void ParseMethodDependency(MethodNode methodNode, CSharpCompilation compilation, RootNode root, List<Dependency> dependencies)
        {
            MethodDeclarationSyntax methodSyntax = (MethodDeclarationSyntax)methodNode.SyntaxNode;
            SemanticModel model = compilation.GetSemanticModel(methodNode.SyntaxTree);

            if (methodSyntax is not null && methodSyntax.Body is not null && model is not null)
            {
                var nameSyntax = methodSyntax.Body.DescendantNodes().OfType<IdentifierNameSyntax>();

                foreach (var statement in nameSyntax)
                {
                    var symbol = model.GetSymbolInfo(statement).Symbol;
                    if (symbol is not null)
                    {
                        var callees = root.childrens.FindAll(node =>
                                   node.BindingName.Equals(symbol.ToString().Replace('.', Path.DirectorySeparatorChar)));

                        foreach (var callee in callees)
                        {
                            if (callee.Type.Equals(NODE_TYPE.METHOD))
                            {
                                Dependency d = new Dependency();
                                d.Type = DEPENDENCY_TYPE.INVOKE;
                                d.Caller = methodNode.OriginName;
                                d.Callee = callee.OriginName;
                                dependencies.Add(d);
                            }
                            else if (callee.Type.Equals(NODE_TYPE.FIELD) || callee.Type.Equals(NODE_TYPE.PROPERTY))
                            {
                                Dependency d = new Dependency();
                                d.Type = DEPENDENCY_TYPE.USE;
                                d.Caller = methodNode.OriginName;
                                d.Callee = callee.OriginName;
                                dependencies.Add(d);
                            }
                        }
                    }
                }
            }
        }
    }
}
﻿using CSharpCIA.CSharpCIA.Nodes;
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
using System.Data;
using System.Xml.Linq;
using CSharpCIA.CSharpCIA.Helpers;
using System.IO;
using System.Runtime.CompilerServices;
using CSharpCIA.CSharpCIA.Builders;

namespace CSharpCIA.CSharpCIA.API
{
    public class Parser
    {
        /// <summary>
        /// Parse node and dependency at root
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Tuple<List<Node>, List<Dependency>>? Parse(string path)
        {
            // Validation
            if (!Directory.Exists(path) && (!File.Exists(path) && path.EndsWith(".cs")))
                return null;

            // Parse Node
            List<Node> tranferedNodes = ParseNode(path);
            RootNode root = null;
            foreach (var item in tranferedNodes)
            {
                if (item.Type.Equals(NODE_TYPE.ROOT.ToString()))
                    root = (RootNode)item;
            }

            // Parse Dependency
            List<Dependency> dependencies = null;
            if (root is not null)
            {
                var compilation = CSharpCompilation.Create("compiler")
                    .AddReferences(MetadataReference.CreateFromFile(typeof(string).Assembly.Location));

                var libs = FileHelper.GetAllDllFile(path);
                foreach (var l in libs)
                {
                    //Console.WriteLine($"DLL: {l.ToString()}");
                    compilation = compilation.AddReferences(MetadataReference.CreateFromFile(l));
                }

                foreach (var tree in root.trees)
                    compilation = compilation.AddSyntaxTrees(tree);

                dependencies = ParseDependency(tranferedNodes, compilation);
            }

            // Return
            Tuple<List<Node>, List<Dependency>> tuple = Tuple.Create(tranferedNodes, dependencies);
            return tuple;
        }

        #region Parse Node
        /// <summary>
        /// Parse node at root
        /// </summary>
        /// <param name="path">Path is Directory Path or File Path of Root</param>
        /// <returns>Root</returns>
        public List<Node>? ParseNode(string path)
        {
            // Validation
            if (!Directory.Exists(path) && (!File.Exists(path) && path.EndsWith(".cs")))
                return null;

            List<Node> tranferedNodes = new List<Node>();
            RootNode root = new RootNode("Root", "Root", path, path, "",  null, null);
            tranferedNodes.Add(root);

            foreach (var filePath in FileHelper.GetSourceFiles(path))
            {
                //Console.WriteLine("Parse File Name: " + filePath);
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
                        ParseNode(tranferedNodes, item, filePath, filePath);
                    }
                }
            }
            return tranferedNodes;
        }

        /// <summary>
        /// Parse node at root
        /// </summary>
        /// <param name="child"></param>
        /// <param name="sourcePath"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        private void ParseNode(List<Node> transferNodes, SyntaxNode child, string sourcePath, string parentPath)
        {
            if (child != null)
            {
                if (child is NamespaceDeclarationSyntax)
                {
                    NamespaceDeclarationSyntax namespaceDeclarationSyntax = (NamespaceDeclarationSyntax)child;

                    // Config new transfer Node
                    string originName = parentPath + Path.DirectorySeparatorChar + namespaceDeclarationSyntax.Name.ToString().Replace('.', Path.DirectorySeparatorChar);

                    // Set Attribute
                    List<string> attributes = new List<string>();
                    foreach (var item in namespaceDeclarationSyntax.AttributeLists)
                    {
                        attributes.Add(item.ToString());
                    }

                    // Set modifiers
                    List<string> modifiers = new List<string>();
                    foreach (var item in namespaceDeclarationSyntax.Modifiers)
                    {
                        modifiers.Add(item.ToString());
                    }

                    // Set syntax
                    string syntax = namespaceDeclarationSyntax.ToFullString();

                    NamespaceNode namespaceNode = new NamespaceNode(namespaceDeclarationSyntax.Name.ToString(), namespaceDeclarationSyntax.Name.ToString(), originName, sourcePath, syntax, child.SyntaxTree, child, attributes, modifiers);

                    // Check namespace is exits but different file
                    bool isExistNamespace = false;
                    foreach (var item in transferNodes)
                    {
                        if (item.BindingName.Equals(namespaceNode.BindingName))
                        {
                            isExistNamespace = true;
                            ((NamespaceNode)item).AllOriginNames.Add(originName);
                            ((NamespaceNode)item).AllSourcePaths.Add(sourcePath);
                            break;
                        }
                    }
                    if (!isExistNamespace)
                        transferNodes.Add(namespaceNode);

                    foreach (var item in namespaceDeclarationSyntax.Members)
                    {
                        ParseNode(transferNodes, item, sourcePath, originName);
                    }
                }
                else if (child is ClassDeclarationSyntax)
                {
                    ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)child;

                    // Config new transfer Node
                    string originName = parentPath + Path.DirectorySeparatorChar + classDeclaration.Identifier.ToString();

                    // Set modifiers
                    List<string> modifiers = new List<string>();
                    foreach (var item in classDeclaration.Modifiers)
                    {
                        modifiers.Add(item.ToString());
                    }

                    // Set attributes
                    List<string> attributes = new List<string>();
                    foreach (var item in classDeclaration.AttributeLists)
                    {
                        attributes.Add(item.ToString());
                    }

                    // Set bases gồm những implemented interface và inherited class
                    List<string> bases = new List<string>();
                    var baseList = classDeclaration.BaseList;
                    if (baseList != null)
                    {
                        foreach (var baseTypeSyntax in baseList.Types)
                        {
                            // Lấy ra tên của base
                            bases.Add(baseTypeSyntax.Type.ToString());
                        }
                    }

                    // Set syntax
                    string syntax = classDeclaration.ToFullString();

                    ClassNode classNode = new ClassNode(classDeclaration.Identifier.ToString(), classDeclaration.Identifier.ToString(), originName, sourcePath, syntax, child.SyntaxTree, child,
                        attributes, modifiers, bases);
                    transferNodes.Add(classNode);
                    foreach (var item in classDeclaration.Members)
                    {
                        ParseNode(transferNodes, item, sourcePath, originName);
                    }
                }
                else if (child is FieldDeclarationSyntax)
                {
                    FieldDeclarationSyntax fieldDeclarationSyntax = (FieldDeclarationSyntax)child;

                    // Set modifiers
                    List<string> modifiers = new List<string>();
                    foreach (var item in fieldDeclarationSyntax.Modifiers)
                    {
                        modifiers.Add(item.ToString());
                    }

                    // Set attributes
                    List<string> attributes = new List<string>();
                    foreach (var item in fieldDeclarationSyntax.AttributeLists)
                    {
                        attributes.Add(item.ToString());
                    }

                    // Set VariableType
                    var variableType = SyntaxFactory.ParseTypeName(fieldDeclarationSyntax.Declaration.Type.ToString()).ToString();

                    // Set syntax
                    string syntax = fieldDeclarationSyntax.ToFullString();

                    foreach (var variable in fieldDeclarationSyntax.Declaration.Variables)
                    {
                        // Config new transfer Node
                        string originName = parentPath + Path.DirectorySeparatorChar + variable.Identifier.ToString();

                        // Set VariableValue
                        var variableValue = variable.Initializer?.Value?.ToString();

                        FieldNode fieldNode = new FieldNode(variable.Identifier.ToString(), variable.Identifier.ToString(), originName, sourcePath, syntax, child.SyntaxTree, child, attributes, modifiers, variableType, variableValue);
                        transferNodes.Add(fieldNode);
                    }
                }
                else if (child is PropertyDeclarationSyntax)
                {
                    PropertyDeclarationSyntax propertyDeclarationSyntax = (PropertyDeclarationSyntax)child;

                    // Config new transfer Node
                    string originName = parentPath + Path.DirectorySeparatorChar + propertyDeclarationSyntax.Identifier.ToString();

                    // Set modifiers
                    List<string> modifiers = new List<string>();
                    foreach (var item in propertyDeclarationSyntax.Modifiers)
                    {
                        modifiers.Add(item.ToString());
                    }

                    // Set attributes
                    List<string> attributes = new List<string>();
                    foreach (var item in propertyDeclarationSyntax.AttributeLists)
                    {
                        attributes.Add(item.ToString());
                    }

                    // Set syntax
                    string syntax = propertyDeclarationSyntax.ToFullString();

                    PropertyNode propertyNode = new PropertyNode(propertyDeclarationSyntax.Identifier.ToString(), propertyDeclarationSyntax.Identifier.ToString(), originName, sourcePath, syntax, child.SyntaxTree, child, attributes, modifiers);
                    transferNodes.Add(propertyNode);
                }
                else if (child is MethodDeclarationSyntax)
                {
                    MethodDeclarationSyntax methodDeclarationSyntax = (MethodDeclarationSyntax)child;

                    // Config new transfer Node
                    // Set Name
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

                    // Set syntax
                    string syntax = methodDeclarationSyntax.ToFullString();

                    // Set Attribute
                    List<string> attributes = new List<string>();
                    foreach (var item in methodDeclarationSyntax.AttributeLists)
                    {
                        attributes.Add(item.ToString());
                    }

                    // Set modifiers
                    List<string> modifiers = new List<string>();
                    foreach (var item in methodDeclarationSyntax.Modifiers)
                    {
                        modifiers.Add(item.ToString());
                    }

                    // Set parameters
                    List<string> parametersType = new List<string>();
                    foreach (var param in methodDeclarationSyntax.ParameterList.Parameters)
                    {
                        if (param.Type != null)
                        {
                            parametersType.Add(param.Type.ToString());
                        }
                    }

                    // Set body
                    string body = methodDeclarationSyntax.Body is null ? "" : methodDeclarationSyntax.Body.ToString();

                    // Set return type
                    string returnType = methodDeclarationSyntax.ReturnType is null ? "" : methodDeclarationSyntax.ReturnType.ToString();

                    MethodNode methodNode = new MethodNode(methodDeclarationSyntax.Identifier.ToString(), qualifiedName, originName, sourcePath, syntax, child.SyntaxTree, child,attributes, modifiers, parametersType, body, returnType, false);
                    transferNodes.Add(methodNode);
                }
                else if (child is ConstructorDeclarationSyntax)
                {
                    ConstructorDeclarationSyntax methodDeclarationSyntax = (ConstructorDeclarationSyntax)child;

                    // Config new transfer Node
                    // Set Name
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

                    // Set syntax
                    string syntax = methodDeclarationSyntax.ToFullString();

                    // Set Attribute
                    List<string> attributes = new List<string>();
                    foreach (var item in methodDeclarationSyntax.AttributeLists)
                    {
                        attributes.Add(item.ToString());
                    }

                    // Set modifiers
                    List<string> modifiers = new List<string>();
                    foreach (var item in methodDeclarationSyntax.Modifiers)
                    {
                        modifiers.Add(item.ToString());
                    }

                    // Set parameters
                    List<string> parametersType = new List<string>();
                    foreach (var param in methodDeclarationSyntax.ParameterList.Parameters)
                    {
                        if (param.Type != null)
                        {
                            parametersType.Add(param.Type.ToString());
                        }
                    }

                    // Set body
                    string body = methodDeclarationSyntax.Body is null ? "" : methodDeclarationSyntax.Body.ToString();

                    // Set return type
                    string returnType = "";

                    MethodNode methodNode = new MethodNode(methodDeclarationSyntax.Identifier.ToString(), qualifiedName, originName, sourcePath, syntax, child.SyntaxTree, child, attributes, modifiers, parametersType, body, returnType, true);
                    transferNodes.Add(methodNode);
                }
                else if (child is InterfaceDeclarationSyntax)
                        {
                    InterfaceDeclarationSyntax interfaceDeclarationSyntax = (InterfaceDeclarationSyntax)child;

                    // Config new transfer Node
                    string simpleName = interfaceDeclarationSyntax.Identifier.ToString();
                    string qualifiedName = simpleName;
                    string originName = parentPath + Path.DirectorySeparatorChar + qualifiedName;

                    // Set syntax
                    string syntax = interfaceDeclarationSyntax.ToFullString();

                    // Set modifiers
                    List<string> modifiers = new List<string>();
                    foreach (var item in interfaceDeclarationSyntax.Modifiers)
                    {
                        modifiers.Add(item.ToString());
                    }

                    // Set attributes
                    List<string> attributes = new List<string>();
                    foreach (var item in interfaceDeclarationSyntax.AttributeLists)
                    {
                        attributes.Add(item.ToString());
                    }

                    // Set bases gồm những implemented interface và inherited class
                    List<string> bases = new List<string>();
                    var baseList = interfaceDeclarationSyntax.BaseList;
                    if (baseList != null)
                    {
                        foreach (var baseTypeSyntax in baseList.Types)
                        {
                            // Lấy ra tên của base
                            bases.Add(baseTypeSyntax.Type.ToString());
                        }
                    }

                    InterfaceNode interfaceNode = new InterfaceNode(interfaceDeclarationSyntax.Identifier.ToString(), qualifiedName, originName, sourcePath, syntax, child.SyntaxTree, child, attributes, modifiers, bases);
                    transferNodes.Add(interfaceNode);
                    foreach (var item in interfaceDeclarationSyntax.Members)
                    {
                        ParseNode(transferNodes, item, sourcePath, originName);
                    }
                }
                else if (child is StructDeclarationSyntax)
                {
                    StructDeclarationSyntax structDeclarationSyntax = (StructDeclarationSyntax)child;

                    // Config new transfer Node
                    string simpleName = structDeclarationSyntax.Identifier.ToString();
                    string qualifiedName = simpleName;
                    string originName = parentPath + Path.DirectorySeparatorChar + qualifiedName;

                    // Set syntax
                    string syntax = structDeclarationSyntax.ToFullString();

                    // Set modifiers
                    List<string> modifiers = new List<string>();
                    foreach (var item in structDeclarationSyntax.Modifiers)
                    {
                        modifiers.Add(item.ToString());
                    }

                    // Set attributes
                    List<string> attributes = new List<string>();
                    foreach (var item in structDeclarationSyntax.AttributeLists)
                    {
                        attributes.Add(item.ToString());
                    }

                    // Set bases gồm những implemented interface và inherited struct
                    List<string> bases = new List<string>();
                    var baseList = structDeclarationSyntax.BaseList;
                    if (baseList != null)
                    {
                        foreach (var baseTypeSyntax in baseList.Types)
                        {
                            // Lấy ra tên của base
                            bases.Add(baseTypeSyntax.Type.ToString());
                        }
                    }

                    StructNode structNode = new StructNode(structDeclarationSyntax.Identifier.ToString(), qualifiedName, originName, sourcePath, syntax, child.SyntaxTree, child, attributes, modifiers, bases);
                    transferNodes.Add(structNode);
                }
                else if (child is EnumDeclarationSyntax)
                {
                    EnumDeclarationSyntax enumDeclarationSyntax = (EnumDeclarationSyntax)child;

                    // Config new transfer Node
                    string simpleName = enumDeclarationSyntax.Identifier.ToString();
                    string qualifiedName = simpleName;
                    string originName = parentPath + Path.DirectorySeparatorChar + qualifiedName;

                    // Set syntax
                    string syntax = enumDeclarationSyntax.ToFullString();

                    // Set modifiers
                    List<string> modifiers = new List<string>();
                    foreach (var item in enumDeclarationSyntax.Modifiers)
                    {
                        modifiers.Add(item.ToString());
                    }

                    // Set attributes
                    List<string> attributes = new List<string>();
                    foreach (var item in enumDeclarationSyntax.AttributeLists)
                    {
                        attributes.Add(item.ToString());
                    }

                    // Set bases gồm những implemented interface và inherited enum
                    List<string> bases = new List<string>();
                    var baseList = enumDeclarationSyntax.BaseList;
                    if (baseList != null)
                    {
                        foreach (var baseTypeSyntax in baseList.Types)
                        {
                            // Lấy ra tên của base
                            bases.Add(baseTypeSyntax.Type.ToString());
                        }
                    }

                    EnumNode enumNode = new EnumNode(enumDeclarationSyntax.Identifier.ToString(), qualifiedName, originName, sourcePath, syntax, child.SyntaxTree, child, attributes, modifiers, bases);
                    transferNodes.Add(enumNode);
                }
                else if (child is DelegateDeclarationSyntax)
                {
                    DelegateDeclarationSyntax delegateDeclarationSyntax = (DelegateDeclarationSyntax)child;

                    // Config new transfer Node
                    string simpleName = delegateDeclarationSyntax.Identifier.ToString();
                    string qualifiedName = simpleName;
                    string originName = parentPath + Path.DirectorySeparatorChar + qualifiedName;

                    // Set syntax
                    string syntax = delegateDeclarationSyntax.ToFullString();

                    // Set modifiers
                    List<string> modifiers = new List<string>();
                    foreach (var item in delegateDeclarationSyntax.Modifiers)
                    {
                        modifiers.Add(item.ToString());
                    }

                    // Set attributes
                    List<string> attributes = new List<string>();
                    foreach (var item in delegateDeclarationSyntax.AttributeLists)
                    {
                        attributes.Add(item.ToString());
                    }

                    DelegateNode delegateNode = new DelegateNode(delegateDeclarationSyntax.Identifier.ToString(), qualifiedName, originName, sourcePath, syntax, child.SyntaxTree, child, attributes, modifiers);
                    transferNodes.Add(delegateNode);
                }

                // 3864 Improve
                //else if (child is GlobalStatementSyntax)
                //{
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

                //    MethodNode methodNode = new MethodNode(methodDeclarationSyntax.Identifier.ToString(), qualifiedName, originName, sourcePath, child.SyntaxTree, child, methodDeclarationSyntax.Body.ToString());
                //    transferNodes.Add(methodNode);
                //}
            }
        }
        #endregion

        #region Parse Dependency
        /// <summary>
        /// Parse dependency at root
        /// </summary>
        /// <param name="root"></param>
        /// <param name="compilation"></param>
        /// <returns></returns>
        public List<Dependency> ParseDependency(List<Node> tranferedNodes, CSharpCompilation compilation)
        {
            List<Dependency> dependencies = new List<Dependency>();

            if (tranferedNodes is not null && compilation is not null)
            {
                // Parse all dependency
                foreach (Node child in tranferedNodes)
                {
                    dependencies.AddRange(ParseUseDependency(child, compilation, tranferedNodes));
                    dependencies.AddRange(ParseInvokeDependency(child, compilation, tranferedNodes));
                    dependencies.AddRange(ParseInheritDependency(child, compilation, tranferedNodes));
                    dependencies.AddRange(ParseImplementDependency(child, compilation, tranferedNodes));
                    dependencies.AddRange(ParseOverrideDependency(child, compilation, tranferedNodes));
                    dependencies.AddRange(ParseCallbackDependency(child, compilation, tranferedNodes));
                }
                dependencies.AddRange(ParseOwnDependency(tranferedNodes));
            }

            return dependencies;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">Type is MethodNode</param>
        /// <param name="compilation"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private List<Dependency> ParseUseDependency(Node node, CSharpCompilation compilation, List<Node> tranferedNodes)
        {
            var dependencies = new List<Dependency>();
            if (!node.Type.Equals(NODE_TYPE.METHOD.ToString()))
                return dependencies;
            SemanticModel model = compilation.GetSemanticModel(node.SyntaxTree);

            if (node.SyntaxNode.Kind() == SyntaxKind.MethodDeclaration)
            {
                MethodDeclarationSyntax methodSyntax = (MethodDeclarationSyntax)node.SyntaxNode;

                if (methodSyntax is not null && methodSyntax.Body is not null && model is not null)
                {
                    var nameSyntax = methodSyntax.Body.DescendantNodes().OfType<IdentifierNameSyntax>();

                    foreach (var statement in nameSyntax)
                    {
                        var symbol = model.GetSymbolInfo(statement).Symbol;
                        if (symbol is not null)
                        {
                            var callees = tranferedNodes.FindAll(node =>
                                       node.BindingName.Equals(symbol.ToString().Replace('.', Path.DirectorySeparatorChar)));

                            foreach (var callee in callees)
                            {
                                if (callee.Type.Equals(NODE_TYPE.FIELD.ToString()) || callee.Type.Equals(NODE_TYPE.PROPERTY.ToString()))
                                {
                                    Dependency d = new Dependency();
                                    d.Type = DEPENDENCY_TYPE.USE.ToString();
                                    d.Caller = node.Id;
                                    d.Callee = callee.Id;
                                    dependencies.Add(d);
                                }
                            }
                        }
                    }
                }
            } 
            else if (node.SyntaxNode.Kind() == SyntaxKind.ConstructorDeclaration)
            {
                ConstructorDeclarationSyntax constructorSyntax = (ConstructorDeclarationSyntax)node.SyntaxNode;

                if (constructorSyntax is not null && constructorSyntax.Body is not null && model is not null)
                {
                    var nameSyntax = constructorSyntax.Body.DescendantNodes().OfType<IdentifierNameSyntax>();

                    foreach (var statement in nameSyntax)
                    {
                        var symbol = model.GetSymbolInfo(statement).Symbol;
                        if (symbol is not null)
                        {
                            var callees = tranferedNodes.FindAll(node =>
                                       node.BindingName.Equals(symbol.ToString().Replace('.', Path.DirectorySeparatorChar)));

                            foreach (var callee in callees)
                            {
                                if (callee.Type.Equals(NODE_TYPE.FIELD.ToString()) || callee.Type.Equals(NODE_TYPE.PROPERTY.ToString()))
                                {
                                    Dependency d = new Dependency();
                                    d.Type = DEPENDENCY_TYPE.USE.ToString();
                                    d.Caller = node.Id;
                                    d.Callee = callee.Id;
                                    dependencies.Add(d);
                                }
                            }
                        }
                    }
                }
            }

            return dependencies;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">Type is MethodNode</param>
        /// <param name="compilation"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private List<Dependency> ParseInvokeDependency(Node node, CSharpCompilation compilation, List<Node> tranferedNodes)
        {
            var dependencies = new List<Dependency>();
            if (!node.Type.Equals(NODE_TYPE.METHOD.ToString()))
                return dependencies;
            SemanticModel model = compilation.GetSemanticModel(node.SyntaxTree);

            if (node.SyntaxNode.Kind() == SyntaxKind.MethodDeclaration)
            {
                MethodDeclarationSyntax methodSyntax = (MethodDeclarationSyntax)node.SyntaxNode;

                if (methodSyntax is not null && methodSyntax.Body is not null && model is not null)
                {
                    var nameSyntax = methodSyntax.Body.DescendantNodes().OfType<IdentifierNameSyntax>();

                    foreach (var statement in nameSyntax)
                    {
                        var symbol = model.GetSymbolInfo(statement).Symbol;
                        if (symbol is not null)
                        {
                            var callees = tranferedNodes.FindAll(node =>
                                       node.BindingName.Equals(symbol.ToString().Replace('.', Path.DirectorySeparatorChar)));

                            foreach (var callee in callees)
                            {
                                if (callee.Type.Equals(NODE_TYPE.METHOD.ToString()))
                                {
                                    Dependency d = new Dependency();
                                    d.Type = DEPENDENCY_TYPE.INVOKE.ToString();
                                    d.Caller = node.Id;
                                    d.Callee = callee.Id;
                                    dependencies.Add(d);
                                }
                            }
                        }
                    }
                }
            }
            else if (node.SyntaxNode.Kind() == SyntaxKind.ConstructorDeclaration)
            {
                ConstructorDeclarationSyntax constructorDeclaration = (ConstructorDeclarationSyntax)node.SyntaxNode;

                if (constructorDeclaration is not null && constructorDeclaration.Body is not null && model is not null)
                {
                    var nameSyntax = constructorDeclaration.Body.DescendantNodes().OfType<IdentifierNameSyntax>();

                    foreach (var statement in nameSyntax)
                    {
                        var symbol = model.GetSymbolInfo(statement).Symbol;
                        if (symbol is not null)
                        {
                            var callees = tranferedNodes.FindAll(node =>
                                       node.BindingName.Equals(symbol.ToString().Replace('.', Path.DirectorySeparatorChar)));

                            foreach (var callee in callees)
                            {
                                if (callee.Type.Equals(NODE_TYPE.METHOD.ToString()))
                                {
                                    Dependency d = new Dependency();
                                    d.Type = DEPENDENCY_TYPE.INVOKE.ToString();
                                    d.Caller = node.Id;
                                    d.Callee = callee.Id;
                                    dependencies.Add(d);
                                }
                            }
                        }
                    }
                }
            }

            return dependencies;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">Type is ClassNode, StructNode</param>
        /// <param name="compilation"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private List<Dependency> ParseInheritDependency(Node node, CSharpCompilation compilation, List<Node> tranferedNodes)
        {
            var dependencies = new List<Dependency>();
            if (!node.Type.Equals(NODE_TYPE.CLASS.ToString()))
                return dependencies;
            SemanticModel model = compilation.GetSemanticModel(node.SyntaxTree);

            if (node is not null && model is not null && tranferedNodes is not null)
            {
                var namedTypeSymbol = model.GetDeclaredSymbol(node.SyntaxNode);
                var symbol = namedTypeSymbol is null ? null : (INamedTypeSymbol)namedTypeSymbol;
                if (symbol is not null)
                {
                    if (symbol.BaseType is not null)
                    {
                        List<Node> baseClasses = tranferedNodes.FindAll(node => node.Type.Equals(NODE_TYPE.CLASS.ToString())
                        && node.BindingName.Equals(symbol.BaseType.ToString().Replace('.', Path.DirectorySeparatorChar)));

                        foreach (var baseClass in baseClasses)
                        {
                            Dependency dependency = new Dependency();
                            dependency.Type = DEPENDENCY_TYPE.INHERIT.ToString();
                            dependency.Caller = node.Id;
                            dependency.Callee = baseClass.Id;
                            dependencies.Add(dependency);
                        }
                    }
                }
            }

            return dependencies;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">Type is ClassNode, StructNode, InterfaceNode</param>
        /// <param name="compilation"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private List<Dependency> ParseImplementDependency(Node node, CSharpCompilation compilation, List<Node> tranferedNodes)
        {
            var dependencies = new List<Dependency>();
            if (!node.Type.Equals(NODE_TYPE.CLASS.ToString()) && !node.Type.Equals(NODE_TYPE.INTERFACE.ToString()) && !node.Type.Equals(NODE_TYPE.STRUCT))
                return dependencies;
            SemanticModel model = compilation.GetSemanticModel(node.SyntaxTree);

            if (node is not null && model is not null && tranferedNodes is not null)
            {
                var namedTypeSymbol = model.GetDeclaredSymbol(node.SyntaxNode);
                var symbol = namedTypeSymbol is null ? null : (INamedTypeSymbol)namedTypeSymbol;
                if (symbol is not null)
                {
                    // get list interface relative direct with classNode
                    var interfaceDirectes = symbol.Interfaces;

                    foreach (var interfaceDirect in interfaceDirectes)
                    {
                        var interfaceNodes = tranferedNodes.FindAll(node =>
                           node.Type.Equals(NODE_TYPE.INTERFACE.ToString())
                           && node.BindingName.Equals(interfaceDirect.ToString().Replace('.', Path.DirectorySeparatorChar)));

                        foreach (var interfaceNode in interfaceNodes)
                        {
                            Dependency dependency = new Dependency();
                            dependency.Type = DEPENDENCY_TYPE.IMPLEMENT.ToString();
                            dependency.Caller = node.Id;
                            dependency.Callee = interfaceNode.Id;
                            dependencies.Add(dependency);
                        }
                    }
                }
            }

            return dependencies;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">Type is ClassNode, StructNode</param>
        /// <param name="compilation"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private List<Dependency> ParseOverrideDependency(Node node, CSharpCompilation compilation, List<Node> tranferedNodes)
        {
            var dependencies = new List<Dependency>(); 
            if (!node.Type.Equals(NODE_TYPE.CLASS.ToString()) && !node.Type.Equals(NODE_TYPE.STRUCT))
                return dependencies;
            SemanticModel model = compilation.GetSemanticModel(node.SyntaxTree);

            if (node is not null && model is not null && tranferedNodes is not null)
            {
                var namedTypeSymbol = model.GetDeclaredSymbol(node.SyntaxNode);
                var symbol = namedTypeSymbol is null ? null : (INamedTypeSymbol)namedTypeSymbol;
                if (symbol is not null)
                {
                    // PREPARE
                    var methodOfNode = tranferedNodes.FindAll(item =>
                    item.Type.Equals(NODE_TYPE.METHOD.ToString()) && item.BindingName.Remove(item.BindingName.Length - item.QualifiedName.Length - 1)
                    .Equals(node.BindingName)); // Get method in node

                    var interfaceDirectes = symbol.Interfaces; // get list interface relative direct with node
                    var interfaceRelatives = symbol.AllInterfaces.ToList(); // get list interface relative with node
                    var interfaceIndirectes = interfaceRelatives.FindAll(n => !interfaceDirectes.ToList().Contains(n)); // get list interface relative indirect with node

                    List<Node> methodDirectNodes = new List<Node>();
                    List<Node> methodIndirectNodes = new List<Node>();

                    // Get method in interface direct
                    foreach (var interfaceDirect in interfaceDirectes)
                    {
                        // Get internal interfaces in root, ignore external interfaces 
                        var interfaceNodes = tranferedNodes.FindAll(node => node.Type.Equals(NODE_TYPE.INTERFACE.ToString())
                            && node.BindingName.Equals(interfaceDirect.ToString().Replace('.', Path.DirectorySeparatorChar)));

                        foreach (var interfaceNode in interfaceNodes)
                        {
                            methodDirectNodes.AddRange(tranferedNodes.FindAll(node => node.Type.Equals(NODE_TYPE.METHOD.ToString())
                            && node.BindingName.Remove(node.BindingName.Length - node.QualifiedName.Length - 1)
                            .Equals(interfaceNode.BindingName)));

                        }
                    }

                    // Get method in interface indirect
                    foreach (var interfaceIndirect in interfaceIndirectes)
                    {
                        // Get internal interfaces in root, ignore external interfaces 
                        var interfaceNodes = tranferedNodes.FindAll(node =>node.Type.Equals(NODE_TYPE.INTERFACE.ToString())
                            && node.BindingName.Equals(interfaceIndirect.ToString().Replace('.', Path.DirectorySeparatorChar)));

                        foreach (var interfaceNode in interfaceNodes)
                        {
                            methodIndirectNodes.AddRange(tranferedNodes.FindAll(node => node.Type.Equals(NODE_TYPE.METHOD.ToString())
                            && node.BindingName.Remove(node.BindingName.Length - node.QualifiedName.Length - 1)
                            .Equals(interfaceNode.BindingName)));
                        }
                    }

                    foreach (var methodNode in methodOfNode)
                    {
                        // OVERRIDE: CLASS - CLASS
                        var methodSymbol = model.GetDeclaredSymbol(methodNode.SyntaxNode);
                        var iMethodSymbol = methodSymbol is null ? null : (IMethodSymbol)methodSymbol;

                        if (node.Type.Equals(NODE_TYPE.CLASS.ToString()) && iMethodSymbol is not null && iMethodSymbol.IsOverride)
                        {
                            var superMethodNodes = tranferedNodes.FindAll(n => n.BindingName.Equals(iMethodSymbol.OverriddenMethod.ToString().Replace('.', Path.DirectorySeparatorChar)));

                            foreach (var superMethodNode in superMethodNodes)
                            {
                                Dependency dependency = new Dependency();
                                dependency.Type = DEPENDENCY_TYPE.OVERRIDE.ToString();
                                dependency.Caller = methodNode.Id;
                                dependency.Callee = superMethodNode.Id;
                                dependencies.Add(dependency);
                            }
                        }
                        else
                        {
                            // OVERRIDE: CLASS/STRUCT - INTERFACE DIRECT
                            var methodDirectOverrides = methodDirectNodes.FindAll(n => n.QualifiedName.Equals(methodNode.QualifiedName));
                            if (methodDirectOverrides.Count > 0)
                            {
                                foreach (var item in methodDirectOverrides)
                                {
                                    Dependency dependency = new Dependency();
                                    dependency.Type = DEPENDENCY_TYPE.OVERRIDE.ToString();
                                    dependency.Caller = methodNode.Id;
                                    dependency.Callee = item.Id;
                                    dependencies.Add(dependency);
                                }
                            }
                            // OVERRIDE: CLASS/STRUCT - INTERFACE INDIRECT
                            else
                            {
                                var methodIndirectOverrides = methodIndirectNodes.FindAll(n => n.QualifiedName.Equals(methodNode.QualifiedName));
                                foreach (var item in methodIndirectOverrides)
                                {
                                    Dependency dependency = new Dependency();
                                    dependency.Type = DEPENDENCY_TYPE.OVERRIDE.ToString();
                                    dependency.Caller = methodNode.Id;
                                    dependency.Callee = item.Id;
                                    dependencies.Add(dependency);
                                }
                            }
                        }
                    }
                }
            }

            return dependencies;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">Type is method node</param>
        /// <param name="compilation"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private List<Dependency> ParseCallbackDependency(Node node, CSharpCompilation compilation, List<Node> tranferedNodes)
        {
            var dependencies = new List<Dependency>();
            if (!node.Type.Equals(NODE_TYPE.METHOD.ToString()))
                return dependencies;
            SemanticModel model = compilation.GetSemanticModel(node.SyntaxTree);
            
            if (node.SyntaxNode.Kind() == SyntaxKind.MethodDeclaration) {
                MethodDeclarationSyntax methodSyntax = (MethodDeclarationSyntax)node.SyntaxNode;

                if (methodSyntax is not null && methodSyntax.ParameterList is not null && model is not null)
                {
                    var parameterSyntaxes = methodSyntax.ParameterList.Parameters;

                    foreach (var parameterSyntax in parameterSyntaxes)
                    {
                        var symbol = model.GetDeclaredSymbol(parameterSyntax);
                        if (symbol is not null)
                        {
                            var delegateNodes = tranferedNodes.FindAll(n => n.Type.Equals(NODE_TYPE.DELEGATE)
                            && n.BindingName.Equals(symbol.ToString().Replace('.', Path.DirectorySeparatorChar)));

                            foreach (var delegateNode in delegateNodes)
                            {
                                Dependency dependency = new Dependency();
                                dependency.Type = DEPENDENCY_TYPE.CALLBACK.ToString();
                                dependency.Caller = node.Id;
                                dependency.Callee = delegateNode.Id;
                                dependencies.Add(dependency);
                            }
                        }
                    }
                }
            }
            else if (node.SyntaxNode.Kind() == SyntaxKind.ConstructorDeclaration)
            {
                ConstructorDeclarationSyntax constructorSyntax = (ConstructorDeclarationSyntax)node.SyntaxNode;

                if (constructorSyntax is not null && constructorSyntax.ParameterList is not null && model is not null)
                {
                    var parameterSyntaxes = constructorSyntax.ParameterList.Parameters;

                    foreach (var parameterSyntax in parameterSyntaxes)
                    {
                        var symbol = model.GetDeclaredSymbol(parameterSyntax);
                        if (symbol is not null)
                        {
                            var delegateNodes = tranferedNodes.FindAll(n => n.Type.Equals(NODE_TYPE.DELEGATE)
                            && n.BindingName.Equals(symbol.ToString().Replace('.', Path.DirectorySeparatorChar)));

                            foreach (var delegateNode in delegateNodes)
                            {
                                Dependency dependency = new Dependency();
                                dependency.Type = DEPENDENCY_TYPE.CALLBACK.ToString();
                                dependency.Caller = node.Id;
                                dependency.Callee = delegateNode.Id;
                                dependencies.Add(dependency);
                            }
                        }
                    }
                }
            }

            return dependencies;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">Type is all node</param>
        /// <param name="root"></param>
        /// <returns></returns>
        private List<Dependency> ParseOwnDependency(List<Node> tranferedNodes)
        {
            var dependencies = new List<Dependency>();

            // 3864: Optimize need optimize performance
            // Thêm quan hệ sở hữu cho namespace và những node có cấp bằng hoặc thấp hơn
            foreach (var node in tranferedNodes)
            {
                foreach (var item in tranferedNodes)
                {
                    // Ex: item is Test10//Animal. node is Test10
                    // MyApp
                    // Test10
                    // Test10//Animal
                    // Test10//Animal//Sound
                    // item, node also in tranferedNodes

                    string childPath = "";
                    if (node.BindingName.Length < item.BindingName.Length)
                    {
                        childPath = item.BindingName.Remove(0, node.BindingName.Length);
                    }
                    if (!String.IsNullOrEmpty(childPath)
                        && childPath.Count(c => c.Equals(Path.DirectorySeparatorChar)) == 1
                        && item.BindingName.StartsWith(node.BindingName))
                    {
                        Dependency dependency = new Dependency();
                        dependency.Type = DEPENDENCY_TYPE.CONTAIN.ToString();
                        dependency.Caller = node.Id;
                        dependency.Callee = item.Id;
                        dependencies.Add(dependency);
                    }
                }
            }

            // Thêm quan hệ giữa Root và namespace
            // 3864: Otimize performance increase - memory decrease
            // Get Root
            RootNode root = null;
            foreach (var item in tranferedNodes)
            {
                if (item.Type.Equals(NODE_TYPE.ROOT.ToString()))
                    root = (RootNode)item;
            }
            if (root is not null)
            {
                // convert dependencies to Dictionary to increase program performance
                Dictionary<string, Dependency> dicNamespaceCallee = new Dictionary<string, Dependency>(); // <Callee.Id, Dependency>
                Dictionary<string, Node> dicTransferedNodes = tranferedNodes.ToDictionary(keySelector: n => n.Id, elementSelector: n => n);
                foreach (var item in dependencies)
                {
                    if (dicTransferedNodes.ContainsKey(item.Callee) && dicTransferedNodes[item.Callee].Type.Equals(NODE_TYPE.NAMESPACE.ToString()))
                    {
                        dicNamespaceCallee.Add(dicTransferedNodes[item.Callee].Id, item);
                    }
                }

                foreach (var item in tranferedNodes)
                {
                    if (item.Type == NODE_TYPE.NAMESPACE.ToString())
                    {
                        if (!dicNamespaceCallee.ContainsKey(item.Id))
                        {
                            Dependency dependency = new Dependency();
                            dependency.Type = DEPENDENCY_TYPE.CONTAIN.ToString();
                            dependency.Caller = root.Id;
                            dependency.Callee = item.Id;
                            dependencies.Add(dependency);
                        }
                    }
                }
            }

            return dependencies;
            // 3864: History
            //// Thêm quan hệ giữa Root và namespace
            //if (node.Type.Equals(NODE_TYPE.NAMESPACE.ToString()) && node.BindingName.Count(c => c.Equals(Path.DirectorySeparatorChar)) == 0)
            //{
            //    Dependency dependency = new Dependency();
            //    dependency.Type = DEPENDENCY_TYPE.OWN.ToString();
            //    dependency.Caller = root;
            //    dependency.Callee = node;
            //    dependencies.Add(dependency);
            //}
        }

        #endregion

        #region History
        ///// <summary>
        ///// Parse dependency of method
        ///// </summary>
        ///// <param name="methodNode"></param>
        ///// <param name="compilation"></param>
        ///// <param name="root"></param>
        ///// <returns></returns>
        //private List<Dependency> ParseMethodDependency(MethodNode methodNode, CSharpCompilation compilation, RootNode root)
        //{
        //    List<Dependency> dependencies = new List<Dependency>();
        //    SemanticModel model = compilation.GetSemanticModel(methodNode.SyntaxTree);
        //    MethodDeclarationSyntax methodSyntax = (MethodDeclarationSyntax)methodNode.SyntaxNode;

        //    if (methodSyntax is not null && methodSyntax.Body is not null && model is not null)
        //    {
        //        var nameSyntax = methodSyntax.Body.DescendantNodes().OfType<IdentifierNameSyntax>();

        //        foreach (var statement in nameSyntax)
        //        {
        //            var symbol = model.GetSymbolInfo(statement).Symbol;
        //            if (symbol is not null)
        //            {
        //                var callees = tranferedNodes.FindAll(node =>
        //                           node.BindingName.Equals(symbol.ToString().Replace('.', Path.DirectorySeparatorChar)));

        //                foreach (var callee in callees)
        //                {
        //                    if (callee.Type.Equals(NODE_TYPE.METHOD.ToString()))
        //                    {
        //                        Dependency d = new Dependency();
        //                        d.Type = DEPENDENCY_TYPE.INVOKE.ToString();
        //                        d.Caller = methodNode.OriginName;
        //                        d.Callee = callee.OriginName;
        //                        dependencies.Add(d);
        //                    }
        //                    else if (callee.Type.Equals(NODE_TYPE.FIELD.ToString()) || callee.Type.Equals(NODE_TYPE.PROPERTY.ToString()))
        //                    {
        //                        Dependency d = new Dependency();
        //                        d.Type = DEPENDENCY_TYPE.USE.ToString();
        //                        d.Caller = methodNode.OriginName;
        //                        d.Callee = callee.OriginName;
        //                        dependencies.Add(d);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return dependencies;
        //}


        //private List<Dependency> ParseInterfaceDependency(InterfaceNode interfaceNode, CSharpCompilation compilation, RootNode root)
        //{
        //    List<Dependency> dependencies = new List<Dependency>();
        //    SemanticModel model = compilation.GetSemanticModel(interfaceNode.SyntaxTree);

        //    if (interfaceNode is not null)
        //    {
        //        var namedTypeSymbol = model.GetDeclaredSymbol(interfaceNode.SyntaxNode);
        //        var symbol = namedTypeSymbol is null ? null : (INamedTypeSymbol)namedTypeSymbol;
        //        if (symbol is not null)
        //        {

        //            //// IMPLEMENT: INTERFACE IMPLEMENT INTERFACE
        //            //var interfaceDirectes = symbol.Interfaces; // get list interface relative direct with classNode

        //            //foreach (var interfaceDirect in interfaceDirectes)
        //            //{
        //            //    var interfaceDirectNodes = tranferedNodes.FindAll(node =>
        //            //       node.Type.Equals(NODE_TYPE.INTERFACE.ToString())
        //            //       && node.BindingName.Equals(interfaceDirect.ToString().Replace('.', Path.DirectorySeparatorChar)));

        //            //    foreach (var interfaceDirectNode in interfaceDirectNodes)
        //            //    {
        //            //        Dependency dependency = new Dependency();
        //            //        dependency.Type = DEPENDENCY_TYPE.IMPLEMENT.ToString();
        //            //        dependency.Caller = interfaceNode.OriginName;
        //            //        dependency.Callee = interfaceDirectNode.OriginName;
        //            //        dependencies.Add(dependency);
        //            //    }
        //            //}
        //        }
        //    }

        //    return dependencies;
        //}


        //private List<Dependency> ParseClassDependency(ClassNode classNode, CSharpCompilation compilation, RootNode root)
        //{
        //    List<Dependency> dependencies = new List<Dependency>();
        //    SemanticModel model = compilation.GetSemanticModel(classNode.SyntaxTree);

        //    if (classNode is not null)
        //    {
        //        var namedTypeSymbol = model.GetDeclaredSymbol(classNode.SyntaxNode);
        //        var symbol = namedTypeSymbol is null ? null : (INamedTypeSymbol)namedTypeSymbol;
        //        if (symbol is not null)
        //        {
        //            //// INHERIT
        //            //if (symbol.BaseType is not null)
        //            //{
        //            //    List<Node> baseClasses = tranferedNodes.FindAll(node => node.Type.Equals(NODE_TYPE.CLASS.ToString())
        //            //    && node.BindingName.Equals(symbol.BaseType.ToString().Replace('.', Path.DirectorySeparatorChar)));

        //            //    foreach (var baseClass in baseClasses)
        //            //    {
        //            //        Dependency dependency = new Dependency();
        //            //        dependency.Type = DEPENDENCY_TYPE.INHERIT.ToString();
        //            //        dependency.Caller = classNode.OriginName;
        //            //        dependency.Callee = baseClass.OriginName;
        //            //        dependencies.Add(dependency);
        //            //    }
        //            //}

        //            //// IMPLEMENT CLASS IMPLEMENT INTERFACE
        //            var interfaceDirectes = symbol.Interfaces; // get list interface relative direct with classNode
        //            //foreach (var interfaceDirect in interfaceDirectes)
        //            //{
        //            //    var interfaceNodes = tranferedNodes.FindAll(node =>
        //            //       node.Type.Equals(NODE_TYPE.INTERFACE.ToString())
        //            //       && node.BindingName.Equals(interfaceDirect.ToString().Replace('.', Path.DirectorySeparatorChar)));

        //            //    foreach (var interfaceNode in interfaceNodes)
        //            //    {
        //            //        Dependency dependency = new Dependency();
        //            //        dependency.Type = DEPENDENCY_TYPE.IMPLEMENT.ToString();
        //            //        dependency.Caller = classNode.OriginName;
        //            //        dependency.Callee = interfaceNode.OriginName;
        //            //        dependencies.Add(dependency);
        //            //    }
        //            //}

        //            // OVERRIDE
        //            // Get method in classNode
        //            var methodOfClass = tranferedNodes.FindAll(node =>
        //            node.Type.Equals(NODE_TYPE.METHOD.ToString())
        //            && node.BindingName.Remove(node.BindingName.Length - node.QualifiedName.Length - 1)
        //            .Equals(classNode.BindingName));

        //            var interfaceRelatives = symbol.AllInterfaces.ToList(); // get list interface relative with class
        //            var interfaceIndirectes = interfaceRelatives.FindAll(n => !interfaceDirectes.ToList().Contains(n)); // get list interface relative indirect with class
        //            List<Node> methodDirectNodes = new List<Node>();
        //            List<Node> methodIndirectNodes = new List<Node>();

        //            // Get method in interface direct
        //            foreach (var interfaceDirect in interfaceDirectes)
        //            {
        //                //  // 3864 debug
        //                //  interfaceDirect.ToString().Replace('.', Path.DirectorySeparatorChar);
        //                //  var test1 = tranferedNodes.FindAll(node =>
        //                //node.Type.Equals(NODE_TYPE.INTERFACE.ToString()));
        //                // test git
        //                // test git 2
        //                // test git 3

        //                var interfaceNodes = tranferedNodes.FindAll(node =>
        //              node.Type.Equals(NODE_TYPE.INTERFACE.ToString())
        //              && node.BindingName.Equals(interfaceDirect.ToString().Replace('.', Path.DirectorySeparatorChar)));

        //                foreach (var interfaceNode in interfaceNodes)
        //                {
        //                    var debug1 = tranferedNodes.FindAll(node => node.Type.Equals(NODE_TYPE.METHOD.ToString()));

        //                    methodDirectNodes.AddRange(tranferedNodes.FindAll(node => node.Type.Equals(NODE_TYPE.METHOD.ToString())
        //                    && node.BindingName.Remove(node.BindingName.Length - node.QualifiedName.Length - 1)
        //                    .Equals(interfaceNode.BindingName)));

        //                }
        //            }

        //            // Get method in interface indirect
        //            foreach (var interfaceIndirect in interfaceIndirectes)
        //            {
        //                var interfaceNodes = tranferedNodes.FindAll(node =>
        //              node.Type.Equals(NODE_TYPE.INTERFACE.ToString())
        //              && node.BindingName.Equals(interfaceIndirect.ToString().Replace('.', Path.DirectorySeparatorChar)));

        //                foreach (var interfaceNode in interfaceNodes)
        //                {
        //                    methodIndirectNodes.AddRange(tranferedNodes.FindAll(node => node.Type.Equals(NODE_TYPE.METHOD.ToString())
        //                    && node.BindingName.Remove(node.BindingName.Length - node.QualifiedName.Length - 1)
        //                    .Equals(interfaceNode.BindingName)));
        //                }
        //            }

        //            foreach (var methodNode in methodOfClass)
        //            {
        //                // OVERRIDE: CLASS - CLASS
        //                var methodSymbol = model.GetDeclaredSymbol(methodNode.SyntaxNode);
        //                var iMethodSymbol = methodSymbol is null ? null : (IMethodSymbol)methodSymbol;

        //                if (iMethodSymbol != null && iMethodSymbol.IsOverride)
        //                {
        //                    var superMethodNodes = root.childrens.FindAll(n => n.BindingName.Equals(iMethodSymbol.OverriddenMethod.ToString().Replace('.', Path.DirectorySeparatorChar)));

        //                    foreach (var superMethodNode in superMethodNodes)
        //                    {
        //                        Dependency dependency = new Dependency();
        //                        dependency.Type = DEPENDENCY_TYPE.OVERRIDE.ToString();
        //                        dependency.Caller = methodNode.OriginName;
        //                        dependency.Callee = superMethodNode.OriginName;
        //                        dependencies.Add(dependency);
        //                    }
        //                }
        //                else
        //                {
        //                    // OVERRIDE: CLASS - INTERFACE DIRECT
        //                    var methodDirectOverrides = methodDirectNodes.FindAll(n => n.QualifiedName.Equals(methodNode.QualifiedName));
        //                    if (methodDirectOverrides.Count > 0)
        //                    {
        //                        foreach (var item in methodDirectOverrides)
        //                        {
        //                            Dependency dependency = new Dependency();
        //                            dependency.Type = DEPENDENCY_TYPE.OVERRIDE.ToString();
        //                            dependency.Caller = methodNode.OriginName;
        //                            dependency.Callee = item.OriginName;
        //                            dependencies.Add(dependency);
        //                        }
        //                    }
        //                    // OVERRIDE: CLASS - INTERFACE INDIRECT
        //                    else
        //                    {
        //                        var methodIndirectOverrides = methodIndirectNodes.FindAll(n => n.QualifiedName.Equals(methodNode.QualifiedName));
        //                        foreach (var item in methodIndirectOverrides)
        //                        {
        //                            Dependency dependency = new Dependency();
        //                            dependency.Type = DEPENDENCY_TYPE.OVERRIDE.ToString();
        //                            dependency.Caller = methodNode.OriginName;
        //                            dependency.Callee = item.OriginName;
        //                            dependencies.Add(dependency);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return dependencies;
        //}
        #endregion
    }
}
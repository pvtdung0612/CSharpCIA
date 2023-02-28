// See https://aka.ms/new-console-template for more information

using CSharpCIA.CSharpCIA.API;
using CSharpCIA.CSharpCIA.Nodes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using CSharpCIA.CSharpCIA.Builders;

//string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest\\test2.cs";
//string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test5";
//string filePath = "‪E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test6\\CreateLibrary.dll";
//string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\CSharpCIA\\CSharpCIA";
//string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test1\\Program.cs";
string filePath = "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test10";

Parser parser = new Parser();
Tuple<List<Dependency>, RootNode> result = parser.Parse(filePath);
#region Print List Node
RootNode root = result.Item2 as RootNode;
Console.WriteLine("------------------------------------------------------------------");
Console.WriteLine($"\n\n\nCount Node: {root.childrens.Count + 1}");
Console.WriteLine($"\nroot:\nId:{root.Id} SimpleName:{root.SimpleName} Type:{root.Type} SourcePath:{root.SourcePath}");
foreach (var item in root.childrens)
{
    //Console.WriteLine($"node:\nId:{item.Id} SimpleName:{item.SimpleName} OriginName:{item.OriginName} Type:{item.Type} SourcePath:{item.SourcePath}");
    Console.WriteLine($"node:\n\n---OriginName:{item.OriginName}\n\nId:{item.Id} SimpleName:{item.SimpleName} Type:{item.Type} SourcePath:{item.SourcePath}");
    if (item is MethodNode) Console.WriteLine($"--QualifiedName:{((MethodNode)item).QualifiedName}");
}
#endregion

#region Print List Dependency
List<Dependency> dependencies = result.Item1;
Console.WriteLine($"\n\n\nCount Dependency: {dependencies.Count}");
foreach (var item in dependencies)
{
    Console.WriteLine($"\nType: {item.Type}");
    Console.WriteLine($"Caller: {item.Caller}");
    Console.WriteLine($"Callee: {item.Callee}");
}
#endregion

#region Use CIAExtension
//FindNodeById
Console.WriteLine("\n\nFind Node by Id");
Console.WriteLine(CIAExtension.FindNodeById(root, root.childrens.First<Node>().Id).OriginName);

// ExportDependencyToJson
CIAExtension.ExportDependencyToJson(dependencies);
CIAExtension.ExportRootToJson(root);
#endregion

//// Learn about Roslyn
//SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));
//CompilationUnitSyntax compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot();
//foreach (var item in compilationUnitSyntax.Members)
//{
//    if (item is GlobalStatementSyntax)
//    {
//        GlobalStatementSyntax gl = (GlobalStatementSyntax)item;
//        Console.WriteLine("------------global----------\n");
//        Console.WriteLine(gl.Statement);
//    }
//    else if (item is NamespaceDeclarationSyntax)
//    {
//        NamespaceDeclarationSyntax np = (NamespaceDeclarationSyntax)item;
//        Console.WriteLine("------------namespace----------\n");
//        Console.WriteLine();
//        foreach (var item2 in np.Members)
//        {
//            if (item2 is UsingDirectiveSyntax)
//            {
//            }
//        }
//    }
//}

// Test push to git



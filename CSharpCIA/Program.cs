// See https://aka.ms/new-console-template for more information

using CSharpCIA.CSharpCIA.API;
using CSharpCIA.CSharpCIA.Nodes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using CSharpCIA.CSharpCIA.Builders;
using CSharpCIA.CSharpCIA.Helpers;
using System.Collections.Generic;

//string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest\\test2.cs";
//string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test5";
//string filePath = "‪E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test6\\CreateLibrary.dll";
//string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\CSharpCIA\\CSharpCIA";
//string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test1\\Program.cs";
string filePath1 = "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test11\\Test11_ver1";
string filePath2 = "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test11\\Test11_ver2";

Parser parser = new Parser();
Tuple<List<Node>, List<Dependency>> project1 = parser.Parse(filePath1);
Tuple<List<Node>, List<Dependency>> project2 = parser.Parse(filePath2);


#region Print List Node
Console.WriteLine("------------------------------------------------------------------");
Console.WriteLine($"\n\n\nCount Node: {project1.Item1.Count + 1}");
foreach (var item in project1.Item1)
{
    //Console.WriteLine($"node:\nId:{item.Id} SimpleName:{item.SimpleName} OriginName:{item.OriginName} Type:{item.Type} SourcePath:{item.SourcePath}");
    Console.WriteLine($"node:\n\n---BindingName:{item.BindingName}\nId:{item.Id} \nSimpleName:{item.SimpleName} \nType:{item.Type} \nOriginName:{item.OriginName} \nSourcePath:{item.SourcePath}");
    if (item is MethodNode) Console.WriteLine($"--QualifiedName:{((MethodNode)item).QualifiedName}");
}
#endregion

#region Print List Dependency
Console.WriteLine($"\n\n\nCount Dependency: {project1.Item2.Count}");
foreach (var item in project1.Item2)
{
    Console.WriteLine($"\nType: {item.Type}");
    Console.WriteLine($"Caller: {item.Caller}");
    Console.WriteLine($"Callee: {item.Callee}");
}
#endregion

#region Use CIAExtension
////FindNodeById
//Console.WriteLine("\n\nFind Node by Id");
//Console.WriteLine(Extension.FindNodeById(root1, ((List<Node>) project1.Item2).First<Node>().Id).OriginName);

// Export
// Root
Console.WriteLine($"\n\nExportNodesToJson return: {Extension.ExportNodesToJson(project2.Item1)}");
// Dependency
Console.WriteLine($"\n\nExportDependencyToJson return: {Extension.ExportDependencyToJson(project2.Item2)}");
// Change
// Change: tìm các thay đổi của ver2 so với ver1
Dictionary<string, string> nodeChanges = AnalyzerImpact.AnalyzerChange(project1.Item1, project2.Item1);
Console.WriteLine($"\n\nExportChangeToJson return: {Extension.ExportChangeToJson(nodeChanges)}");
// Impact
// Impact: ảnh hưởng của sự thay đổi lên ver1
//Dictionary<string, ulong> impactsVer2 = AnalyzerImpact.ChangeImpactAnalysis(nodeChanges, project1.Item1);
//Console.WriteLine($"\n\nExportImpactToJson return: {Extension.ExportImpactToJson(impactsVer2)}");
// Impact: ảnh hưởng của sự thay đổi lên ver2
Dictionary<string, ulong> impactsVer2 = AnalyzerImpact.ChangeImpactAnalysis(nodeChanges, project2.Item2);
Console.WriteLine($"\n\nExportImpactToJson return: {Extension.ExportImpactToJson(impactsVer2)}");
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



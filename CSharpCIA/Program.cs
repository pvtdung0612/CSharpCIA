// See https://aka.ms/new-console-template for more information

using CSharpCIA.CSharpCIA.API;
using CSharpCIA.CSharpCIA.Nodes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using CSharpCIA.CSharpCIA.Builders;
using CSharpCIA.CSharpCIA.Helpers;

//string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest\\test2.cs";
//string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test5";
//string filePath = "‪E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test6\\CreateLibrary.dll";
//string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\CSharpCIA\\CSharpCIA";
//string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test1\\Program.cs";
string filePath1 = "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test11\\Test11_ver1";
string filePath2 = "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test11\\Test11_ver2";

Parser parser = new Parser();
Tuple<List<Dependency>, RootNode> project1 = parser.Parse(filePath1);
Tuple<List<Dependency>, RootNode> project2 = parser.Parse(filePath2);


#region Print List Node
RootNode root1 = project1.Item2 as RootNode;
Console.WriteLine("------------------------------------------------------------------");
Console.WriteLine($"\n\n\nCount Node: {root1.childrens.Count + 1}");
Console.WriteLine($"\nroot:\nId:{root1.Id} SimpleName:{root1.SimpleName} Type:{root1.Type} SourcePath:{root1.SourcePath}");
foreach (var item in root1.childrens)
{
    //Console.WriteLine($"node:\nId:{item.Id} SimpleName:{item.SimpleName} OriginName:{item.OriginName} Type:{item.Type} SourcePath:{item.SourcePath}");
    Console.WriteLine($"node:\n\n---BindingName:{item.BindingName}\nId:{item.Id} \nSimpleName:{item.SimpleName} \nType:{item.Type} \nOriginName:{item.OriginName} \nSourcePath:{item.SourcePath}");
    if (item is MethodNode) Console.WriteLine($"--QualifiedName:{((MethodNode)item).QualifiedName}");
}
#endregion

#region Print List Dependency
List<Dependency> dependencies1 = project1.Item1;
Console.WriteLine($"\n\n\nCount Dependency: {dependencies1.Count}");
foreach (var item in dependencies1)
{
    Console.WriteLine($"\nType: {item.Type}");
    Console.WriteLine($"Caller: {item.Caller}");
    Console.WriteLine($"Callee: {item.Callee}");
}
#endregion

#region Use CIAExtension
//FindNodeById
Console.WriteLine("\n\nFind Node by Id");
Console.WriteLine(Extension.FindNodeById(root1, root1.childrens.First<Node>().Id).OriginName);

// Export
// Root
Console.WriteLine($"\n\nExportRootToJson return: {Extension.ExportRootToJson(project2.Item2)}");
// Dependency
Console.WriteLine($"\n\nExportDependencyToJson return: {Extension.ExportDependencyToJson(project2.Item1)}");
// Change
// Change: tìm các thay đổi của ver2 so với ver1
Dictionary<string, string> nodeChanges = AnalyzerImpact.AnalyzerChange(((RootNode)project1.Item2).childrens, ((RootNode)project2.Item2).childrens);
Console.WriteLine($"\n\nExportChangeToJson return: {Extension.ExportChangeToJson(nodeChanges)}");
// Impact
// Impact: ảnh hưởng của sự thay đổi lên ver1
//Dictionary<string, ulong> impactsVer2 = AnalyzerImpact.ChangeImpactAnalysis(nodeChanges, project1.Item1);
//Console.WriteLine($"\n\nExportImpactToJson return: {Extension.ExportImpactToJson(impactsVer2)}");
// Impact: ảnh hưởng của sự thay đổi lên ver2
Dictionary<string, ulong> impactsVer2 = AnalyzerImpact.ChangeImpactAnalysis(nodeChanges, project2.Item1);
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



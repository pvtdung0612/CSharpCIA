// See https://aka.ms/new-console-template for more information

using CSharpCIA.CSharpCIA.API;
using CSharpCIA.CSharpCIA.Nodes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using CSharpCIA.DataTest;
using Test;
using Test.Test.test1;

//string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest\\test2.cs";
string filePath = "‪E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test6\\CreateLibrary.dl";


Parser parser = new Parser();
RootNode root = parser.ParserFile(filePath);

Console.WriteLine("------------------------------------------------------------------");
Console.WriteLine($"root:\nId:{root.Id} Name:{root.Name} Type:{root.Type} SourcePath:{root.SourcePath}");
foreach (var item in root.childrens)
{
    Console.WriteLine($"node:\nId:{item.Id} Name:{item.Name} Type:{item.Type} SourcePath:{item.SourcePath}");
    if (item is MethodNode) Console.WriteLine($"--UniqueName:{((MethodNode)item).UniqueName}");
}


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

// Test

test1.info();


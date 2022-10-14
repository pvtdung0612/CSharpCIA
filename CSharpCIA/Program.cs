// See https://aka.ms/new-console-template for more information

using CSharpCIA.CSharpCIA.API;
using CSharpCIA.CSharpCIA.Nodes;

Console.WriteLine("Hello, World!");

Parser parser = new Parser();
RootNode root = parser.ParserFile("F:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\CSharpCIA\\CSharpCIA\\DataTest\\Parser.cs");

Console.WriteLine("------------------------------------------------------------------");
Console.WriteLine($"root:\nId:{root.Id} Name:{root.Name} Type:{root.Type} SourcePath:{root.SourcePath}");
foreach (var item in root.childrens)
{
    Console.WriteLine($"node:\nId:{item.Id} Name:{item.Name} Type:{item.Type} SourcePath:{item.SourcePath}");
    if (item is MethodNode) Console.WriteLine($"--UniqueName:{((MethodNode)item).UniqueName}");
}
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

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {

            try
            {
                MenuConsole();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); ;
            }


            #region Test
            ////string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest\\test2.cs";
            ////string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test5";
            ////string filePath = "‪E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test6\\CreateLibrary.dll";
            ////string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\CSharpCIA\\CSharpCIA";
            ////string filePath = "E:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test1\\Program.cs";
            //string filePath1 = "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test11\\Test11_ver1";
            //string filePath2 = "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\DataTest2\\Test11\\Test11_ver2";

            //Parser parser = new Parser();
            //Tuple<List<Node>, List<Dependency>> project1 = parser.Parse(filePath1);
            //Tuple<List<Node>, List<Dependency>> project2 = parser.Parse(filePath2);


            //#region Print List Node
            //Console.WriteLine("------------------------------------------------------------------");
            //Console.WriteLine($"\n\n\nCount Node: {project1.Item1.Count + 1}");
            //foreach (var item in project1.Item1)
            //{
            //    //Console.WriteLine($"node:\nId:{item.Id} SimpleName:{item.SimpleName} OriginName:{item.OriginName} Type:{item.Type} SourcePath:{item.SourcePath}");
            //    Console.WriteLine($"node:\n\n---BindingName:{item.BindingName}\nId:{item.Id} \nSimpleName:{item.SimpleName} \nType:{item.Type} \nOriginName:{item.OriginName} \nSourcePath:{item.SourcePath}");
            //    if (item is MethodNode) Console.WriteLine($"--QualifiedName:{((MethodNode)item).QualifiedName}");
            //}
            //#endregion

            //#region Print List Dependency
            //Console.WriteLine($"\n\n\nCount Dependency: {project1.Item2.Count}");
            //foreach (var item in project1.Item2)
            //{
            //    Console.WriteLine($"\nType: {item.Type}");
            //    Console.WriteLine($"Caller: {item.Caller}");
            //    Console.WriteLine($"Callee: {item.Callee}");
            //}
            //#endregion

            //#region Use CIAExtension Export
            //////FindNodeById
            ////Console.WriteLine("\n\nFind Node by Id");
            ////Console.WriteLine(Extension.FindNodeById(root1, ((List<Node>) project1.Item2).First<Node>().Id).OriginName);

            //// Export
            //// Dependency
            //Console.WriteLine($"\nExportDependencyToJson return: {Extension.ExportDependencyToJson(project2.Item2, "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\ResultDataTest\\Test11\\ExportDependencyToJson.json")}");
            //// Change
            //// Change: tìm các thay đổi của ver2 so với ver1
            //Dictionary<string, string> nodeChanges = AnalyzerImpact.AnalyzerChange(project1.Item1, project2.Item1);
            //Console.WriteLine($"\nExportChangeToJson return: {Extension.ExportChangeToJson(nodeChanges, "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\ResultDataTest\\Test11\\ExportChangeToJson.json")}");
            //// Impact
            //// Impact: ảnh hưởng của sự thay đổi lên ver1
            ////Dictionary<string, ulong> impactsVer2 = AnalyzerImpact.ChangeImpactAnalysis(nodeChanges, project1.Item1);
            ////Console.WriteLine($"\n\nExportImpactToJson return: {Extension.ExportImpactToJson(impactsVer2)}");
            //// Impact: ảnh hưởng của sự thay đổi lên ver2
            //Dictionary<string, ulong> impactsVer2 = AnalyzerImpact.ChangeImpactAnalysis(project2.Item1, nodeChanges, project2.Item2);
            //Console.WriteLine($"\nExportImpactToJson return: {Extension.ExportImpactToJson(impactsVer2, "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\ResultDataTest\\Test11\\ExportImpactToJson.json")}");

            //// Nodes
            //Console.WriteLine($"\nExportNodesToJson return: {Extension.ExportNodesToJson(project2.Item1, "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\ResultDataTest\\Test11\\ExportNodesToJson.json")}");
            //#endregion

            //#region Use CIAExtension Import 
            //List<Node> importNodes = Extension.ImportNodesFromJson("D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\ResultDataTest\\Test11\\ExportNodesToJson.json");
            //var check = Extension.ExportNodesToJson(importNodes, "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\ResultDataTest\\Test11\\TestImport\\Nodes.json");

            //Console.WriteLine($"\nExport Node from import: {check}");

            //var importDependencies = Extension.ImportDependencyFromJson("D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\ResultDataTest\\Test11\\ExportDependencyToJson.json");
            //check = Extension.ExportDependencyToJson(importDependencies, "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\ResultDataTest\\Test11\\TestImport\\Dependency.json");
            //Console.WriteLine($"\nExport dependency from import: {check}");
            //#endregion

            //#region Use CIAExtension ExportJsonToGexf
            //check = Extension.ExportJsonToGexf("D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\ResultDataTest\\Test11\\TestImport\\Nodes.json",
            //    "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\ResultDataTest\\Test11\\TestImport\\Dependency.json",
            //    "D:\\dung\\UET-VNU\\Lab\\Work\\CSharpCIA\\ResultDataTest\\Test11\\TestImport\\test11.gexf");
            //Console.WriteLine($"\nExport json to gexf: {check}");
            //#endregion

            ////// Learn about Roslyn
            ////SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));
            ////CompilationUnitSyntax compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot();
            ////foreach (var item in compilationUnitSyntax.Members)
            ////{
            ////    if (item is GlobalStatementSyntax)
            ////    {
            ////        GlobalStatementSyntax gl = (GlobalStatementSyntax)item;
            ////        Console.WriteLine("------------global----------\n");
            ////        Console.WriteLine(gl.Statement);
            ////    }
            ////    else if (item is NamespaceDeclarationSyntax)
            ////    {
            ////        NamespaceDeclarationSyntax np = (NamespaceDeclarationSyntax)item;
            ////        Console.WriteLine("------------namespace----------\n");
            ////        Console.WriteLine();
            ////        foreach (var item2 in np.Members)
            ////        {
            ////            if (item2 is UsingDirectiveSyntax)
            ////            {
            ////            }
            ////        }
            ////    }
            ////}

            //// Test push to git
            #endregion
        }

        public static void MenuConsole()
        {

            Console.WriteLine("********************************************");
            Console.WriteLine("**                                        **");
            Console.WriteLine("**          Welcome to CSharpCIA          **");
            Console.WriteLine("**                                        **");
            Console.WriteLine("********************************************");

            // Init check for quit loop do while
            bool isQuit = true;
            // Create dictionary for save project import
            Dictionary<string, Tuple<List<Node>, List<Dependency>>> inputDictionaryProject = new Dictionary<string, Tuple<List<Node>, List<Dependency>>>();

            do
            {
                Console.WriteLine("\n\n\n\nPlease choose your number option!");
                Console.WriteLine("Option:");
                Console.WriteLine("1. Parse project tree of source code C# (Nodes, Dependencies) from path");
                Console.WriteLine("2. Analyzer change's impact of path's source code C# (Changes, Impactes) from path");
                Console.WriteLine("3. Export json path to gexf");
                Console.WriteLine("4. Import CSharpCIA's json file and Save");
                Console.WriteLine("5. Show list project saved");
                Console.WriteLine("6. Export project to gexf");
                Console.WriteLine("7. QUIT");

                int inputNumperOption = 0;
                try
                {
                    inputNumperOption = Convert.ToInt32(Console.ReadLine().Trim());
                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                // 1. Parse project tree of source code C# (Nodes, Dependencies) from path
                if (inputNumperOption == 1)
                {
                    // Get information
                    Console.WriteLine("Enter your path of source code C#: ");
                    string inputPathSourceCode = Convert.ToString(Console.ReadLine().Trim());

                    Console.WriteLine("Enter your path of parser result: ");
                    Console.WriteLine("Nodes: ");
                    string outputPathNodes = Convert.ToString(Console.ReadLine().Trim());
                    Console.WriteLine("Dependencies: ");
                    string outputPathDependencies = Convert.ToString(Console.ReadLine().Trim());

                    // Parsing
                    Console.WriteLine("Parsing...");
                    Parser parser1 = new Parser();
                    var inputProject = parser1.Parse(inputPathSourceCode);

                    bool inputCheck = true;
                    inputCheck = Extension.ExportNodesToJson(inputProject.Item1, outputPathNodes);
                    Console.WriteLine($"Export Nodes to Json: {inputCheck}");
                    inputCheck = Extension.ExportDependencyToJson(inputProject.Item2, outputPathDependencies);
                    Console.WriteLine($"Export Dependencies to Json: {inputCheck}");
                }
                // 2. Analyzer change's impact of path's source code C# (Changes, Impactes) from path
                else if (inputNumperOption == 2)
                {
                    // Get information
                    Console.WriteLine("Enter your path of source code ver1 C#: ");
                    string inputPathSourceCodeVer1 = Convert.ToString(Console.ReadLine().Trim());
                    Console.WriteLine("Enter your path of source code ver2 C#: ");
                    string inputPathSourceCodeVer2 = Convert.ToString(Console.ReadLine().Trim());

                    Console.WriteLine("Enter your path of parser result: ");
                    Console.WriteLine("Changes: ");
                    string outputPathChanges = Convert.ToString(Console.ReadLine().Trim());
                    Console.WriteLine("Impactes: ");
                    string outputPathImpactes = Convert.ToString(Console.ReadLine().Trim());

                    // Parsing
                    Console.WriteLine("Analyzer...");
                    Parser parser1 = new Parser();
                    var inputProjectVer1 = parser1.Parse(inputPathSourceCodeVer1);
                    var inputProjectVer2 = parser1.Parse(inputPathSourceCodeVer2);
                    var inputChange = AnalyzerImpact.AnalyzerChange(inputProjectVer1.Item1, inputProjectVer2.Item1);

                    bool inputCheck = true;
                    inputCheck = Extension.ExportChangeToJson(inputChange, outputPathChanges);
                    Console.WriteLine($"Export Changed to Json: {inputCheck}");
                    inputCheck = Extension.ExportImpactToJson(AnalyzerImpact.ChangeImpactAnalysis(inputProjectVer2.Item1, inputChange, inputProjectVer2.Item2), outputPathImpactes);
                    Console.WriteLine($"Export Impactes of ver2 to Json: {inputCheck}");
                }
                // 3. Export json path to gexf
                else if (inputNumperOption == 3)
                {
                    // Get information
                    Console.WriteLine("Enter your json path of nodes: ");
                    string inputPathNode = Convert.ToString(Console.ReadLine().Trim());
                    Console.WriteLine("Enter your json path of dependencies: ");
                    string inputPathDependency = Convert.ToString(Console.ReadLine().Trim());

                    Console.WriteLine("Enter your path of parser result: ");
                    Console.WriteLine("Gexf: ");
                    string outputPathGexf = Convert.ToString(Console.ReadLine().Trim());

                    // Exporting
                    Console.WriteLine("Exporting...");

                    bool inputCheck = true;
                    inputCheck = Extension.ExportJsonToGexf(inputPathNode, inputPathDependency, outputPathGexf);
                    Console.WriteLine($"Export json to gexf: {inputCheck}");
                }
                // 4. Import CSharpCIA's json file and Save:
                else if (inputNumperOption == 4)
                {
                    // Get information
                    Console.WriteLine("Enter your name of project: ");
                    var inputKeyNameProject = Convert.ToString(Console.ReadLine().Trim());
                    if (inputDictionaryProject.ContainsKey(inputKeyNameProject))
                    {
                        Console.WriteLine("Name is exits!");
                    }
                    else
                    {
                        Console.WriteLine("Enter your json path of nodes: ");
                        string inputPathNode = Convert.ToString(Console.ReadLine().Trim());
                        Console.WriteLine("Enter your json path of dependencies: ");
                        string inputPathDependency = Convert.ToString(Console.ReadLine().Trim());

                        var inputNode = Extension.ImportNodesFromJson(inputPathNode);
                        var inputDependency = Extension.ImportDependencyFromJson(inputPathDependency);

                        Tuple<List<Node>, List<Dependency>> inputProject = Tuple.Create(inputNode, inputDependency);
                        if (inputProject.Item1 is not null && inputProject.Item2 is not null)
                        {
                            inputDictionaryProject.Add(inputKeyNameProject, inputProject);
                        }
                        else
                        {
                            Console.WriteLine("Your project is null so i can't save it!");
                        }
                    }
                }
                // 5. Show list project saved
                else if (inputNumperOption == 5)
                {
                    Console.WriteLine($"Have {inputDictionaryProject.Count} saved project: ");

                    foreach (var item in inputDictionaryProject)
                    {
                        Console.WriteLine($"Name: {item.Key}");
                        Console.WriteLine($"Project: {item.Value}");
                        Console.WriteLine($"Count: {item.Value.Item1.Count} node, {item.Value.Item2.Count} edge");
                    }
                }
                // 6. Export project to gexf
                else if (inputNumperOption == 6)
                {
                    // Get information
                    Console.WriteLine("Enter your name of saved project: ");
                    string inputNameproject = Convert.ToString(Console.ReadLine().Trim());
                    if (inputDictionaryProject.ContainsKey(inputNameproject))
                    {
                        Console.WriteLine("Enter your path of parser result: ");
                        Console.WriteLine("Gexf: ");
                        string outputPathGexfFormNodeAndDependency = Convert.ToString(Console.ReadLine().Trim());

                        var inputProject = inputDictionaryProject[inputNameproject];

                        bool inputCheck = true;
                        inputCheck = Extension.ExportNodeAndDependencyToGexf(inputProject.Item1, inputProject.Item2, outputPathGexfFormNodeAndDependency);
                        Console.WriteLine($"Export project to gexf: {inputCheck}");
                    }
                    else
                    {
                        Console.WriteLine("Name is not exits!");
                    }
                }


                Console.WriteLine("\nDo you want quit? (y/n)");
                isQuit = Convert.ToString(Console.ReadLine().Trim()) == "y" ? true : false;
            } while (!isQuit);
        }
    }
}

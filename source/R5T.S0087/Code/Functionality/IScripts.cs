using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R5T.F0000.Extensions;
using R5T.T0132;
using R5T.T0162;
using R5T.T0172;
using R5T.T0179.Extensions;
using R5T.T0181;
using R5T.T0212.F000;


namespace R5T.S0087
{
    [FunctionalityMarker]
    public partial interface IScripts : IFunctionalityMarker
    {
        /// <summary>
        /// For a dotnet pack, verify that all XML files in its directory are documentation XML files.
        /// </summary>
        public async Task Verify_DotnetPackDirectoryXmlFilesAreDocumentationFiles()
        {
            /// Inputs.
            var dotnetPackNames =
                // Test only the .Net 6.0 packs.
                Instances.DotnetPackNameSets.ForNet6;
            var targetFramework =
                Instances.TargetFrameworkMonikers.NET_6
                ;
            var outputFilePath = Instances.FilePaths.OutputTextFilePath;


            /// Run.
            var allXmlFilePaths = new List<IXmlFilePath>();
            var xmlFilesThatAreNotDocumentationFiles = new List<IXmlFilePath>();

            foreach (var dotnetPackName in dotnetPackNames)
            {
                var dotnetPackDirectoryPath = Instances.DotnetPackPathOperator.Get_DotnetPackDirectoryPath(
                    dotnetPackName,
                    targetFramework);

                var xmlFilePaths = Instances.FileSystemOperator.Enumerate_ChildXmlFilePaths(dotnetPackDirectoryPath);

                foreach (var xmlFilePath in xmlFilePaths)
                {
                    allXmlFilePaths.Add(xmlFilePath);

                    Console.WriteLine($"Analyzing path...\n\t{xmlFilePath}");

                    var isDocumentationXmlFile = await Instances.DocumentationFileOperator.Is_DocumentationXmlFile(xmlFilePath);
                    if(!isDocumentationXmlFile)
                    {
                        xmlFilesThatAreNotDocumentationFiles.Add(xmlFilePath);
                    }
                }
            }

            var lines = Instances.EnumerableOperator.Empty<string>()
                .AppendIf(xmlFilesThatAreNotDocumentationFiles.Any(),
                    xmlFilesThatAreNotDocumentationFiles.Get_Values()
                )
                .AppendIf(xmlFilesThatAreNotDocumentationFiles.None(), Instances.EnumerableOperator.From("<In all .NET 6.0 dotnet pack directories, there were no XML files that were not documentation files.>\n")
                    .Append($"Analyzed paths (count: {allXmlFilePaths.Count}):")
                    .Append(allXmlFilePaths
                        .Get_Values()
                        .OrderAlphabetically()
                        .Select(x => $"\t{x}")
                    )
                );

            Instances.NotepadPlusPlusOperator.WriteLinesAndOpen(
                outputFilePath.Value,
                lines);
        }

        /// <summary>
        /// For a dotnet pack, get all member elements by identity name.
        /// (Note: there might be multiple member elements for a single identity name, i.e. duplicate member identity names.)
        /// </summary>
        public async Task Ingest_DotnetPackMemberDocumentations()
        {
            /// Inputs.
            var dotnetPackName =
                //Instances.DotnetPackNames.Microsoft_NETCore_App_Ref
                Instances.DotnetPackNames.NETStandard_Library_Ref
                ;
            var targetFramework =
                //Instances.TargetFrameworkMonikers.NET_6
                Instances.TargetFrameworkMonikers.NET_Standard2_1
                ;

            var humanOutputFilePath = Instances.FilePaths.HumanOutputTextFilePath;
            var logFilePath = Instances.FilePaths.LogFilePath;
            // For identities that appear multiple times, but with different values.
            var outputFilePath = Instances.FilePaths.OutputTextFilePath;
            // For all identities.
            var outputFilePath2 = Instances.FilePaths.OutputTextFilePath_Secondary;
            // For identities that appear multiple times, even if their values are identical.
            var outputFilePath3 = Instances.FilePaths.OutputTextFilePath_Tertiary;


            /// Run.
            var result = new Dictionary<IIdentityName, List<T0212.F000.MemberDocumentation>>();
            var identitiesAndDocumentationXmlFilePaths = new Dictionary<IIdentityName, List<IDocumentationXmlFilePath>>();

            await Instances.TextOutputOperator.In_TextOutputContext(
                humanOutputFilePath,
                nameof(Ingest_DotnetPackMemberDocumentations),
                logFilePath,
                async textOutput =>
                {
                    var documentationXmlFilePaths = Instances.DotnetPackPathOperator.Get_DocumentationXmlFilePaths(
                        dotnetPackName,
                        targetFramework,
                        textOutput);

                    foreach (var documentationXmlFilePath in documentationXmlFilePaths)
                    {
                        textOutput.WriteInformation("Processing documentation XML file...\n\t{0}", documentationXmlFilePath);

                        var documentationXmlFileTarget = Instances.DocumentationTargetOperator.Get_DocumentationXmlFileTarget(documentationXmlFilePath);

                        var memberElements = await Instances.DocumentationFileOperator.Get_MemberElements_Raw(
                            documentationXmlFilePath);

                        foreach (var memberElement in memberElements)
                        {
                            var identityName = Instances.MemberElementOperator.Get_IdentityName(memberElement);

                            var memberDocumentation = new T0212.F000.MemberDocumentation
                            {
                                DocumentationTarget = documentationXmlFileTarget,
                                IdentityName = identityName,
                                MemberElement = memberElement,
                            };

                            // Only add if the identity name does not exist, or if the member element text is different.
                            var keyAlreadyExists = result.ContainsKey(identityName);
                            if(keyAlreadyExists)
                            {
                                var list = result[identityName];

                                var append = true;

                                var memberElementText = memberDocumentation.MemberElement.ToString();

                                foreach (var item in list)
                                {
                                    var itemText = item.MemberElement.ToString();

                                    if(memberElementText == itemText)
                                    {
                                        append = false;
                                        break;
                                    }
                                }

                                if(append)
                                {
                                    list.Add(memberDocumentation);
                                }
                            }
                            else
                            {
                                var list = new List<T0212.F000.MemberDocumentation>
                                {
                                    memberDocumentation
                                };

                                result.Add(
                                    identityName,
                                    list);
                            }

                            identitiesAndDocumentationXmlFilePaths.Add_Value(
                                identityName,
                                documentationXmlFilePath);
                        }
                    }

                    // Find those identity names with multiple member documentations across one or more dotnet pack documentation files.
                    var duplicateIdentities = identitiesAndDocumentationXmlFilePaths
                        .Where(x => x.Value.Count > 1)
                        .Now();

                    {
                        if (duplicateIdentities.Any())
                        {
                            var lines = Instances.EnumerableOperator_F0000.From($"Duplicate Identities, count: {duplicateIdentities.Length}")
                                .Append(duplicateIdentities
                                    .OrderAlphabetically(x => x.Key.Value)
                                    .SelectMany(x =>
                                    {
                                        return Instances.EnumerableOperator_F0000.From($"{x.Key.Value}:")
                                            .Append(x.Value
                                                .OrderAlphabetically(x => x.Value)
                                                .Select(x => $"\t{x.Value}"))
                                            ;
                                    })
                                );

                            Instances.FileOperator.Write_Lines_Synchronous(
                                outputFilePath3,
                                lines);
                        }
                        else
                        {
                            Instances.FileOperator.Write_Lines_Synchronous(
                                outputFilePath3,
                                "No duplicates identities.");
                        }
                    }

                    var ofInterest = result
                        .Where(x => x.Value.Count > 1)
                        .Now();

                    if(ofInterest.Any())
                    {
                        var lines = Instances.EnumerableOperator_F0000.From($"Duplicate identities with different documentation values, count: {ofInterest.Length}")
                            .Append(ofInterest
                                .OrderAlphabetically(x => x.Key.Value)
                                .SelectMany(x =>
                                {
                                    var lines = Instances.MemberDocumentationOperator_T0212_F000.Describe(x.Value);
                                    return lines;
                                })
                            );

                        Instances.FileOperator.Write_Lines_Synchronous(
                            outputFilePath,
                            lines);
                    }
                    else
                    {
                        Instances.FileOperator.Write_Text_Synchronous(
                            outputFilePath,
                            "No duplicates identities with different documentation values.");
                    }

                    {
                        var lines = result.Keys
                            .Select(x => x.Value)
                            .OrderAlphabetically()
                            ;

                        Instances.FileOperator.Write_Lines_Synchronous(
                            outputFilePath2,
                            lines);
                    }
                });

            Instances.NotepadPlusPlusOperator.Open(
                humanOutputFilePath,
                logFilePath,
                outputFilePath,
                outputFilePath2,
                outputFilePath3);
        }

        /// <summary>
        /// Opens the dotnet packs directory in an Explorer window.
        /// </summary>
        public void Open_DotnetPacksDirectory_InExplorer()
        {
            Instances.WindowsExplorerOperator.Open(
                Instances.DotnetPacksDirectoryPaths.Windows);
        }
    }
}

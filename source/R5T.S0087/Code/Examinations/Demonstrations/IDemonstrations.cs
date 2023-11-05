using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R5T.L0062.T000;
using R5T.T0141;


namespace R5T.S0087
{
    [DemonstrationsMarker]
    public partial interface IDemonstrations : IDemonstrationsMarker
    {
        public async Task Load_DotnetPackDocumentationFiles()
        {
            /// Inputs.
            var targetFramework = Instances.TargetFrameworkMonikers.NET_6;
            var outputFilePath = Instances.FilePaths.OutputTextFilePath;
            var errorsFilePath = Instances.FilePaths.OutputErrorsTextFilePath;


            /// Run.
            var documentationFilePaths = Instances.DotnetPackPathsOperator.Get_DotnetPackDocumentationFilePaths(targetFramework);

            var results = new Dictionary<IIdentityString, MemberDocumentation>();
            var duplicates = new Dictionary<IIdentityString, List<MemberDocumentation>>();

            var index = 1;
            var documentationFilePathsCount = documentationFilePaths.Length;
            foreach (var documentationFilePath in documentationFilePaths)
            {
                Console.WriteLine($"Processing documentation file ({index++}/{documentationFilePathsCount})...\n{documentationFilePath}");

                var documentationXmlFileDocumentationTarget = Instances.DocumentationTargetOperator.Get_DocumentationXmlFileTarget(documentationFilePath);

                var rawMemberElements = await Instances.DocumentationFileOperator.Get_MemberElements_Raw(documentationFilePath);

                foreach (var memberElement in rawMemberElements)
                {
                    // Reformat the member element.
                    Instances.MemberElementOperator.RemoveExtraTextLineEndings(memberElement);

                    var memberDocumentation = Instances.MemberDocumentationOperator.Get_MemberDocumentation(
                        memberElement,
                        documentationXmlFileDocumentationTarget);

                    var added = results.TryAdd(
                        memberDocumentation.IdentityString,
                        memberDocumentation);
                    if(!added)
                    {
                        // If this is the first duplicate, add the initial member documentation.
                        var isFirstDuplicate = !duplicates.ContainsKey(memberDocumentation.IdentityString);
                        if(isFirstDuplicate)
                        {
                            var initialMemberDocumentation = results[memberDocumentation.IdentityString];

                            Instances.DictionaryOperator.Add_Value(
                                duplicates,
                                initialMemberDocumentation.IdentityString,
                                initialMemberDocumentation);
                        }

                        // Now add the duplicate member documentation.
                        Instances.DictionaryOperator.Add_Value(
                            duplicates,
                            memberDocumentation.IdentityString,
                            memberDocumentation);
                    }
                }
            }

            // Describe results and duplicates.
            var lines = Instances.EnumerableOperator.From($"Dotnet pack ('{targetFramework}') member documentations:\n(count: {results.Count})\n")
                .Append(results.Values
                    .Select(Instances.MemberDocumentationOperator.Describe)
                );

            Instances.FileOperator.Write_Lines_Synchronous(
                outputFilePath,
                lines);

            lines = Instances.EnumerableOperator.From($"Duplicate dotnet pack ('{targetFramework}') member documentations:\n(count: {duplicates.Count})\n")
                .Append(duplicates
                    .SelectMany(duplicate => Instances.EnumerableOperator.From($"{duplicate.Key}:")
                        .Append(duplicate.Value
                            .Select(Instances.MemberDocumentationOperator.Describe)
                        )
                        .Append("")
                    )
                );

            Instances.FileOperator.Write_Lines_Synchronous(
                errorsFilePath,
                lines);

            Instances.NotepadPlusPlusOperator.Open(
                outputFilePath,
                errorsFilePath);
        }

        public void List_DuplicateDotnetPackAssemblyFilePaths()
        {
            /// Inputs.
            var targetFramework = Instances.TargetFrameworkMonikers.NET_6;
            var outputFilePath = Instances.FilePaths.OutputTextFilePath;


            /// Run.
            var runtimeAssemblyFilePaths = Instances.DotnetPackPathsOperator.Get_DotnetPackAssemblyFilePaths(targetFramework);

            var distinctRuntimeAssemblyFilePaths = Instances.AssemblyFilePathOperator.Get_Distinct_KeepFirst(
                runtimeAssemblyFilePaths,
                out var duplicateAssemblyFilePathsByAssemblyFileName);

            var lines = Instances.EnumerableOperator.Empty<string>()
                .AppendIf(duplicateAssemblyFilePathsByAssemblyFileName.Any(), Instances.EnumerableOperator.From($"Duplicate '{targetFramework}' dotnet pack assembly file paths:")
                    .Append(duplicateAssemblyFilePathsByAssemblyFileName
                        .SelectMany(pair => Instances.EnumerableOperator.From($"{pair.Key}:")
                            .Append(pair.Value
                                .Select(assemblyFilePath => $"\t{assemblyFilePath}")
                            )
                        )
                    )
                )
                .AppendIf(duplicateAssemblyFilePathsByAssemblyFileName.None(), $"<No duplicate '{targetFramework}' dotnet pack assembly file paths.>")
                ;

            Instances.NotepadPlusPlusOperator.WriteLinesAndOpen(
                outputFilePath.Value,
                lines);
        }

        public void Get_DocumentationFilePaths()
        {
            /// Inputs.
            var dotnetPackName = Instances.DotnetPackNames.Microsoft_NETCore_App_Ref;
            var targetFramework = Instances.TargetFrameworkMonikers.NET_6;


            /// Run.
            Instances.TextOutputOperator._Base.In_TextOutputContext_Console(
                textOutput =>
                {
                    var documentationXmlFilePaths = Instances.DotnetPackPathOperator.Get_DocumentationXmlFilePaths(
                        dotnetPackName,
                        targetFramework,
                        textOutput);

                    foreach (var filePath in documentationXmlFilePaths)
                    {
                        Console.WriteLine(filePath);
                    }
                });
        }

        public void Get_DotnetPackDirectoryPath()
        {
            /// Inputs.
            var dotnetPackName = Instances.DotnetPackNames.Microsoft_NETCore_App_Ref;
            var targetFramework = Instances.TargetFrameworkMonikers.NET_6;


            /// Run.
            Instances.TextOutputOperator._Base.In_TextOutputContext_Console(
                textOutput =>
                {
                    var dotnetDirectoryPath = Instances.DotnetPackPathOperator.Get_DotnetPackDirectoryPath(
                        dotnetPackName,
                        targetFramework,
                        textOutput);

                    Console.WriteLine(dotnetDirectoryPath);
                });
        }

        public async Task List_MemberDocumentations()
        {
            /// Inputs.
            var dotnetPackName = Instances.DotnetPackNames.Microsoft_NETCore_App_Ref;
            var targetFramework = Instances.TargetFrameworkMonikers.NET_6;
            var outputFilePath = Instances.FilePaths.OutputTextFilePath;


            /// Run.
            var memberDocumentationsByIdentityName = await Instances.DotnetPackOperator.Get_MemberDocumentationsByIdentityName(
                dotnetPackName,
                targetFramework);

            var lines = Instances.MemberDocumentationOperator_T0212_F000.Describe(memberDocumentationsByIdentityName.Values);

            Instances.NotepadPlusPlusOperator.WriteLinesAndOpen(
                outputFilePath.Value,
                lines);
        }

        public async Task Count_MemberDocumentations()
        {
            /// Inputs.
            var dotnetPackName = Instances.DotnetPackNames.Microsoft_NETCore_App_Ref;
            var targetFramework = Instances.TargetFrameworkMonikers.NET_6;
            //var dataFilePath = Instances.FilePaths.OutputDataFilePath.ToFilePath();


            /// Run.
            // Fails on serialization. But whatever, it's quick enough to just get the files all over again.
            //var memberDocumentationsByIdentityName = Instances.BinaryFileSerializer.Deserialize<Dictionary<IIdentityName, MemberDocumentation>>(
            //    dataFilePath);
            var memberDocumentationsByIdentityName = await Instances.DotnetPackOperator.Get_MemberDocumentationsByIdentityName(
                dotnetPackName,
                targetFramework);

            Console.WriteLine($"{memberDocumentationsByIdentityName.Count}: count");
        }

        public async Task Get_DotnetPackDocumentationComments()
        {
            /// Inputs.
            var dotnetPackName = Instances.DotnetPackNames.Microsoft_NETCore_App_Ref;
            var targetFramework = Instances.TargetFrameworkMonikers.NET_6;
            //var dataFilePath = Instances.FilePaths.OutputDataFilePath.ToFilePath();


            /// Run.
            //var documentationXmlFilePaths = Instances.DotnetPackPathOperator.GetDocumentationXmlFilePaths(
            //    dotnetPackName,
            //    targetFramework);

            //var documentationTarget = new DotnetFrameworkTarget()
            //{
            //    TargetFrameworkMoniker = targetFramework,
            //};

            //var memberDocumentationsByIdentityName = await Instances.DocumentationFileOperator.Get_MemberDocumentationsByIdentityName(
            //    documentationXmlFilePaths,
            //    documentationTarget);

            var memberDocumentationsByIdentityName = await Instances.DotnetPackOperator.Get_MemberDocumentationsByIdentityName(
                dotnetPackName,
                targetFramework);

            //// Binary serialize.
            //// Fails! ALL types must be marked with the Serializable attribute.
            //// I tried SharpSerializer in R5T.E0072, but it had trouble deserializing a dictionary?
            //Instances.BinaryFileSerializer.Serialize(
            //    dataFilePath,
            //    memberDocumentationsByIdentityName);

            // Whatever, it's quick enough to just deserialize all ~100 files. (About 2.5 seconds.)
        }
    }
}

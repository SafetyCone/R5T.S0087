using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using R5T.T0141;
using R5T.T0162;
using R5T.T0180.Extensions;
using R5T.T0212.F000;
using R5T.T0215;


namespace R5T.S0087
{
    [DemonstrationsMarker]
    public partial interface IDemonstrations : IDemonstrationsMarker
    {
        public void Get_DocumentationFilePaths()
        {
            /// Inputs.
            var dotnetPackName = Instances.DotnetPackNames.Microsoft_NETCore_App_Ref;
            var targetFramework = Instances.TargetFrameworkMonikers.NET_6;


            /// Run.
            Instances.TextOutputOperator.In_TextOutputContext_Console(
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
            Instances.TextOutputOperator.In_TextOutputContext_Console(
                textOutput =>
                {
                    var dotnetDirectoryPath = Instances.DotnetPackPathOperator.Get_DotnetPackDirectoryPath(
                        dotnetPackName,
                        targetFramework,
                        textOutput);

                    Console.WriteLine(dotnetDirectoryPath);
                });
        }

        public async Task Count_InheritDocElements()
        {

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

            var lines = Instances.MemberDocumentationOperator.Describe(memberDocumentationsByIdentityName.Values);

            Instances.NotepadPlusPlusOperator.WriteLinesAndOpen(
                outputFilePath,
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

using System;
using System.Threading.Tasks;


namespace R5T.S0087
{
    class Program
    {
        static async Task Main()
        {
            Scripts.Instance.Open_DotnetPacksDirectory_InExplorer();
            //await Scripts.Instance.Ingest_DotnetPackMemberDocumentations();
            //await Scripts.Instance.Verify_DotnetPackDirectoryXmlFilesAreDocumentationFiles();

            //Demonstrations.Instance.Get_DocumentationFilePaths();
            //Demonstrations.Instance.Get_DotnetPackDirectoryPath();
            //await Demonstrations.Instance.Get_DotnetPackDocumentationComments();
            //await Demonstrations.Instance.Count_MemberDocumentations();
            //await Demonstrations.Instance.List_MemberDocumentations();
            //Demonstrations.Instance.List_DuplicateDotnetPackAssemblyFilePaths();
            //await Demonstrations.Instance.Load_DotnetPackDocumentationFiles();

            //await Explorations.Instance.Find_AnyDuplicateNetFrameworkMemberIDs();
        }
    }
}
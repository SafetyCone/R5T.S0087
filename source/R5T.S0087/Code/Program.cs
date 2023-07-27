using System;
using System.Threading.Tasks;


namespace R5T.S0087
{
    class Program
    {
        static async Task Main()
        {
            //await Demonstrations.Instance.Get_DotnetPackDocumentationComments();
            //await Demonstrations.Instance.Count_MemberDocumentations();
            //await Demonstrations.Instance.List_MemberDocumentations();

            await Explorations.Instance.Find_AnyDuplicateNetFrameworkMemberIDs();
        }
    }
}
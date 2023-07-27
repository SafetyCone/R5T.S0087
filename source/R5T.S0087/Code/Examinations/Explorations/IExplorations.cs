using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R5T.T0141;
using R5T.T0162;
using R5T.T0212.F000;
using R5T.T0215;


namespace R5T.S0087
{
    [ExplorationsMarker]
    public partial interface IExplorations : IExplorationsMarker
    {
        /// <summary>
        /// There are several .NET framework packs (<see cref="T0215.Z000.IDotnetPackNames.Microsoft_AspNetCore_App_Ref"/>, <see cref="T0215.Z000.IDotnetPackNames.NETStandard_Library_Ref"/>, etc.).
        /// Are there duplicate member IDs in the documentation files across these packs?
        /// <para>
        /// Result: yes, there are some duplicate IDs, but not many.
        /// Also: even within a pack's documentation files, there are some member IDs that are the same, but have different member documentation values!
        /// </para>
        /// </summary>
        public async Task Find_AnyDuplicateNetFrameworkMemberIDs()
        {
            /// Inputs.
            // Use all useful dotnet packs.
            var dotnetPackNames = Instances.DotnetPackNameSets.ForNet6;
            // Standardize on the .NET 6.0 framework.
            var targetFramework = Instances.TargetFrameworkMonikers.NET_6;


            /// Run.
            // Get member documentations for each pack.
            var memberDocumentationsByPack = new Dictionary<IDotnetPackName, IDictionary<IIdentityName, MemberDocumentation>>();

            foreach (var dotnetPackName in dotnetPackNames)
            {
                var memberDocumentationsByIdentityName = await Instances.DotnetPackOperator.Get_MemberDocumentationsByIdentityName(
                    dotnetPackName,
                    targetFramework);

                memberDocumentationsByPack.Add(
                    dotnetPackName,
                    memberDocumentationsByIdentityName);
            }

            // Now compare across packs.
            var dotnetPackPairs = Instances.EnumerableOperator.Get_Combinations(dotnetPackNames);

            foreach (var pair in dotnetPackPairs)
            {
                var firstMemberDocumentations = memberDocumentationsByPack[pair.Item1];
                var secondMemberDocumentations = memberDocumentationsByPack[pair.Item2];

                var commonMemberNames = firstMemberDocumentations.Keys.Intersect(
                    secondMemberDocumentations.Keys)
                    .Now();

                var lines = Instances.EnumerableOperator_F0000.From($"{pair.Item1}:{pair.Item2}")
                    .Append($"{commonMemberNames.Length}: count (of {firstMemberDocumentations.Count} for '{pair.Item1}', and {secondMemberDocumentations.Count} for '{pair.Item2}')")
                    .Append(
                        commonMemberNames
                            .Select(x => x.Value)
                            .OrderAlphabetically()
                    );

                Instances.FileOperator.WriteAllLines_Synchronous(
                    $@"C:\Temp\{pair.Item1}-{pair.Item2}.txt",
                    lines);
            }
        }
    }
}

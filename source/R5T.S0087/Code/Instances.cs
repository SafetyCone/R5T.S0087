using System;


namespace R5T.S0087
{
    public class Instances :
        L0055.Instances
    {
        public static L0057.IAssemblyFilePathOperator AssemblyFilePathOperator => L0057.AssemblyFilePathOperator.Instance;
        public static L0035.IBinaryFileSerializer BinaryFileSerializer => L0035.BinaryFileSerializer.Instance;
        public static L0053.IDictionaryOperator DictionaryOperator => L0053.DictionaryOperator.Instance;
        public static T0212.F000.IDocumentationFileOperator DocumentationFileOperator => T0212.F000.DocumentationFileOperator.Instance;
        public static L0069.IDocumentationTargetOperator DocumentationTargetOperator => L0069.DocumentationTargetOperator.Instance;
        public static T0215.Z000.IDotnetPackNames DotnetPackNames => T0215.Z000.DotnetPackNames.Instance;
        public static T0215.Z000.IDotnetPackNameSets DotnetPackNameSets => T0215.Z000.DotnetPackNameSets.Instance;
        public static F0141.IDotnetPackOperator DotnetPackOperator => F0141.DotnetPackOperator.Instance;
        public static L0068.IDotnetPackPathsOperator DotnetPackPathsOperator => L0068.DotnetPackPathsOperator.Instance;
        public static T0214.Z001.IDotnetPacksDirectoryPaths DotnetPacksDirectoryPaths => T0214.Z001.DotnetPacksDirectoryPaths.Instance;
        public static F0000.IEnumerableOperator EnumerableOperator_F0000 => F0000.EnumerableOperator.Instance;
        //public static F0000.IFileOperator FileOperator => F0000.FileOperator.Instance;
        public static IMemberDocumentationOperator MemberDocumentationOperator => S0087.MemberDocumentationOperator.Instance;
        public static T0212.F000.IMemberDocumentationOperator MemberDocumentationOperator_T0212_F000 => T0212.F000.MemberDocumentationOperator.Instance;
        public static T0212.F000.IMemberElementOperator MemberElementOperator => T0212.F000.MemberElementOperator.Instance;
        public static L0032.Z000.IProjectSdkNames ProjectSdkNames => L0032.Z000.ProjectSdkNames.Instance;
        public static Z0057.ITargetFrameworkMonikers TargetFrameworkMonikers => Z0057.TargetFrameworkMonikers.Instance;
    }
}
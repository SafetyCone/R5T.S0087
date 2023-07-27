using System;


namespace R5T.S0087
{
    public static class Instances
    {
        public static L0035.IBinaryFileSerializer BinaryFileSerializer => L0035.BinaryFileSerializer.Instance;
        public static T0212.F000.IDocumentationFileOperator DocumentationFileOperator => T0212.F000.DocumentationFileOperator.Instance;
        public static T0215.Z000.IDotnetPackNames DotnetPackNames => T0215.Z000.DotnetPackNames.Instance;
        public static T0215.Z000.IDotnetPackNameSets DotnetPackNameSets => T0215.Z000.DotnetPackNameSets.Instance;
        public static F0141.IDotnetPackOperator DotnetPackOperator => F0141.DotnetPackOperator.Instance;
        public static F0138.IDotnetPackPathOperator DotnetPackPathOperator => F0138.DotnetPackPathOperator.Instance;
        public static L0035.IEnumerableOperator EnumerableOperator => L0035.EnumerableOperator.Instance;
        public static F0000.IEnumerableOperator EnumerableOperator_F0000 => F0000.EnumerableOperator.Instance;
        public static F0000.IFileOperator FileOperator => F0000.FileOperator.Instance;
        public static Z0015.IFilePaths FilePaths => Z0015.FilePaths.Instance;
        public static T0212.F000.IMemberDocumentationOperator MemberDocumentationOperator => T0212.F000.MemberDocumentationOperator.Instance;
        public static F0033.INotepadPlusPlusOperator NotepadPlusPlusOperator => F0033.NotepadPlusPlusOperator.Instance;
        public static Z0057.ITargetFrameworkMonikers TargetFrameworkMonikers => Z0057.TargetFrameworkMonikers.Instance;
    }
}
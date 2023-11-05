using System;
using System.Text;
using System.Xml;

using R5T.L0062.T000.Extensions;
using R5T.L0069.T000;
using R5T.T0132;
using R5T.T0212;


namespace R5T.S0087
{
    [FunctionalityMarker]
    public partial interface IMemberDocumentationOperator : IFunctionalityMarker
    {
        public string Describe(MemberDocumentation memberDocumentation)
        {
            var stringBuilder = new StringBuilder();

            var xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
            };

            using (var xmlWriter = XmlWriter.Create(
                stringBuilder,
                xmlWriterSettings))
            {
                memberDocumentation.MemberElement.Value.WriteTo(xmlWriter);
            }

            var text = stringBuilder.ToString();

            return $"{memberDocumentation.IdentityString}:\n{memberDocumentation.DocumentationTarget}\n{text}\n";
        }

        /// <summary>
        /// Does not perform any reformatting of the member element XML content (for example, to remove indentation).
        /// That should be done either before or after calling this method to get the member element content into its most useful form.
        /// See <see cref="R5T.T0212.F000.IMemberElementOperator.RemoveExtraTextLineEndings(IMemberElement)"/> for that.
        /// </summary>
        public MemberDocumentation Get_MemberDocumentation(
            IMemberElement memberElement,
            IDocumentationTarget documentationTarget)
        {
            var identityString = Instances.MemberElementOperator._Platform.Get_IdentityString(memberElement)
                .ToIdentityString();

            var output = new MemberDocumentation
            {
                IdentityString = identityString,
                MemberElement = memberElement,
                DocumentationTarget = documentationTarget,
            };

            return output;
        }
    }
}

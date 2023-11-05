using System;

using R5T.L0062.T000;
using R5T.L0069.T000;
using R5T.T0212;


namespace R5T.S0087
{
    public class MemberDocumentation
    {
        public IIdentityString IdentityString { get; set; }
        public IMemberElement MemberElement { get; set; }
        public IDocumentationTarget DocumentationTarget { get; set; }


        public override string ToString()
        {
            var output = this.IdentityString.ToString();
            return output;
        }
    }
}

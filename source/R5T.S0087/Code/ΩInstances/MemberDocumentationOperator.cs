using System;


namespace R5T.S0087
{
    public class MemberDocumentationOperator : IMemberDocumentationOperator
    {
        #region Infrastructure

        public static IMemberDocumentationOperator Instance { get; } = new MemberDocumentationOperator();


        private MemberDocumentationOperator()
        {
        }

        #endregion
    }
}

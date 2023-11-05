using System;


namespace R5T.S0087
{
    public class DotnetPackOperator : IDotnetPackOperator
    {
        #region Infrastructure

        public static IDotnetPackOperator Instance { get; } = new DotnetPackOperator();


        private DotnetPackOperator()
        {
        }

        #endregion
    }
}

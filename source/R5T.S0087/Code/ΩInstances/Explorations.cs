using System;


namespace R5T.S0087
{
    public class Explorations : IExplorations
    {
        #region Infrastructure

        public static IExplorations Instance { get; } = new Explorations();


        private Explorations()
        {
        }

        #endregion
    }
}

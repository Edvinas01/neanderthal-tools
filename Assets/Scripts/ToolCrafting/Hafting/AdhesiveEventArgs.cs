namespace NeanderthalTools.ToolCrafting.Hafting
{
    public class AdhesiveEventArgs
    {
        #region Properties

        public RawAdhesive RawAdhesive { get; }

        #endregion

        #region Methods

        public AdhesiveEventArgs(RawAdhesive rawAdhesive)
        {
            RawAdhesive = rawAdhesive;
        }

        #endregion
    }
}

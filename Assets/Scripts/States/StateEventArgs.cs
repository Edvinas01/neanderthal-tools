namespace NeanderthalTools.States
{
    public class StateEventArgs
    {
        #region Properties

        public State State { get; }

        #endregion

        #region Methods

        public StateEventArgs(State state)
        {
            State = state;
        }

        #endregion
    }
}

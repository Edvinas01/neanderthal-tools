namespace NeanderthalTools.States
{
    public class StateEventArgs
    {
        public State State { get; }

        public StateEventArgs(State state)
        {
            State = state;
        }
    }
}

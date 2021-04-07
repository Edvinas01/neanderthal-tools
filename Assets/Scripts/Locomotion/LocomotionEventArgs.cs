using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Locomotion
{
    public class LocomotionEventArgs
    {
        #region Properties

        public LocomotionSystem LocomotionSystem { get; }

        #endregion

        #region Methods

        public LocomotionEventArgs(LocomotionSystem locomotionSystem)
        {
            LocomotionSystem = locomotionSystem;
        }

        #endregion
    }
}

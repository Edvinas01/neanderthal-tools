using NeanderthalTools.Haptics;
using UnityEngine;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    public class HaftingHapticFeedbackAdapter : MonoBehaviour
    {
        #region Enums

        private enum Target
        {
            Handle,
            ToolPart,
            Both
        }

        #endregion

        #region Editor

        [SerializeField]
        private HapticFeedbackSettings settings;

        [SerializeField]
        private Target target = Target.Both;

        #endregion

        #region Methods

        public void SendHapticImpulse(HaftEventArgs args)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            switch (target)
            {
                case Target.Handle:
                    settings.SendHapticImpulse(args.HandleInteractor);
                    break;
                case Target.ToolPart:
                    settings.SendHapticImpulse(args.ToolPartInteractor);
                    break;
                case Target.Both:
                    settings.SendHapticImpulse(args.HandleInteractor, args.ToolPartInteractor);
                    break;
            }
        }

        #endregion
    }
}

using NeanderthalTools.Haptics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.ToolCrafting.Knapping
{
    public class FlakeHapticFeedbackAdapter : MonoBehaviour
    {
        #region Enums

        private enum Target
        {
            Knapper,
            Objective,
            Both
        }

        #endregion

        #region Editor

        [SerializeField]
        private HapticFeedbackSettings settings;

        [SerializeField]
        private Target target = Target.Knapper;

        #endregion

        #region Methods

        public void SendHapticImpulse(FlakeEventArgs args)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            switch (target)
            {
                case Target.Knapper:
                    SendHapticImpulse(args.KnapperInteractor);
                    break;
                case Target.Objective:
                    SendHapticImpulse(args.ObjectiveInteractor);
                    break;
                case Target.Both:
                    SendHapticImpulse(args.KnapperInteractor, args.ObjectiveInteractor);
                    break;
            }
        }

        private void SendHapticImpulse(params XRBaseInteractor[] interactors)
        {
            foreach (var interactor in interactors)
            {
                if (interactor == null)
                {
                    continue;
                }

                var feedback = interactor.GetComponent<ManagedHapticFeedback>();
                if (feedback == null)
                {
                    continue;
                }

                feedback.SendHapticImpulse(settings);
            }
        }

        #endregion
    }
}

using System.Linq;
using NeanderthalTools.Haptics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Knapping
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
            var controllers = interactors
                .Where(interactor => interactor != null)
                .Select(interactor => interactor.GetComponent<XRBaseController>())
                .Where(controller => controller != null)
                .ToArray();

            settings.SendHapticImpulse(controllers);
        }

        #endregion
    }
}

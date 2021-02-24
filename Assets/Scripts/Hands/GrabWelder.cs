using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Hands
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(XRGrabInteractable))]
    public class GrabWelder : MonoBehaviour
    {
        private XRGrabInteractable interactable;
        private new Rigidbody rigidbody;
        private FixedJoint joint;

        private void Awake()
        {
            interactable = GetComponent<XRGrabInteractable>();
            interactable.trackPosition = false;
            interactable.trackRotation = false;
            interactable.movementType = XRBaseInteractable.MovementType.VelocityTracking;

            rigidbody = GetComponent<Rigidbody>();
        }

        public void Weld(SelectEnterEventArgs args)
        {
            joint = args.interactor.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = rigidbody;
        }

        public void UnWeld(SelectExitEventArgs args)
        {
            if (joint != null)
            {
                Destroy(joint);
            }
        }
    }
}

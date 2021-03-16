using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Locomotion
{
    public class LocomotionHandler : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private LocomotionSettings locomotionSettings;

        [SerializeField]
        private InputActionReference teleportActivate;

        [SerializeField]
        private InputActionReference teleportCancel;

        [SerializeField]
        private ContinuousMoveProviderBase continuousMoveProvider;

        [SerializeField]
        private SnapTurnProviderBase snapTurnProvider;

        [SerializeField]
        private TeleportationProvider teleportationProvider;

        [SerializeField]
        private XRBaseController teleportController;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            continuousMoveProvider.enabled = locomotionSettings.ContinuousMove;
            snapTurnProvider.enabled = locomotionSettings.SnapTurn;
            teleportationProvider.enabled = locomotionSettings.Teleport;

            SetTeleportActivated(false);
        }

        private void OnEnable()
        {
            teleportActivate.action.performed += OnTeleportActivatePerformed;
            teleportActivate.action.canceled += OnTeleportActivateCanceled;
            teleportCancel.action.performed += OnTeleportCancelPerformed;
        }

        private void OnDisable()
        {
            teleportActivate.action.performed -= OnTeleportActivatePerformed;
            teleportActivate.action.canceled -= OnTeleportActivateCanceled;
            teleportCancel.action.performed -= OnTeleportCancelPerformed;
        }

        #endregion

        #region Methods

        private void OnTeleportActivatePerformed(InputAction.CallbackContext ctx)
        {
            SetTeleportActivated(true);
        }

        private void OnTeleportActivateCanceled(InputAction.CallbackContext ctx)
        {
            if (IsTeleportActivated())
            {
                StartDeactivateTeleport();
            }
        }

        private void OnTeleportCancelPerformed(InputAction.CallbackContext ctx)
        {
            if (IsTeleportActivated())
            {
                SetTeleportActivated(false);
            }
        }

        private void StartDeactivateTeleport()
        {
            StopAllCoroutines();
            StartCoroutine(DeactivateTeleport());
        }

        private IEnumerator DeactivateTeleport()
        {
            // Wait one frame so that teleportation gets executed.
            yield return null;

            SetTeleportActivated(false);
        }

        private bool IsTeleportActivated()
        {
            return locomotionSettings.Teleport && teleportController.gameObject.activeSelf;
        }

        private void SetTeleportActivated(bool activated)
        {
            teleportController.gameObject.SetActive(activated);
        }

        #endregion
    }
}

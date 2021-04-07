using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Locomotion
{
    [RequireComponent(typeof(SnapTurnProviderBase))]
    [RequireComponent(typeof(TeleportationProvider))]
    [RequireComponent(typeof(ContinuousMoveProviderBase))]
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
        private XRBaseController teleportController;

        [SerializeField]
        private LocomotionUnityEvent onTeleport;

        [SerializeField]
        private LocomotionUnityEvent onSnapTurn;

        #endregion

        #region Fields

        private ContinuousMoveProviderBase continuousMoveProvider;
        private TeleportationProvider teleportationProvider;
        private SnapTurnProviderBase snapTurnProvider;

        private bool teleportActive;

        #endregion

        #region Events

        public event Action<LocomotionSystem> OnLocomotionEnd;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            continuousMoveProvider = GetComponent<ContinuousMoveProviderBase>();
            teleportationProvider = GetComponent<TeleportationProvider>();
            snapTurnProvider = GetComponent<SnapTurnProviderBase>();

            continuousMoveProvider.enabled = locomotionSettings.ContinuousMove;
            teleportationProvider.enabled = locomotionSettings.Teleport;
            snapTurnProvider.enabled = locomotionSettings.SnapTurn;

            SetTeleportController(false);
        }

        private void OnEnable()
        {
            continuousMoveProvider.endLocomotion += InvokeOnLocomotionEnd;
            teleportationProvider.endLocomotion += InvokeOnLocomotionEnd;
            snapTurnProvider.endLocomotion += InvokeOnLocomotionEnd;
            snapTurnProvider.endLocomotion += InvokeOnTeleport;
            snapTurnProvider.endLocomotion += InvokeOnSnapTurn;
        }

        private void OnDisable()
        {
            continuousMoveProvider.endLocomotion -= InvokeOnLocomotionEnd;
            teleportationProvider.endLocomotion -= InvokeOnLocomotionEnd;
            snapTurnProvider.endLocomotion -= InvokeOnLocomotionEnd;
            snapTurnProvider.endLocomotion -= InvokeOnTeleport;
            snapTurnProvider.endLocomotion -= InvokeOnSnapTurn;
        }

        private void Update()
        {
            if (locomotionSettings.Teleport)
            {
                UpdateTeleport();
            }
        }

        #endregion

        #region Methods

        private void InvokeOnLocomotionEnd(LocomotionSystem locomotionSystem)
        {
            OnLocomotionEnd?.Invoke(locomotionSystem);
        }

        private void InvokeOnTeleport(LocomotionSystem locomotionSystem)
        {
            onTeleport.Invoke(CreateLocomotionEventArgs(locomotionSystem));
        }

        private void InvokeOnSnapTurn(LocomotionSystem locomotionSystem)
        {
            onSnapTurn.Invoke(CreateLocomotionEventArgs(locomotionSystem));
        }

        private LocomotionEventArgs CreateLocomotionEventArgs(LocomotionSystem locomotionSystem)
        {
            return new LocomotionEventArgs(locomotionSystem);
        }

        private void UpdateTeleport()
        {
            if (teleportActive)
            {
                UpdateActiveTeleport();
            }
            else
            {
                UpdateIdleTeleport();
            }
        }

        private void UpdateActiveTeleport()
        {
            var teleportReleased = teleportActivate.action.phase == InputActionPhase.Waiting;
            var teleportCanceled = teleportCancel.action.triggered;

            if (teleportReleased || teleportCanceled)
            {
                SetTeleportController(false);
                teleportActive = false;
            }
        }

        private void UpdateIdleTeleport()
        {
            var teleportActivated = teleportActivate.action.triggered;
            if (teleportActivated)
            {
                SetTeleportController(true);
                teleportActive = true;
            }
        }

        private void SetTeleportController(bool activated)
        {
            teleportController.gameObject.SetActive(activated);
        }

        #endregion
    }
}

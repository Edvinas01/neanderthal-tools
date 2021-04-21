using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace NeanderthalTools.Triggers
{
    public class HoldInputActionTrigger : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private InputActionReference inputAction;

        [Min(0f)]
        [SerializeField]
        private float holdDuration = 4f;

        [SerializeField]
        private UnityEvent onTrigger;

        #endregion

        #region Fields

        private bool holding;
        private float holdingEnd;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            inputAction.action.performed += OnPerformed;
            inputAction.action.canceled += OnCanceled;
        }

        private void OnDisable()
        {
            inputAction.action.performed -= OnPerformed;
            inputAction.action.canceled -= OnCanceled;
        }

        private void Update()
        {
            if (holding && holdingEnd <= Time.time)
            {
                onTrigger.Invoke();
                holding = false;
            }
        }

        #endregion

        #region Methods

        private void OnPerformed(InputAction.CallbackContext ctx)
        {
            holding = true;
            holdingEnd = Time.time + holdDuration;
        }

        private void OnCanceled(InputAction.CallbackContext ctx)
        {
            holding = false;
            holdingEnd = 0f;
        }

        #endregion
    }
}

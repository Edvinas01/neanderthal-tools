using System;
using UnityEngine;
using UnityEngine.Events;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    public class Adhesive : MonoBehaviour, IToolPart
    {
        #region Editor

        [Min(0f)]
        [SerializeField]
        [Tooltip("Speed of adhesive production upon adding material")]
        private float productionSpeed = 1f;

        [SerializeField]
        private UnityEvent onStartProduction;

        [SerializeField]
        private UnityEvent onStopProduction;

        #endregion

        #region Fields

        private Vector3 initialScale;

        private float currentAmount;
        private float targetAmount;

        private bool pendingProductionStop;

        #endregion

        #region Properties

        public float Amount
        {
            get => currentAmount;
            set
            {
                targetAmount = Mathf.Min(value, 1f);
                if (!IsTargetAmount())
                {
                    onStartProduction.Invoke();
                    pendingProductionStop = true;
                }
            }
        }

        public Vector3 AttachDirection => Vector3.zero;

        public string Name => name;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            SetupInitialScale();
            UpdateScale();
        }


        private void Update()
        {
            if (IsTargetAmount())
            {
                return;
            }

            UpdateCurrentAmount();
            UpdateScale();
        }

        #endregion

        #region Methods

        private void SetupInitialScale()
        {
            initialScale = transform.localScale;
        }

        private void UpdateCurrentAmount()
        {
            currentAmount = Mathf.Lerp(
                currentAmount,
                targetAmount,
                Time.deltaTime * productionSpeed
            );

            if (IsTargetAmount())
            {
                currentAmount = targetAmount;
                if (pendingProductionStop)
                {
                    HandleProductionStop();
                }
            }
        }

        private void UpdateScale()
        {
            transform.localScale = initialScale * currentAmount;
        }

        private bool IsTargetAmount()
        {
            return Math.Abs(currentAmount - targetAmount) < 0.01f;
        }

        private void HandleProductionStop()
        {
            onStopProduction.Invoke();
            pendingProductionStop = false;
        }

        #endregion
    }
}

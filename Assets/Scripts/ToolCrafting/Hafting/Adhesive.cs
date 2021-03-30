using UnityEngine;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    public class Adhesive : MonoBehaviour, IToolPart
    {
        #region Editor

        [Min(0f)]
        [SerializeField]
        [Tooltip("Speed of adhesive production upon adding material")]
        private float productionSpeed = 1f;

        #endregion

        #region Fields

        private Vector3 initialScale;

        private float currentAmount;
        private float targetAmount;

        #endregion

        #region Properties

        public float Amount
        {
            get => currentAmount;
            set => targetAmount = Mathf.Min(value, 1f);
        }

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
        }

        private void UpdateScale()
        {
            transform.localScale = initialScale * currentAmount;
        }

        private bool IsTargetAmount()
        {
            return Mathf.Approximately(currentAmount, targetAmount);
        }

        #endregion
    }
}

using UnityEngine;

namespace NeanderthalTools.Knapping
{
    public class Objective : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        [Tooltip("Called when there are some dependencies remaining")]
        private FlakeUnityEvent onDependenciesRemaining;

        [SerializeField]
        [Tooltip("Called when the impact angle is invalid")]
        private FlakeUnityEvent onInvalidAngle;

        [SerializeField]
        [Tooltip("Called when the impact force is too weak")]
        private FlakeUnityEvent onWeakImpact;

        [SerializeField]
        [Tooltip("Called when the flake detaches")]
        private FlakeUnityEvent onDetach;

        #endregion

        #region Properties

        public FlakeUnityEvent OnDependenciesRemaining => onDependenciesRemaining;

        public FlakeUnityEvent OnInvalidAngle => onInvalidAngle;

        public FlakeUnityEvent OnWeakImpact => onWeakImpact;

        public FlakeUnityEvent OnDetach => onDetach;

        #endregion
    }
}

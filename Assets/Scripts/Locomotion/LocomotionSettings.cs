using UnityEngine;

namespace NeanderthalTools.Locomotion
{
    [CreateAssetMenu(fileName = "LocomotionSettings", menuName = "Game/Locomotion Settings")]
    public class LocomotionSettings : ScriptableObject
    {
        #region Editor

        [SerializeField]
        private bool continuousMove;

        [SerializeField]
        private bool snapTurn;

        [SerializeField]
        private bool teleport;

        #endregion

        #region Properties

        public bool ContinuousMove => continuousMove;

        public bool SnapTurn => snapTurn;

        public bool Teleport => teleport;

        #endregion
    }
}

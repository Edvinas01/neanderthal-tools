using NaughtyAttributes;
using UnityEngine;

namespace NeanderthalTools.Scenes
{
    [CreateAssetMenu(fileName = "SceneSettings", menuName = "Game/Scene Settings")]
    public class SceneSettings : ScriptableObject
    {
        #region Editor

        [Scene]
        [SerializeField]
        private int bootstrapSceneIndex;

        [Scene]
        [SerializeField]
        private int mainSceneIndex;

        #endregion

        #region Properties

        public int BootstrapSceneIndex => bootstrapSceneIndex;

        public int MainSceneIndex => mainSceneIndex;

        #endregion
    }
}

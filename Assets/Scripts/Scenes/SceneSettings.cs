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
        private int menuSceneIndex = 1;

        [Scene]
        [SerializeField]
        private int mainSceneIndex = 2;

        #endregion

        #region Properties

        public int BootstrapSceneIndex => bootstrapSceneIndex;

        public int MenuSceneIndex => menuSceneIndex;

        public int MainSceneIndex => mainSceneIndex;

        #endregion
    }
}

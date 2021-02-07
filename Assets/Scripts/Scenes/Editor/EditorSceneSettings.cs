using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace NeanderthalTools.Scenes.Editor
{
    [CreateAssetMenu(fileName = "EditorSceneSettings", menuName = "Game/Editor Scene Settings")]
    public class EditorSceneSettings : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Enable bootstrap scene functionality while in editor")]
        private bool bootstrapEditor = true;

        [SerializeField]
        [NaughtyAttributes.ReadOnly]
        private SceneSetup[] setup = new SceneSetup[0];

        public bool BootstrapEditor => bootstrapEditor;

        /// <summary>
        /// Scene setup before entering the play mode.
        /// </summary>
        public SceneSetup[] Setup
        {
            get => setup;
            set => Replace(value);
        }

        private void Replace(IEnumerable<SceneSetup> newSetups)
        {
            setup = newSetups.Select(Copy).ToArray();
        }

        private static SceneSetup Copy(SceneSetup sceneSetup)
        {
            return new SceneSetup
            {
                isActive = sceneSetup.isActive,
                isLoaded = sceneSetup.isLoaded,
                isSubScene = sceneSetup.isSubScene,
                path = sceneSetup.path
            };
        }
    }
}

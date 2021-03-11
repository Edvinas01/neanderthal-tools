using System.Linq;
using NeanderthalTools.Util.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using static NeanderthalTools.Util.Editor.ScriptableObjectExtensions;

namespace NeanderthalTools.Scenes.Editor
{
    [InitializeOnLoad]
    public static class SceneBootstrapLoader
    {
        #region Fields

        private static EditorSceneSettings editorSceneSettings;
        private static SceneSettings sceneSettings;

        #endregion

        #region Properties

        private static EditorSceneSettings EditorSceneSettings
        {
            get
            {
                if (editorSceneSettings == null)
                {
                    editorSceneSettings = FindOrCreateAsset<EditorSceneSettings>(
                        "Assets/Settings/Scenes/EditorSceneSettings.asset"
                    );
                }

                return editorSceneSettings;
            }
        }

        private static SceneSettings SceneSettings
        {
            get
            {
                if (sceneSettings == null)
                {
                    sceneSettings = FindOrCreateAsset<SceneSettings>(
                        "Assets/Settings/Scenes/SceneSettings.asset"
                    );
                }

                return sceneSettings;
            }
        }

        #endregion

        #region Methods

        static SceneBootstrapLoader()
        {
            EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
        }

        [OnOpenAsset]
        private static bool OnOpenAsset(int instanceID, int line)
        {
            if (!EditorSceneSettings.BootstrapEditor)
            {
                return false;
            }

            var sceneAsset = EditorUtility.InstanceIDToObject(instanceID) as SceneAsset;
            if (sceneAsset == null)
            {
                return false;
            }

            var scenePath = AssetDatabase.GetAssetPath(sceneAsset);
            if (IsBootstrapScene(scenePath))
            {
                return false;
            }

            BootstrapSceneSetup(scenePath);

            return true;
        }

        private static bool IsBootstrapScene(string scenePath)
        {
            var buildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
            return buildIndex == SceneSettings.BootstrapSceneIndex;
        }

        private static void BootstrapSceneSetup(string scenePath)
        {
            EditorSceneManager.RestoreSceneManagerSetup(new[]
            {
                new SceneSetup
                {
                    isActive = false,
                    isLoaded = true,
                    path = SceneSettings.GetBootstrapScenePath()
                },
                new SceneSetup
                {
                    isActive = true,
                    isLoaded = true,
                    path = scenePath
                }
            });
        }

        private static void OnPlaymodeStateChanged(PlayModeStateChange change)
        {
            if (!EditorSceneSettings.BootstrapEditor)
            {
                return;
            }

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (change)
            {
                case PlayModeStateChange.ExitingEditMode:
                    BootstrapSceneSettings();
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    RestoreSceneSettings();
                    break;
            }
        }

        private static void BootstrapSceneSettings()
        {
            var setup = EditorSceneManager.GetSceneManagerSetup();
            EditorSceneSettings.Setup = setup;
            AssetDatabase.SaveAssets();

            var bootstrapScene = SceneSettings.GetBootstrapScene();
            if (bootstrapScene.IsValid())
            {
                SceneManager.SetActiveScene(bootstrapScene);
            }
        }

        private static void RestoreSceneSettings()
        {
            var activeScene = EditorSceneSettings
                .Setup
                .FirstOrDefault(sceneSetup => sceneSetup.isActive)
                .GetScene();

            SceneManager.SetActiveScene(activeScene);
        }

        #endregion
    }
}

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

        private const string EditorSceneSettingsPath =
            "Assets/Settings/Scenes/EditorSceneSettings.asset";

        private const string SceneSettingsPath =
            "Assets/Settings/Scenes/SceneSettings.asset";

        private static readonly EditorSceneSettings EditorSceneSettings;
        private static readonly SceneSettings SceneSettings;

        #endregion

        #region Methods

        static SceneBootstrapLoader()
        {
            EditorSceneSettings = FindOrCreateAsset<EditorSceneSettings>(EditorSceneSettingsPath);
            SceneSettings = FindOrCreateAsset<SceneSettings>(SceneSettingsPath);

            EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
        }

        [OnOpenAsset]
        private static bool OnOpenAsset(int instanceID, int line)
        {
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

            SceneManager.SetActiveScene(SceneSettings.GetBootstrapScene());
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

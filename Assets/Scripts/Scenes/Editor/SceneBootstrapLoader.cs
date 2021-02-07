using System.Linq;
using NeanderthalTools.Extensions.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using static NeanderthalTools.Extensions.Editor.ScriptableObjectExtensions;

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

            EditorSceneManager.sceneOpened += OnSceneOpened;
            EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
        }

        private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            if (!EditorSceneSettings.BootstrapEditor
                || BuildPipeline.isBuildingPlayer
                || !scene.IsValid()
                || IsBootstrapScene(scene))
            {
                return;
            }

            BootstrapSceneSetup(scene);
        }

        private static bool IsBootstrapScene(Scene scene)
        {
            return scene.buildIndex == SceneSettings.BootstrapSceneIndex;
        }

        private static void BootstrapSceneSetup(Scene scene)
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
                    path = scene.path
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

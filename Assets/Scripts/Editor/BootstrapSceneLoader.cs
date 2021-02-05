using System.IO;
using System.Linq;
using NeanderthalTools.Scenes;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NeanderthalTools.Editor
{
    [InitializeOnLoad]
    public static class BootstrapSceneLoader
    {
        #region Fields

        private const string DefaultSceneSettingsDirectoryPath = "Assets/Settings/Scenes";

        private const string DefaultSceneSettingsAssetPath =
            DefaultSceneSettingsDirectoryPath + "/SceneSettings.asset";

        private static readonly SceneSettings SceneSettings;

        #endregion

        #region Methods

        static BootstrapSceneLoader()
        {
            SceneSettings = LoadSceneSettings();
            EditorSceneManager.sceneOpened += SceneOpened;
        }

        private static SceneSettings LoadSceneSettings()
        {
            var sceneSettings = FindSceneSettings();
            return sceneSettings != null
                ? sceneSettings
                : CreateSceneSettings();
        }

        private static SceneSettings FindSceneSettings()
        {
            return AssetDatabase
                .FindAssets($"t:{nameof(Scenes.SceneSettings)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<SceneSettings>)
                .FirstOrDefault();
        }

        private static SceneSettings CreateSceneSettings()
        {
            var sceneSettings = ScriptableObject.CreateInstance<SceneSettings>();

            Directory.CreateDirectory(DefaultSceneSettingsDirectoryPath);

            AssetDatabase.CreateAsset(sceneSettings, DefaultSceneSettingsAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return sceneSettings;
        }

        private static void SceneOpened(Scene scene, OpenSceneMode mode)
        {
            if (!scene.IsValid() || IsBootstrapScene(scene))
            {
                return;
            }

            var sceneSetups = CreateBootstrappedSceneSetups(scene);
            EditorSceneManager.RestoreSceneManagerSetup(sceneSetups);
        }

        private static bool IsBootstrapScene(Scene scene)
        {
            return scene.buildIndex == SceneSettings.BootstrapSceneIndex;
        }

        private static SceneSetup[] CreateBootstrappedSceneSetups(Scene scene)
        {
            return new[]
            {
                new SceneSetup
                {
                    isActive = false,
                    isLoaded = true,
                    path = GetBootstrapScenePath()
                },
                new SceneSetup
                {
                    isActive = true,
                    isLoaded = true,
                    path = scene.path
                }
            };
        }

        private static string GetBootstrapScenePath()
        {
            return SceneUtility.GetScenePathByBuildIndex(SceneSettings.BootstrapSceneIndex);
        }

        #endregion
    }
}

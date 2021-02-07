using NeanderthalTools.Scenes;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace NeanderthalTools.Extensions.Editor
{
    public static class SceneExtensions
    {
        public static Scene GetBootstrapScene(this SceneSettings sceneSettings)
        {
            return SceneManager.GetSceneByPath(sceneSettings.GetBootstrapScenePath());
        }

        public static Scene GetScene(this SceneSetup sceneSetup)
        {
            return SceneManager.GetSceneByPath(sceneSetup.path);
        }

        public static string GetBootstrapScenePath(this SceneSettings sceneSettings)
        {
            return SceneUtility.GetScenePathByBuildIndex(sceneSettings.BootstrapSceneIndex);
        }
    }
}

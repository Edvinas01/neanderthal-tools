using NeanderthalTools.Scenes;
using UnityEngine.SceneManagement;

namespace NeanderthalTools.Extensions.Editor
{
    public static class SceneSettingsExtensions
    {
        public static string GetBootstrapScenePath(this SceneSettings sceneSettings)
        {
            return SceneUtility.GetScenePathByBuildIndex(sceneSettings.BootstrapSceneIndex);
        }
    }
}

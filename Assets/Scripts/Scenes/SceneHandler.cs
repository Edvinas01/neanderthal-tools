using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NeanderthalTools.Scenes
{
    public class SceneHandler : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private SceneSettings sceneSettings;

        #endregion

        #region Fields

        private bool loadingScene;

        #endregion

        #region Unity Lifecycle

        private void Start()
        {
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.buildIndex == sceneSettings.BootstrapSceneIndex)
            {
                return;
            }

            LoadMainScene();
        }

        #endregion

        #region Methods

        public void LoadMainScene()
        {
            StartLoadScene(sceneSettings.MainSceneIndex);
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void RestartScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            StartLoadScene(activeScene.buildIndex);
        }

        #endregion

        #region Private Methods

        private void StartLoadScene(int sceneIndex)
        {
            if (loadingScene)
            {
                return;
            }

            StartCoroutine(LoadScene(sceneIndex));
        }

        private IEnumerator LoadScene(int sceneIndex)
        {
            loadingScene = true;

            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.buildIndex > 0)
            {
                yield return SceneManager.UnloadSceneAsync(activeScene);
            }

            yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

            var loadedScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
            SceneManager.SetActiveScene(loadedScene);

            loadingScene = false;
        }

        #endregion
    }
}

using System.Collections;
using NeanderthalTools.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NeanderthalTools.Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private SceneSettings sceneSettings;

        [SerializeField]
        private FadeCanvas fadeCanvas;

        [SerializeField]
        private UnityEvent onSceneReady;

        #endregion

        #region Fields

        private bool loadingScene;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }


#if UNITY_EDITOR
        private IEnumerator Start()
        {
            // Activate all scenes "below" the bootstrap scene, step by step to ensure correct load
            // order.
            var activate = false;
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (activate)
                {
                    yield return new WaitUntil(() => scene.isLoaded);
                    SceneManager.SetActiveScene(scene);
                }
                else if (scene.buildIndex == sceneSettings.BootstrapSceneIndex)
                {
                    activate = true;
                }
            }
        }
#else
        private void Start()
        {
            LoadNextScene();
        }
#endif

        #endregion

        #region Methods

        /// <summary>
        /// Load the next scene from the current active scene.
        /// </summary>
        public void LoadNextScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            var nextIndex = activeScene.buildIndex + 1;

            StartLoadScene(nextIndex);
        }

        /// <summary>
        /// Load the main menu scene.
        /// </summary>
        public void LoadMenuScene()
        {
            StartLoadScene(sceneSettings.MenuSceneIndex);
        }

        /// <summary>
        /// Load the main game scene.
        /// </summary>
        public void LoadMainScene()
        {
            StartLoadScene(sceneSettings.MainSceneIndex);
        }

        /// <summary>
        /// Exit the game into the editor or the OS.
        /// </summary>
        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// Restart currently active scene.
        /// </summary>
        public void RestartScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            StartLoadScene(activeScene.buildIndex);
        }

        #endregion

        #region Private Methods

        private void OnActiveSceneChanged(Scene prev, Scene next)
        {
            if (next.buildIndex != sceneSettings.BootstrapSceneIndex)
            {
                onSceneReady.Invoke();
            }
        }

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
            yield return fadeCanvas.FadeIn();

            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.buildIndex != sceneSettings.BootstrapSceneIndex)
            {
                yield return SceneManager.UnloadSceneAsync(activeScene);
            }

            yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

            var loadedScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
            SceneManager.SetActiveScene(loadedScene);
            LightProbes.TetrahedralizeAsync();

            yield return fadeCanvas.FadeOut();
            loadingScene = false;
        }

        #endregion
    }
}

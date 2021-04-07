using System.Collections;
using System.Collections.Generic;
using NeanderthalTools.Util;
using ScriptableEvents.Simple;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace NeanderthalTools.Logging
{
    public class LoggingHandler : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private LoggingSettings loggingSettings;

        [SerializeField]
        private SimpleScriptableEvent startLoggingEvent;

        [SerializeField]
        private SimpleScriptableEvent stopLoggingEvent;

        #endregion

        #region Fields

        private Scene currentLoggingScene;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            if (currentLoggingScene.IsValid())
            {
                StopLogging();
                if (IsUploadLogs())
                {
                    ImmediateUploadLogs();
                }
            }
        }

        #endregion

        #region Methods

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (!loggingSettings.EnableLogging)
            {
                return;
            }

            if (currentLoggingScene.IsValid() && currentLoggingScene == oldScene)
            {
                StopLogging();
                if (IsUploadLogs())
                {
                    StartUploadLogs();
                }
            }

            if (IsLoggingScene(newScene))
            {
                currentLoggingScene = newScene;
                StartLogging();
            }
        }

        private void StartLogging()
        {
            loggingSettings.LogFileDirectory = $"{Files.DateName()}_{currentLoggingScene.name}";
            startLoggingEvent.Raise();
        }

        private void StopLogging()
        {
            stopLoggingEvent.Raise();
        }

        private bool IsLoggingScene(Scene scene)
        {
            return loggingSettings.LoggingSceneIndexes.Contains(scene.buildIndex);
        }

        private bool IsUploadLogs()
        {
            return loggingSettings.UploadLogsToDropbox;
        }

        private void ImmediateUploadLogs()
        {
            var requests = CreateUploadRequests();
            foreach (var request in requests)
            {
                Log(request);
                var operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                }

                if (IsError(request))
                {
                    LogError(request);
                }
            }
        }

        private void StartUploadLogs()
        {
            StartCoroutine(UploadLogs());
        }

        private IEnumerator UploadLogs()
        {
            var requests = CreateUploadRequests();
            foreach (var request in requests)
            {
                Log(request);
                yield return request.SendWebRequest();

                if (IsError(request))
                {
                    LogError(request);
                }
            }
        }

        private IEnumerable<UnityWebRequest> CreateUploadRequests()
        {
            return Dropbox.CreateUploadRequests(
                loggingSettings.LogFileDirectory,
                $"{loggingSettings.LoggingId}/{loggingSettings.CurrentLogFileDirectory}",
                loggingSettings.DropboxAuthorizationToken
            );
        }

        private static void Log(UnityWebRequest request)
        {
            var pathInfo = Dropbox.GetPathInfo(request);
            Debug.Log($"{request.method} request {request.url} path {pathInfo}");
        }

        private static bool IsError(UnityWebRequest request)
        {
            var result = request.result;
            return result == UnityWebRequest.Result.ConnectionError
                   || result == UnityWebRequest.Result.ProtocolError;
        }

        private void LogError(UnityWebRequest request)
        {
            Debug.LogError($"{request.method} request {request.url} error: {request.error}", this);
            Debug.LogError($"{request.downloadHandler.text}", this);
        }

        #endregion
    }
}

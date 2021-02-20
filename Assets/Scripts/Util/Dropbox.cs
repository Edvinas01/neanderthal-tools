using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace NeanderthalTools.Util
{
    public static class Dropbox
    {
        #region Fields

        private const string UploadUrl = "https://content.dropboxapi.com/2/files/upload";

        #endregion

        #region Methods

        /// <summary>
        /// Uploads all files in given directory to dropbox.
        /// </summary>
        public static void UploadDirectory(
            this Object obj,
            string srcDir,
            string dstDir,
            string token
        )
        {
            var persistentDirectory = Path.Combine(Application.persistentDataPath, srcDir);
            var operations = new List<UnityWebRequestAsyncOperation>();

            foreach (var srcPath in Directory.GetFiles(persistentDirectory))
            {
                var dstPath = $"/{dstDir}/{Path.GetFileName(srcPath)}";
                var request = CreateRequest(srcPath, dstPath, token);

                Debug.Log($"Uploading {srcPath} to {dstPath}", obj);
                operations.Add(request.SendWebRequest());
            }

            WaitCompletion(obj, operations);
        }

        private static UnityWebRequest CreateRequest(string srcPath, string dstPath, string token)
        {
            var request = new UnityWebRequest
            {
                uploadHandler = new UploadHandlerFile(srcPath),
                method = UnityWebRequest.kHttpVerbPOST,
                url = UploadUrl
            };

            request.SetRequestHeader("Authorization", $"Bearer {token}");
            request.SetRequestHeader("Dropbox-API-Arg", "{\"path\": \"" + dstPath + "\"}");
            request.SetRequestHeader("Content-Type", "application/octet-stream");

            return request;
        }

        private static void WaitCompletion(
            Object obj,
            IEnumerable<UnityWebRequestAsyncOperation> operations
        )
        {
            foreach (var operation in operations)
            {
                while (!operation.isDone)
                {
                }

                var request = operation.webRequest;
                if (!string.IsNullOrWhiteSpace(request.error))
                {
                    Debug.LogError(
                        $"{request.method} request {request.url} error: {request.error}",
                        obj
                    );
                }
            }
        }

        #endregion
    }
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace NeanderthalTools.Util
{
    public static class Dropbox
    {
        #region Fields

        private const string PathHeader = "Dropbox-API-Arg";
        private const string UploadUrl = "https://content.dropboxapi.com/2/files/upload";

        #endregion

        #region Methods

        /// <summary>
        /// Creates a list of requests for uploading all files in given directory.
        /// </summary>
        public static List<UnityWebRequest> CreateUploadRequests(
            string srcDir,
            string dstDir,
            string token
        )
        {
            var persistentDirectory = Path.Combine(Application.persistentDataPath, srcDir);
            var requests = new List<UnityWebRequest>();

            foreach (var srcPath in Directory.GetFiles(persistentDirectory))
            {
                var dstPath = $"/{dstDir}/{Path.GetFileName(srcPath)}";
                var request = CreateUploadRequest(srcPath, dstPath, token);

                requests.Add(request);
            }

            return requests;
        }

        /// <returns>
        /// Path header info.
        /// </returns>
        public static string GetPathInfo(UnityWebRequest request)
        {
            return request.GetRequestHeader(PathHeader);
        }

        private static UnityWebRequest CreateUploadRequest(
            string srcPath,
            string dstPath,
            string token
        )
        {
            var request = new UnityWebRequest
            {
                downloadHandler = new DownloadHandlerBuffer(),
                uploadHandler = new UploadHandlerFile(srcPath),
                method = UnityWebRequest.kHttpVerbPOST,
                url = UploadUrl
            };

            request.SetRequestHeader("Authorization", $"Bearer {token}");
            request.SetRequestHeader(PathHeader, "{\"path\": \"" + dstPath + "\"}");
            request.SetRequestHeader("Content-Type", "application/octet-stream");

            return request;
        }

        #endregion
    }
}

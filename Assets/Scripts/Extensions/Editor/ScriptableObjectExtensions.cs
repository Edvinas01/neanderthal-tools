using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NeanderthalTools.Extensions.Editor
{
    public static class ScriptableObjectExtensions
    {
        /// <returns>
        /// New ScriptableObject asset at given path or an existing one of given type.
        /// </returns>
        public static T FindOrCreateAsset<T>(string path) where T : ScriptableObject
        {
            var asset = FindAsset<T>();
            return asset != null
                ? asset
                : CreateAsset<T>(path);
        }

        private static T FindAsset<T>() where T : ScriptableObject
        {
            return AssetDatabase
                .FindAssets($"t:{typeof(T).Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .FirstOrDefault();
        }

        private static T CreateAsset<T>(string path) where T : ScriptableObject
        {
            var directoryName = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directoryName ?? string.Empty);

            var scriptableObject = ScriptableObject.CreateInstance<T>();
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(scriptableObject, uniquePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return scriptableObject;
        }
    }
}

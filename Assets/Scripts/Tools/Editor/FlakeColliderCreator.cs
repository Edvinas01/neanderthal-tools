using UnityEditor;
using UnityEngine;

namespace NeanderthalTools.Tools.Editor
{
    public class FlakeColliderCreator : EditorWindow
    {
        #region Unity Lifecycle

        [MenuItem("GameObject/Create Flake Collider")]
        private static void CreateCollider(MenuCommand menuCommand)
        {
            var gameObject = menuCommand.context as GameObject;
            var mesh = GetSharedMesh(gameObject);

            if (mesh == null)
            {
                return;
            }

            CreateCollider(gameObject, mesh);
        }

        [MenuItem("GameObject/Create Flake Collider", true)]
        private static bool IsCreateCollider()
        {
            return GetSharedMesh(Selection.activeObject as GameObject) != null;
        }

        #endregion

        #region Methods

        private static Mesh GetSharedMesh(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return null;
            }

            var meshFilter = gameObject.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                return null;
            }

            return meshFilter.sharedMesh;
        }

        private static void CreateCollider(GameObject parentGameObject, Mesh mesh)
        {
            var parentTransform = parentGameObject.transform;
            var gameObject = new GameObject
            {
                transform =
                {
                    position = parentTransform.position,
                    rotation = parentTransform.rotation,
                    parent = parentTransform
                },
                layer = LayerMask.NameToLayer("Interactable"),
                name = "Collider"
            };

            var meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            meshCollider.convex = true;
        }

        #endregion
    }
}

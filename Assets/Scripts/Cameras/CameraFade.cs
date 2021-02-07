using System;
using UnityEditor;
using UnityEngine;

namespace NeanderthalTools.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class CameraFade : MonoBehaviour
    {
        private Camera camera;

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }
    }
}

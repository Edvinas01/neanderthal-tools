using System;
using OpenXRRuntimeJsons;
using UnityEngine;
using UnityEngine.Events;

namespace NeanderthalTools.Triggers
{
    public class XRRuntimeTrigger : MonoBehaviour
    {
        #region Editor

        [Header("Runtimes")]
        [SerializeField]
        private string oculusPathName = "Oculus";

        [SerializeField]
        private string steamPathName = "Steam";

        [Header("Events")]
        [SerializeField]
        private UnityEvent onOculus;

        [SerializeField]
        private UnityEvent onSteam;

        [SerializeField]
        private UnityEvent onNone;

        #endregion

        #region Unity Lifecycle

        private void Start()
        {
            if (IsRuntime(oculusPathName))
            {
                onOculus.Invoke();
            }
            else if (IsRuntime(steamPathName))
            {
                onSteam.Invoke();
            }
            else
            {
                onNone.Invoke();
            }
        }

        #endregion

        #region Methods

        private static bool IsRuntime(string runtimeName)
        {
            var path = GetRuntimePath();
            var result = path.IndexOf(
                runtimeName,
                0,
                StringComparison.CurrentCultureIgnoreCase
            );

            return result != -1;
        }

        private static string GetRuntimePath()
        {
            var runtimes = OpenXRRuntimeJson.GetRuntimeJsonPaths();
            if (runtimes.TryGetValue(OpenXRRuntimeType.EnvironmentVariable, out var envPath))
            {
                return envPath;
            }

            if (runtimes.TryGetValue(OpenXRRuntimeType.SystemDefault, out var defaultPath))
            {
                return defaultPath;
            }

            return "";
        }

        #endregion
    }
}

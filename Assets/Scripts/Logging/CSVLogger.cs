using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NeanderthalTools.Logging
{
    public class CsvLogger : MonoBehaviour, ILogger
    {
        #region Editor

        [SerializeField]
        private LoggableCollection loggables;

        [Min(0f)]
        [SerializeField]
        private float sampleRateSeconds = 1f;

        #endregion

        #region Fields

        private readonly List<object> snapshot = new List<object>();
        private float nextLogSeconds;

        #endregion

        #region Unity Lifecycle

        private void Update()
        {
            if (Time.time < nextLogSeconds)
            {
                return;
            }

            snapshot.Clear();
            foreach (var loggable in loggables)
            {
                loggable.Accept(this);
            }

            var str = snapshot
                .Select(obj => obj.ToString())
                .Aggregate((a, b) => a + " " + b);

            Debug.Log(str);

            nextLogSeconds = Time.time + sampleRateSeconds;
        }

        #endregion

        #region Overrides

        public void Log(object obj)
        {
            snapshot.Add(obj);
        }

        #endregion
    }
}

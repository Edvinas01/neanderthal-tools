using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    [Serializable]
    public class PoseData
    {
        #region Fields

        [SerializeField]
        private bool isHeatmap = true;

        [SerializeField]
        private bool isDraw = true;

        [SerializeField]
        private string name;

        [SerializeField]
        private List<Vector3> positions;

        #endregion

        #region Properties

        public bool IsHeatmap
        {
            get => isHeatmap;
            set => isHeatmap = value;
        }

        public bool IsDraw
        {
            get => isDraw;
            set => isDraw = value;
        }

        public string Name => name;

        public List<Vector3> Positions => positions;

        #endregion

        #region Methods

        public PoseData(string name, List<Vector3> positions)
        {
            this.name = name;
            this.positions = positions;
        }

        #endregion
    }
}

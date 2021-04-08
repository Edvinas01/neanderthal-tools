using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    public class PoseData
    {
        #region Properties

        public bool IsDraw { get; set; } = true;

        public string Name { get; }

        public List<Vector3> Positions { get; }

        #endregion

        #region Methods

        public PoseData(string name, List<Vector3> positions)
        {
            Name = name;
            Positions = positions;
        }

        #endregion
    }
}

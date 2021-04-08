using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    [CreateAssetMenu(
        fileName = "LogVisualizerSettings",
        menuName = "Game/Log Visualizer Settings"
    )]
    public class LogVisualizerSettings : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private List<UserData> users = new List<UserData>();

        [SerializeField]
        private Material heatmapMaterial;

        [SerializeField]
        private Mesh heatmapMesh;

        [SerializeField]
        private bool isDrawUserPositions = true;

        [SerializeField]
        private bool isDrawHeatmap = true;

        [SerializeField]
        private float heatmapCellMaxScale = 2f;

        [SerializeField]
        private float heatmapCellSize = 0.5f;

        [SerializeField]
        private string heatmapColorProperty = "_BaseColor";

        [SerializeField]
        private Color heatmapFromColor = Color.green;

        [SerializeField]
        private Color heatmapToColor = Color.red;

        #endregion

        #region Properties

        public List<UserData> Users => users;

        public Material HeatmapMaterial
        {
            get => heatmapMaterial;
            set => heatmapMaterial = value;
        }

        public Mesh HeatmapMesh
        {
            get => heatmapMesh;
            set => heatmapMesh = value;
        }

        public bool IsDrawUserPositions
        {
            get => isDrawUserPositions;
            set => isDrawUserPositions = value;
        }

        public bool IsDrawHeatmap
        {
            get => isDrawHeatmap;
            set => isDrawHeatmap = value;
        }

        public float HeatmapCellMaxScale
        {
            get => heatmapCellMaxScale;
            set => heatmapCellMaxScale = value;
        }

        public float HeatmapCellSize
        {
            get => heatmapCellSize;
            set => heatmapCellSize = value;
        }

        public string HeatmapColorProperty
        {
            get => heatmapColorProperty;
            set => heatmapColorProperty = value;
        }

        public Color HeatmapFromColor
        {
            get => heatmapFromColor;
            set => heatmapFromColor = value;
        }

        public Color HeatmapToColor
        {
            get => heatmapToColor;
            set => heatmapToColor = value;
        }

        #endregion
    }
}

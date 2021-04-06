using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers.Session
{
    [Serializable]
    public class SessionData
    {
        #region Fields

        [SerializeField]
        private string loggingId;

        [SerializeField]
        private int teleportCount;

        [SerializeField]
        private int snapTurnCount;

        [SerializeField]
        private List<HitData> dependenciesRemainingHits = new List<HitData>();

        [SerializeField]
        private List<HitData> invalidAngleHits = new List<HitData>();

        [SerializeField]
        private List<HitData> weakHits = new List<HitData>();

        [SerializeField]
        private List<AttachData> attachedAdhesives = new List<AttachData>();

        [SerializeField]
        private List<AttachData> attachedFlakes = new List<AttachData>();

        [SerializeField]
        private List<string> consumedAdhesives = new List<string>();

        [SerializeField]
        private List<PickupData> pickups = new List<PickupData>();

        [SerializeField]
        private List<StateData> states = new List<StateData>();

        #endregion

        #region Properties

        public string LoggingId
        {
            get => loggingId;
            set => loggingId = value;
        }

        public int TeleportCount
        {
            get => teleportCount;
            set => teleportCount = value;
        }

        public int SnapTurnCount
        {
            get => snapTurnCount;
            set => snapTurnCount = value;
        }

        public List<HitData> DependenciesRemainingHits => dependenciesRemainingHits;

        public List<HitData> InvalidAngleHits => invalidAngleHits;

        public List<HitData> WeakHits => weakHits;

        public List<AttachData> AttachedAdhesives => attachedAdhesives;

        public List<AttachData> AttachedFlakes => attachedFlakes;

        public List<string> ConsumedAdhesives => consumedAdhesives;

        public List<PickupData> Pickups => pickups;

        public List<StateData> States => states;

        #endregion
    }
}

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
        private float startTime;

        [SerializeField]
        private float endTime;

        [SerializeField]
        private List<LocomotionData> teleports = new List<LocomotionData>();

        [SerializeField]
        private List<LocomotionData> snapTurns = new List<LocomotionData>();

        [SerializeField]
        private List<HitData> dependenciesRemainingHits = new List<HitData>();

        [SerializeField]
        private List<HitData> invalidAngleHits = new List<HitData>();

        [SerializeField]
        private List<HitData> weakHits = new List<HitData>();

        [SerializeField]
        private List<HitData> removals = new List<HitData>();

        [SerializeField]
        private List<AttachData> attachedAdhesives = new List<AttachData>();

        [SerializeField]
        private List<AttachData> attachedFlakes = new List<AttachData>();

        [SerializeField]
        private List<ConsumeData> consumedAdhesives = new List<ConsumeData>();

        [SerializeField]
        private List<GrabData> grabs = new List<GrabData>();

        [SerializeField]
        private List<StateData> states = new List<StateData>();

        #endregion

        #region Properties

        public string LoggingId
        {
            get => loggingId;
            set => loggingId = value;
        }

        public float StartTime
        {
            get => startTime;
            set => startTime = value;
        }

        public float EndTime
        {
            get => endTime;
            set => endTime = value;
        }

        public List<LocomotionData> Teleports
        {
            get => teleports;
            set => teleports = value;
        }

        public List<LocomotionData> SnapTurns
        {
            get => snapTurns;
            set => snapTurns = value;
        }

        public List<HitData> DependenciesRemainingHits => dependenciesRemainingHits;

        public List<HitData> InvalidAngleHits => invalidAngleHits;

        public List<HitData> WeakHits => weakHits;

        public List<HitData> Removals => removals;

        public List<AttachData> AttachedAdhesives => attachedAdhesives;

        public List<AttachData> AttachedFlakes => attachedFlakes;

        public List<ConsumeData> ConsumedAdhesives => consumedAdhesives;

        public List<GrabData> Grabs => grabs;

        public List<StateData> States => states;

        #endregion
    }
}

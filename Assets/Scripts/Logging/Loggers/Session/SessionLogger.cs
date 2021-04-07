using System.Linq;
using NeanderthalTools.Locomotion;
using NeanderthalTools.Logging.Writers;
using NeanderthalTools.States;
using NeanderthalTools.ToolCrafting.Hafting;
using NeanderthalTools.ToolCrafting.Knapping;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Logging.Loggers.Session
{
    // todo - consider logging positions and expand hand data (e.g. which object is held).
    public class SessionLogger : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private LogWriterProvider logWriterProvider;

        [SerializeField]
        private LoggingSettings loggingSettings;

        #endregion

        #region Fields

        private ILogWriter logWriter;
        private SessionData sessionData;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            SetupSessionData();
            SetupLogWriter();
        }

        private void OnDisable()
        {
            CleanupLogWriter();
        }

        #endregion

        #region Methods

        public void LogTeleport(LocomotionEventArgs args)
        {
            sessionData.TeleportCount++;
        }

        public void LogSnapTurn(LocomotionEventArgs args)
        {
            sessionData.SnapTurnCount++;
        }

        public void LogDependenciesRemaining(FlakeEventArgs args)
        {
            var data = CreateHitData(args);
            sessionData.DependenciesRemainingHits.Add(data);
        }

        public void LogInvalidAngleHit(FlakeEventArgs args)
        {
            var data = CreateHitData(args);
            sessionData.InvalidAngleHits.Add(data);
        }

        public void LogWeakHit(FlakeEventArgs args)
        {
            var data = CreateHitData(args);
            sessionData.WeakHits.Add(data);
        }

        public void LogAttachAdhesive(HaftEventArgs args)
        {
            var data = CreateAttachData(args);
            sessionData.AttachedAdhesives.Add(data);
        }

        public void LogAttachFlake(HaftEventArgs args)
        {
            var data = CreateAttachData(args);
            sessionData.AttachedFlakes.Add(data);
        }

        public void LogConsumeAdhesive(AdhesiveEventArgs args)
        {
            var rawAdhesiveName = args.RawAdhesive.name;
            sessionData.ConsumedAdhesives.Add(rawAdhesiveName);
        }

        public void LogPickup(SelectEnterEventArgs args)
        {
            var data = CreatePickupData(args);
            sessionData.Pickups.Add(data);
        }

        public void LogEnterState(StateEventArgs args)
        {
            var stateName = args.State.name;
            var stateData = FindOrCreateStateData(stateName);
            stateData.StartTime = Time.time;

            Debug.Log("enter: " + stateName);
        }

        public void LogExitState(StateEventArgs args)
        {
            var stateName = args.State.name;
            var stateData = FindOrCreateStateData(stateName);
            stateData.EndTime = Time.time;
            
            Debug.Log("exit: " + stateName);
        }

        private void SetupLogWriter()
        {
            logWriter = logWriterProvider.CreateLogWriter(name);
            logWriter.Start();
        }

        private void SetupSessionData()
        {
            sessionData = new SessionData
            {
                LoggingId = loggingSettings.LoggingId
            };
        }

        private void CleanupLogWriter()
        {
            logWriter.Write(sessionData);
            logWriter.Close();
            logWriter = null;
        }

        private static HitData CreateHitData(FlakeEventArgs args)
        {
            var objective = args.Objective;
            var flake = args.Flake;

            return new HitData
            {
                ObjectiveName = objective.name,
                KnapperName = args.KnapperInteractor.name,
                FlakeName = flake.name,
                Time = Time.time
            };
        }

        private static AttachData CreateAttachData(HaftEventArgs args)
        {
            return new AttachData
            {
                ToolPartName = args.ToolPart.Name,
                HandleName = args.HandleInteractable.name,
                Time = Time.time
            };
        }

        private static PickupData CreatePickupData(SelectEnterEventArgs args)
        {
            return new PickupData
            {
                TargetName = args.interactable.name,
                HandName = args.interactor.name
            };
        }

        private StateData FindOrCreateStateData(string stateName)
        {
            var data = sessionData.States.FirstOrDefault(state =>
                state.StateName == stateName
            );

            if (data != null)
            {
                return data;
            }

            var newData = new StateData
            {
                StateName = stateName
            };

            sessionData.States.Add(newData);

            return newData;
        }

        #endregion
    }
}

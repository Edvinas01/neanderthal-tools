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
    // 1. Zip json based on boolean flag
    // 3. Test & document
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
            sessionData.EndTime = Time.time;
            CleanupLogWriter();
        }

        #endregion

        #region Methods

        public void LogTeleport(LocomotionEventArgs args)
        {
            var data = CreateLocomotionData(args);
            sessionData.Teleports.Add(data);
        }

        public void LogSnapTurn(LocomotionEventArgs args)
        {
            var data = CreateLocomotionData(args);
            sessionData.SnapTurns.Add(data);
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

        public void LogDetach(FlakeEventArgs args)
        {
            var data = CreateHitData(args);
            sessionData.Removals.Add(data);
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
            var data = CreateConsumeData(args);
            sessionData.ConsumedAdhesives.Add(data);
        }

        public void LogGrab(BaseInteractionEventArgs args)
        {
            var data = CreateGrabData(args);
            sessionData.Grabs.Add(data);
        }

        public void LogEnterState(StateEventArgs args)
        {
            var stateName = args.State.name;
            var data = FindOrCreateStateData(stateName);
            data.StartTime = Time.time;
        }

        public void LogExitState(StateEventArgs args)
        {
            var stateName = args.State.name;
            var data = FindOrCreateStateData(stateName);
            data.EndTime = Time.time;
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
                LoggingId = loggingSettings.LoggingId,
                StartTime = Time.time
            };
        }

        private void CleanupLogWriter()
        {
            logWriter.Write(sessionData);
            logWriter.Close();
            logWriter = null;
        }

        private static LocomotionData CreateLocomotionData(LocomotionEventArgs args)
        {
            var xrRig = args.LocomotionSystem.xrRig;

            var cameraTransform = xrRig.cameraGameObject.transform;
            var xrRigTransform = xrRig.transform;

            return new LocomotionData
            {
                CameraPosition = cameraTransform.position,
                CameraRotation = cameraTransform.eulerAngles,
                RigPosition = xrRigTransform.position,
                RigRotation = xrRigTransform.eulerAngles,
                Time = Time.time
            };
        }

        private static HitData CreateHitData(FlakeEventArgs args)
        {
            var objectiveInteractor = args.ObjectiveInteractor;
            var knapperInteractor = args.KnapperInteractor;
            var objective = args.Objective;
            var flake = args.Flake;

            return new HitData
            {
                // Can be hit without being held.
                ObjectiveHandName = objectiveInteractor == null ? null : objectiveInteractor.name,
                KnapperHandName = knapperInteractor.name,
                ObjectiveName = objective.name,
                KnapperName = knapperInteractor.selectTarget.name,
                FlakeName = flake.name,
                ImpactForce = args.ImpactForce,
                Time = Time.time
            };
        }

        private static AttachData CreateAttachData(HaftEventArgs args)
        {
            var toolPartInteractor = args.ToolPartInteractor;
            var handleInteractor = args.HandleInteractor;

            return new AttachData
            {
                // Can be attached without holding the tool part (e.g. tar).
                ToolPartHandName = toolPartInteractor == null ? null : toolPartInteractor.name,
                HandleHandName = handleInteractor.name,
                ToolPartName = args.ToolPart.Name,
                HandleName = handleInteractor.selectTarget.name,
                Time = Time.time
            };
        }

        private static ConsumeData CreateConsumeData(AdhesiveEventArgs args)
        {
            return new ConsumeData
            {
                AdhesiveName = args.RawAdhesive.name,
                Time = Time.time
            };
        }

        private static GrabData CreateGrabData(BaseInteractionEventArgs args)
        {
            return new GrabData
            {
                TargetName = args.interactable.name,
                HandName = args.interactor.name,
                Time = Time.time
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

using System.Collections.Generic;
using NeanderthalTools.Logging.Writers;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers
{
    public class PoseLogger : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private LogWriterProvider logWriterProvider;

        [SerializeField]
        private PoseLoggerSettings poseLoggerSettings;

        [SerializeField]
        private List<Transform> poses;

        #endregion

        #region Fields

        private ILogWriter logWriter;
        private float nextSampleTime;

        private object[] batch;
        private int batchIndex;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            SetupBatch();
            SetupLogWriter();
            LogMeta();
        }

        private void OnDisable()
        {
            CleanupBatch();
            CleanupLogWriter();
        }

        private void Update()
        {
            if (!IsSample())
            {
                return;
            }

            UpdateNextSampleTime();
            LogPoses();
        }

        #endregion

        #region Methods

        private void SetupLogWriter()
        {
            logWriter = logWriterProvider.CreateLogWriter(name);
            logWriter.Start();
        }

        private void SetupBatch()
        {
            // Time + PoseCount * (Position, Rotation)
            batch = new object[1 + poses.Count * 6];
            batchIndex = 0;
        }

        private void CleanupBatch()
        {
            batch = null;
            batchIndex = 0;
        }

        private void CleanupLogWriter()
        {
            logWriter.Close();
            logWriter = null;
        }

        private void LogMeta()
        {
            Log("Time");
            foreach (var pose in poses)
            {
                var poseName = pose.name;
                Log($"{poseName}_PositionX");
                Log($"{poseName}_PositionY");
                Log($"{poseName}_PositionZ");
                Log($"{poseName}_RotationX");
                Log($"{poseName}_RotationY");
                Log($"{poseName}_RotationZ");
            }

            FlushLogs();
        }

        private bool IsSample()
        {
            return Time.time >= nextSampleTime;
        }

        private void UpdateNextSampleTime()
        {
            nextSampleTime = Time.time + poseLoggerSettings.SampleInterval;
        }

        private void LogPoses()
        {
            Log(Time.time);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < poses.Count; index++)
            {
                var pose = poses[index];
                LogPose(pose);
            }

            FlushLogs();
        }

        private void LogPose(Transform pose)
        {
            var position = pose.position;
            Log(position.x);
            Log(position.y);
            Log(position.z);

            var rotation = pose.rotation.eulerAngles;
            Log(rotation.x);
            Log(rotation.y);
            Log(rotation.z);
        }

        private void Log(object obj)
        {
            batch[batchIndex++] = obj;
        }

        private void FlushLogs()
        {
            logWriter.Write(batch);
            batchIndex = 0;
        }

        #endregion
    }
}

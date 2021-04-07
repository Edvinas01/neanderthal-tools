using System;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Locomotion
{
    [Serializable]
    public class LocomotionUnityEvent : UnityEvent<LocomotionEventArgs>
    {
    }
}

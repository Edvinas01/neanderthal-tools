using System;
using UnityEngine.Events;

namespace NeanderthalTools.States
{
    [Serializable]
    public class StateUnityEvent : UnityEvent<StateEventArgs>
    {
    }
}

using System;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Hands
{
    [Serializable]
    public class InteractionUnityEvent : UnityEvent<BaseInteractionEventArgs>
    {
    }
}

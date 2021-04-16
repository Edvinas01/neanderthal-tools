using System;
using UnityEngine;
using UnityEngine.Events;

namespace NeanderthalTools.Triggers
{
    [Serializable]
    public class CollisionUnityEvent : UnityEvent<Collision>
    {
    }
}

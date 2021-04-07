using ScriptableEvents;
using UnityEngine;

namespace NeanderthalTools.Locomotion
{
    [CreateAssetMenu(
        fileName = "LocomotionScriptableEvent",
        menuName = "Game/Scriptable Events/Locomotion Scriptable Event"
    )]
    public class LocomotionScriptableEvent : BaseScriptableEvent<LocomotionEventArgs>
    {
    }
}

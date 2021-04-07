using ScriptableEvents;
using UnityEngine;

namespace NeanderthalTools.States
{
    [CreateAssetMenu(
        fileName = "StateScriptableEvent",
        menuName = "Game/Scriptable Events/State Scriptable Event"
    )]
    public class StateScriptableEvent : BaseScriptableEvent<StateEventArgs>
    {
    }
}

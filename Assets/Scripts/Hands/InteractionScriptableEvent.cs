using ScriptableEvents;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Hands
{
    [CreateAssetMenu(
        fileName = "InteractionScriptableEvent",
        menuName = "Game/Scriptable Events/Interaction Scriptable Event"
    )]
    public class InteractionScriptableEvent : BaseScriptableEvent<BaseInteractionEventArgs>
    {
    }
}

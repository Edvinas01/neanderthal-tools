using ScriptableEvents;
using UnityEngine;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    [CreateAssetMenu(
        fileName = "AdhesiveScriptableEvent",
        menuName = "Game/Scriptable Events/Adhesive Scriptable Event"
    )]
    public class AdhesiveScriptableEvent : BaseScriptableEvent<AdhesiveEventArgs>
    {
    }
}

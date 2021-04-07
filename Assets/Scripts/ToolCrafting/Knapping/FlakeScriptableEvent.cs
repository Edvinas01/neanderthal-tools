using ScriptableEvents;
using UnityEngine;

namespace NeanderthalTools.ToolCrafting.Knapping
{
    [CreateAssetMenu(
        fileName = "FlakeScriptableEvent",
        menuName = "Game/Scriptable Events/Flake Scriptable Event"
    )]
    public class FlakeScriptableEvent : BaseScriptableEvent<FlakeEventArgs>
    {
    }
}

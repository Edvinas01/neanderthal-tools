using ScriptableEvents;
using UnityEngine;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    [CreateAssetMenu(
        fileName = "HaftScriptableEvent",
        menuName = "Game/Scriptable Events/Haft Scriptable Event"
    )]
    public class HaftScriptableEvent : BaseScriptableEvent<HaftEventArgs>
    {
    }
}

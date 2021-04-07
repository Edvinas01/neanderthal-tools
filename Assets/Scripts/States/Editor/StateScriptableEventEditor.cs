using ScriptableEvents.Editor;
using UnityEditor;

namespace NeanderthalTools.States.Editor
{
    [CustomEditor(typeof(StateScriptableEvent))]
    public class StateScriptableEventEditor : BaseScriptableEventEditor<StateEventArgs>
    {
        protected override StateEventArgs DrawArgField(StateEventArgs value)
        {
            if (value == null)
            {
                value = new StateEventArgs(null);
            }

            EditorGUILayout.BeginVertical();
            var state = (State) EditorGUILayout.ObjectField(value.State, typeof(State), true);
            EditorGUILayout.EndVertical();

            return new StateEventArgs(state);
        }
    }
}

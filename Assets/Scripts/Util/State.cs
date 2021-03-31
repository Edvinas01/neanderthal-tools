using ScriptableEvents;
using ScriptableEvents.Simple;
using UnityEngine;
using UnityEngine.Events;

namespace NeanderthalTools.Util
{
    public class State : MonoBehaviour, IScriptableEventListener<SimpleArg>
    {
        #region Editor

        [SerializeField]
        private SimpleScriptableEvent nextStateTrigger;

        [SerializeField]
        private State nextState;

        [SerializeField]
        private UnityEvent onEnter;

        [SerializeField]
        private UnityEvent onExit;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            Debug.Log("entered " + name); // todo remove
            StartState();

            if (nextStateTrigger == null)
            {
                return;
            }

            nextStateTrigger.Add(this);
        }

        private void OnDisable()
        {
            Debug.Log("exited " + name); // todo remove
            StartNextState();

            if (nextStateTrigger == null)
            {
                return;
            }

            nextStateTrigger.Remove(this);
        }

        #endregion

        #region Methods

        public void OnRaised(SimpleArg arg)
        {
            gameObject.SetActive(false);
        }

        private void StartState()
        {
            onEnter.Invoke();
        }

        private void StartNextState()
        {
            onExit.Invoke();
            if (nextState != null)
            {
                nextState.gameObject.SetActive(true);
            }

            gameObject.SetActive(false);
        }

        #endregion
    }
}

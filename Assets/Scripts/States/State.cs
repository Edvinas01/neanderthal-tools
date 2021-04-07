using System.Collections;
using ScriptableEvents;
using ScriptableEvents.Simple;
using UnityEngine;

namespace NeanderthalTools.States
{
    public class State : MonoBehaviour, IScriptableEventListener<SimpleArg>
    {
        #region Editor

        [SerializeField]
        private SimpleScriptableEvent nextStateTrigger;

        [SerializeField]
        [Tooltip("The next state in line")]
        private State nextState;

        [SerializeField]
        [Tooltip("Should this state trigger on game start")]
        private bool triggerOnStart;

        [Min(0f)]
        [SerializeField]
        [Tooltip("State start delay in seconds")]
        private float startDelay = 2f;

        [SerializeField]
        private StateUnityEvent onEnter;

        [SerializeField]
        private StateUnityEvent onExit;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            if (nextStateTrigger != null)
            {
                nextStateTrigger.Add(this);
            }
        }

        private void OnDisable()
        {
            if (nextStateTrigger != null)
            {
                nextStateTrigger.Remove(this);
            }
        }

        private void Start()
        {
            if (triggerOnStart)
            {
                StartState();
            }
        }

        #endregion

        #region Methods

        public void OnRaised(SimpleArg arg)
        {
            StartNextState();
        }

        public void StartState()
        {
            StartCoroutine(StartStateDelayed());
        }

        public void StartNextState()
        {
            StopAllCoroutines();

            onExit.Invoke(CreateStateEventArgs());

            if (nextState != null)
            {
                nextState.gameObject.SetActive(true);
                nextState.StartState();
            }

            gameObject.SetActive(false);
        }

        private IEnumerator StartStateDelayed()
        {
            yield return new WaitForSeconds(startDelay);
            onEnter.Invoke(CreateStateEventArgs());
        }

        private StateEventArgs CreateStateEventArgs()
        {
            return new StateEventArgs(this);
        }

        #endregion
    }
}

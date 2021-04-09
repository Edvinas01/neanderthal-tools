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

        #region Fields

        private Coroutine startCoroutine;
        private Coroutine startNextStateCoroutine;
        private bool started;

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

            StopAllCoroutines();
            startCoroutine = null;
            startNextStateCoroutine = null;
            started = false;
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
            if (IsStateStarting())
            {
                return;
            }

            startCoroutine = StartCoroutine(StartStateDelayed());
        }

        public void StartNextState()
        {
            if (IsNextStateStarting())
            {
                return;
            }

            startNextStateCoroutine = StartCoroutine(StartNextStateDelayed());
        }

        private IEnumerator StartStateDelayed()
        {
            yield return new WaitForSeconds(startDelay);
            onEnter.Invoke(CreateStateEventArgs());
            started = true;
        }

        private IEnumerator StartNextStateDelayed()
        {
            // Next state coroutines are allowed to fire immediately without waiting for the state
            // to actually start. This is useful as it solves problems related to states firing out
            // of order. Due to this, we have to wait until the state has actually started before
            // we can continue.
            yield return WaitForStart();

            onExit.Invoke(CreateStateEventArgs());

            if (nextState != null)
            {
                nextState.gameObject.SetActive(true);
                nextState.StartState();
            }

            gameObject.SetActive(false);
        }

        private StateEventArgs CreateStateEventArgs()
        {
            return new StateEventArgs(this);
        }

        private bool IsStateStarting()
        {
            return startCoroutine != null;
        }

        private bool IsNextStateStarting()
        {
            return startNextStateCoroutine != null;
        }

        private IEnumerator WaitForStart()
        {
            yield return new WaitUntil(() => started);
        }

        #endregion
    }
}

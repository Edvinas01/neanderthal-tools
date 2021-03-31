using ScriptableEvents;
using ScriptableEvents.Simple;
using UnityEngine;
using UnityEngine.Events;

namespace NeanderthalTools.Triggers
{
    public class ScriptableEventLatchTrigger : MonoBehaviour, IScriptableEventListener<SimpleArg>
    {
        #region Editor

        [SerializeField]
        private SimpleScriptableEvent openTrigger;

        [SerializeField]
        private SimpleScriptableEvent closeTrigger;

        [SerializeField]
        private UnityEvent onOpen;

        [SerializeField]
        private UnityEvent onClose;

        #endregion

        #region Fields

        private bool closed = true;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            // Closer is not added, as it will be added in OnRaised when the latch is opened for
            // the first time.
            openTrigger.Add(this);
        }

        private void OnDisable()
        {
            openTrigger.Remove(this);
            closeTrigger.Remove(this);
        }

        #endregion

        #region Methods

        public void OnRaised(SimpleArg arg)
        {
            if (closed)
            {
                // Opening.
                openTrigger.Remove(this);
                closeTrigger.Add(this);
                onOpen.Invoke();
            }
            else
            {
                // Closing.
                openTrigger.Add(this);
                closeTrigger.Remove(this);
                onClose.Invoke();
            }

            closed = !closed;
        }

        #endregion
    }
}

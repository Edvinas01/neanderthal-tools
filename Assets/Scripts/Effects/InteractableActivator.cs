using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NeanderthalTools.Util;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Effects
{
    public class InteractableActivator : MonoBehaviour
    {
        #region Helpers

        private class ChildInteractable
        {
            public XRBaseInteractable Interactable { get; }

            public Rigidbody Rigidbody { get; }

            public Vector3 InitialScale { get; }

            public ChildInteractable(
                XRBaseInteractable interactable,
                Rigidbody rigidbody,
                Vector3 initialScale
            )
            {
                Interactable = interactable;
                Rigidbody = rigidbody;
                InitialScale = initialScale;
            }
        }

        #endregion

        #region Editor

        [SerializeField]
        private float duration = 0.5f;

        #endregion

        #region Fields

        private List<ChildInteractable> childInteractables;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            childInteractables = GetInteractables();
            DeactivateChildren();
        }

        #endregion

        #region Methods

        public void ActivateChildren()
        {
            foreach (var child in childInteractables)
            {
                var interactable = child.Interactable;
                var body = child.Rigidbody;

                interactable.gameObject.SetActive(true);

                // No grabbing up while scaling >:(
                interactable.enabled = false;

                // Stay in-air while scaling.
                body.isKinematic = true;
            }

            StartCoroutine(ScaleUp());
        }

        private List<ChildInteractable> GetInteractables()
        {
            return GetComponentsInChildren<XRBaseInteractable>(true)
                .Select(interactable =>
                {
                    var body = interactable.GetComponent<Rigidbody>();
                    if (body == null)
                    {
                        return null;
                    }

                    return new ChildInteractable(
                        interactable,
                        body,
                        interactable.transform.localScale
                    );
                })
                .Where(interactable => interactable != null)
                .ToList();
        }

        private void DeactivateChildren()
        {
            foreach (var child in childInteractables)
            {
                child.Interactable.gameObject.SetActive(false);
            }
        }

        private IEnumerator ScaleUp()
        {
            yield return Coroutines.Progress(0f, 1f, duration, SetScale);

            foreach (var child in childInteractables)
            {
                var interactable = child.Interactable;
                var body = child.Rigidbody;

                // Allow grabbing.
                interactable.enabled = true;

                // Scaled up, drop to the ground.
                body.isKinematic = false;
            }
        }

        private void SetScale(float scaleProgress)
        {
            for (var index = childInteractables.Count - 1; index >= 0; index--)
            {
                var child = childInteractables[index];
                var interactableTransform = child.Interactable.transform;

                interactableTransform.localScale = child.InitialScale * scaleProgress;
            }
        }

        #endregion
    }
}

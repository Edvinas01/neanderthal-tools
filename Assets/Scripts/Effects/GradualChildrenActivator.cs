using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Effects
{
    public class GradualChildrenActivator : MonoBehaviour
    {
        #region Fields

        private readonly List<Transform> children = new List<Transform>();

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            var activatorTransform = transform;
            var childCount = activatorTransform.childCount;

            for (var i = 0; i < childCount; i++)
            {
                var child = activatorTransform.GetChild(i);
                children.Add(child);
            }

            DeactivateChildren();
        }

        #endregion

        #region Methods

        public void ActivateChildren()
        {
            ActivateChildren(true);
        }

        public void DeactivateChildren()
        {
            ActivateChildren(false);
        }

        private void ActivateChildren(bool active)
        {
            foreach (var child in children)
            {
                child.gameObject.SetActive(active);
            }
        }

        #endregion
    }
}

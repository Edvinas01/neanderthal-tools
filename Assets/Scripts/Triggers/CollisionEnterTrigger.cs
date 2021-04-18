using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace NeanderthalTools.Triggers
{
    public class CollisionEnterTrigger : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private CollisionUnityEvent onCollision;

        [SerializeField]
        private List<string> componentBlacklistTypeNames;

        #endregion

        #region Fields

        private readonly List<Type> componentBlacklistTypes = new List<Type>();

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            SetupBlacklistTypes();
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (IsBlacklisted(collision))
            {
                return;
            }

            onCollision.Invoke(collision);
        }

        #endregion

        #region Methods

        private void SetupBlacklistTypes()
        {
            foreach (var componentBlacklistTypeName in componentBlacklistTypeNames)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var type = assembly.GetType(componentBlacklistTypeName);
                if (type == null)
                {
                    Debug.LogError($"Could not get type: {componentBlacklistTypeName}", this);
                    continue;
                }

                componentBlacklistTypes.Add(type);
            }
        }

        private bool IsBlacklisted(Collision collision)
        {
            foreach (var componentBlacklistType in componentBlacklistTypes)
            {
                var otherRigidbody = collision.rigidbody;
                if (otherRigidbody == null)
                {
                    continue;
                }

                var component = otherRigidbody.GetComponent(componentBlacklistType);
                if (component != null)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}

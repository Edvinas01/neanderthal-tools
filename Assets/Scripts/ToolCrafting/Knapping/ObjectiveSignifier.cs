using System.Linq;
using NeanderthalTools.Util;
using UnityEngine;

namespace NeanderthalTools.ToolCrafting.Knapping
{
    [RequireComponent(typeof(Objective))]
    public class ObjectiveSignifier : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private GameObject signifierPrefab;

        [Min(0)]
        [SerializeField]
        [Tooltip("How many times the user can fail until a signifier is shown")]
        private int failureThreshold = 1;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Position offset of the signifier from the flake")]
        private float offset = 0.2f;

        #endregion

        #region Fields

        private GameObject signifier;
        private Objective objective;

        private int failures;

        #endregion

        #region Unity Lifecylce

        private void Awake()
        {
            objective = GetComponent<Objective>();
        }

        private void Start()
        {
            signifier = Instantiate(signifierPrefab, transform);
            signifier.SetActive(false);
        }

        private void OnEnable()
        {
            objective.OnInvalidAngle.AddListener(OnInvalidAngle);
            objective.OnDetach.AddListener(OnDetach);
        }

        private void OnDisable()
        {
            objective.OnInvalidAngle.RemoveListener(OnInvalidAngle);
            objective.OnDetach.RemoveListener(OnDetach);
        }

        #endregion

        #region Methods

        private void OnInvalidAngle(FlakeEventArgs args)
        {
            if (IsShowSignifier())
            {
                if (IsShowingSignifier())
                {
                    return;
                }

                var flake = FindRandomFlake();
                if (flake == null)
                {
                    return;
                }

                ShowSignifier(flake);
            }
            else
            {
                failures++;
            }
        }

        private void OnDetach(FlakeEventArgs args)
        {
            signifier.SetActive(false);
            failures = 0;
        }

        private bool IsShowSignifier()
        {
            return failures >= failureThreshold;
        }

        private bool IsShowingSignifier()
        {
            return signifier.activeSelf;
        }

        private Flake FindRandomFlake()
        {
            return objective
                .Flakes
                .Where(otherFlake => !otherFlake.IsDependencies)
                .ToList()
                .GetRandom();
        }

        private void ShowSignifier(Flake flake)
        {
            var direction = flake.OffsetDirections.GetRandom();
            var position = flake.transform.position;

            var signifierTransform = signifier.transform;
            signifierTransform.up = direction;
            signifierTransform.position = position + direction * offset;

            signifier.SetActive(true);
        }

        #endregion
    }
}

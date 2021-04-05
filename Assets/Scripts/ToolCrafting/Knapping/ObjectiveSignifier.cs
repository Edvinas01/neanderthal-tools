using System.Collections;
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
        private int failureThreshold = 5;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Position offset of the signifier from the flake")]
        private float offset = 0.2f;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Delay to deactivate the signifier")]
        private float deactivateDelay = 0.5f;

        [SerializeField]
        private string animationGrowTrigger = "Grow";

        [SerializeField]
        private string animationShrinkTrigger = "Shrink";

        #endregion

        #region Fields

        private Objective objective;

        private GameObject signifier;
        private Animator signifierAnimator;

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
            signifierAnimator = signifier.GetComponentInChildren<Animator>();

            signifier.SetActive(false);
        }

        private void OnEnable()
        {
            objective.OnDependenciesRemaining.AddListener(OnInvalidHit);
            objective.OnInvalidAngle.AddListener(OnInvalidHit);
            objective.OnDetach.AddListener(OnDetach);
        }

        private void OnDisable()
        {
            objective.OnDependenciesRemaining.RemoveListener(OnInvalidHit);
            objective.OnInvalidAngle.RemoveListener(OnInvalidHit);
            objective.OnDetach.RemoveListener(OnDetach);
        }

        #endregion

        #region Methods

        private void OnInvalidHit(FlakeEventArgs args)
        {
            if (IsShowSignifier())
            {
                if (IsShowingSignifier())
                {
                    return;
                }

                var flake = FindFlake(args);
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
            HideSignifier();
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

        private Flake FindFlake(FlakeEventArgs args)
        {
            var flake = args.Flake;
            if (flake == null)
            {
                return null;
            }

            return flake.IsDependencies
                ? FindRandomFlake()
                : flake;
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

            signifierAnimator.SetTrigger(animationGrowTrigger);
            signifier.SetActive(true);
        }

        private void HideSignifier()
        {
            signifierAnimator.SetTrigger(animationShrinkTrigger);
            StartSetSignifierActiveDelayed(false);
        }

        private void StartSetSignifierActiveDelayed(bool active)
        {
            StartCoroutine(SetSignifierActiveDelayed(active));
        }

        private IEnumerator SetSignifierActiveDelayed(bool active)
        {
            yield return new WaitForSeconds(deactivateDelay);
            signifier.SetActive(active);
        }

        #endregion
    }
}

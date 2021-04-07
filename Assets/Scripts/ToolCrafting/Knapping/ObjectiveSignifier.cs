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
        [Tooltip("Delay in seconds to deactivate the signifier on resetting failures")]
        private float deactivateDelay = 0.5f;

        [Min(0f)]
        [SerializeField]
        [Tooltip(
            "Delay in seconds to deactivate cleanup the signifier, e.g. when the objective is " +
            "not used"
        )]
        private float cleanupDelay = 30f;

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

        private Coroutine cleanupCoroutine;

        #endregion

        #region Properties

        private GameObject Signifier
        {
            get
            {
                if (signifier == null)
                {
                    signifier = Instantiate(signifierPrefab, transform);
                    signifier.SetActive(false);
                }

                return signifier;
            }
        }

        private Animator SignifierAnimator
        {
            get
            {
                if (signifierAnimator == null)
                {
                    signifierAnimator = Signifier.GetComponentInChildren<Animator>();
                }

                return signifierAnimator;
            }
        }

        #endregion

        #region Unity Lifecylce

        private void Awake()
        {
            objective = GetComponent<Objective>();
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
            return Signifier.activeSelf;
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

            var signifierTransform = Signifier.transform;
            signifierTransform.up = direction;
            signifierTransform.position = position + direction * offset;

            ShowSignifier();
            StartCleanupSignifier();
        }

        private void ShowSignifier()
        {
            SignifierAnimator.SetTrigger(animationGrowTrigger);
            Signifier.SetActive(true);
        }

        private void HideSignifier()
        {
            SignifierAnimator.SetTrigger(animationShrinkTrigger);
            StartSetSignifierActive(false);
        }

        private void StartSetSignifierActive(bool active)
        {
            StartCoroutine(SetActiveSignifier(active));
        }

        private IEnumerator SetActiveSignifier(bool active)
        {
            yield return new WaitForSeconds(deactivateDelay);
            Signifier.SetActive(active);
        }

        private void StartCleanupSignifier()
        {
            if (cleanupCoroutine != null)
            {
                StopCoroutine(cleanupCoroutine);
            }

            cleanupCoroutine = StartCoroutine(SetCleanupSignifier());
        }

        private IEnumerator SetCleanupSignifier()
        {
            yield return new WaitForSeconds(cleanupDelay);
            HideSignifier();
        }

        #endregion
    }
}

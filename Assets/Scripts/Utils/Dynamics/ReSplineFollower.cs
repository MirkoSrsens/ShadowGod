using LitMotion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;

namespace Razorhead.Core
{
    [HideMonoScript]
    [DefaultExecutionOrder(100)]
    public sealed class ReSplineFollower : MonoBehaviour
    {
        public SplineContainer splines;

        [ValidateInput(nameof(ValidateSplineIndex))]
        public int splineIndex;

        public float delay;
        public float duration;
        public Ease ease;

        [ShowIf("@ease == LitMotion.Ease.CustomAnimationCurve"), Indent]
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Tooltip("Will update transform rotation along direction")]
        public bool alignToDirection = true;

        [ShowIf("alignToDirection"), Indent]
        public Vector3 rotationOffset;

        float? timeStarted;

        private void OnEnable()
        {
            timeStarted = Time.time + delay;
        }

        private void OnDisable()
        {
            timeStarted = null;
        }

        private void LateUpdate()
        {
            if (!timeStarted.HasValue || duration == 0f) return;

            var t = Mathf.Clamp01((Time.time - timeStarted.Value) / duration);

            if (ease == Ease.CustomAnimationCurve)
            {
                t = curve.Evaluate(t);
            }
            else
            {
                EaseUtility.Evaluate(t, ease);
            }

            splines.Evaluate(splineIndex, t, out var position, out var tangent, out var upVector);

            if (alignToDirection)
            {
                transform.SetPositionAndRotation(position, Quaternion.LookRotation(tangent, upVector) * Quaternion.Euler(rotationOffset));
            }
            else
            {
                transform.position = position;
            }
        }

        private void Reset()
        {
            splines = GetComponent<SplineContainer>();
        }

        private bool ValidateSplineIndex()
        {
            return splineIndex >= 0 && (!splines || splineIndex < splines.Splines.Count);
        }
    }
}

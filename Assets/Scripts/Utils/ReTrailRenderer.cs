using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Razorhead.Core
{
    [RequireComponent(typeof(TrailRenderer))]
    [DefaultExecutionOrder(1), HideMonoScript]
    public sealed class ReTrailRenderer : MonoBehaviour
    {
        public TrailRenderer target;

        public bool clearOnEnable = true;

        private void OnEnable()
        {
            if (clearOnEnable)
            {
                if (target) target.Clear();
                ClearLate().Forget();
            }
        }

        private async UniTaskVoid ClearLate()
        {
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            if (target) target.Clear();
        }

        private void Reset()
        {
            target = GetComponent<TrailRenderer>();
        }
    }
}

using Noo.Tools;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Razorhead.Core
{
    [HideMonoScript]
    public class ShakeController : MonoBehaviour
    {
        const float minVelocity = 0.0001f;

        public RectTransform target;

        [SerializeField]
        SODCurve shakeSettings = SODCurve.Elastic;

        [SerializeField, FormerlySerializedAs("shakeScale")]
        float positionIntensity = 1f;

        [SerializeField]
        float scaleIntensity = 1f;

        readonly SODFloat2 shake1 = new();
        readonly SODFloat2 shake2 = new();
        readonly SODFloat2 shake3 = new();
        readonly SODFloat scale = new();

        bool sleeping = false;
        Vector2 initialPosition;
        Vector3 initalScale;

        public Vector2 Offset
        {
            get
            {
                return shake1.Target;
            }
            set
            {
                shake1.Target = value;
                shake2.Target = value;
                shake3.Target = value;
                sleeping = false;
            }
        }

        private void OnEnable()
        {
            sleeping = false;

            initialPosition = target ? target.anchoredPosition : default;
            initalScale = target ? target.localScale : Vector3.one;

            shake1.Curve = shake2.Curve = shake3.Curve = shakeSettings;
            scale.Curve = shakeSettings;

            shake1.Reset(default);
            shake2.Reset(default);
            shake3.Reset(default);
            scale.Reset(default);
        }

        private void OnDisable()
        {
            if (target)
            {
                target.anchoredPosition = initialPosition;
                target.localScale = initalScale;
            }
        }

        private void LateUpdate()
        {
            if (sleeping) return;

            shake1.Update(Time.deltaTime);
            shake2.Update(Time.deltaTime * 1.3f);
            shake3.Update(Time.deltaTime * 1.6f);
            scale.Update(Time.deltaTime);

            var position = shake1.Value + shake2.Value + shake3.Value;

            if (target)
            {
                target.anchoredPosition = initialPosition + (Vector2)position * positionIntensity;
                target.localScale = (1f + scale.Value * scaleIntensity) * initalScale;
            }

            if (math.lengthsq(position) < minVelocity &&
                math.lengthsq(shake1.Velocity) < minVelocity &&
                math.lengthsq(shake2.Velocity) < minVelocity &&
                math.lengthsq(shake3.Velocity) < minVelocity &&
                math.abs(scale.Velocity) < minVelocity)
            {
                sleeping = true;
            }
        }

        [Button(Expanded = true)]
        [FoldoutGroup("Runtime Controls")]
        [EnableIf("@UnityEngine.Application.isPlaying")]
        public void AddForce(Vector2 force, float lateralModifier = 0f, float positionModifer = 1f, float scaleForce = 0f)
        {
            shake1.Velocity += (float2)force * positionModifer;
            shake2.Velocity += UnityEngine.Random.value * lateralModifier * positionModifer * (float2)force.Rotate90();
            shake3.Velocity += UnityEngine.Random.value * lateralModifier * positionModifer * (float2)force.Rotate270();
            scale.Velocity += scaleForce;
            sleeping = false;
        }

        [Button(Expanded = true)]
        [FoldoutGroup("Runtime Controls")]
        [EnableIf("@UnityEngine.Application.isPlaying")]
        public void Shake(float intensity, float lateralModifier = 1f, float positionModifer = 1f, float scaleModifier = 0f)
        {
            var direction = UnityEngine.Random.insideUnitCircle.normalized * positionModifer;
            shake1.Velocity += 1f * intensity * (float2)direction;
            shake2.Velocity += UnityEngine.Random.value * intensity * lateralModifier * (float2)direction.Rotate90();
            shake3.Velocity += UnityEngine.Random.value * intensity * lateralModifier * (float2)direction.Rotate270();
            scale.Velocity += intensity * scaleModifier;
            sleeping = false;
        }
    }
}

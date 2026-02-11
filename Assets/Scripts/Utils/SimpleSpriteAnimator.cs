using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Razorhead.Core
{
    [HideMonoScript]
    public sealed class SimpleSpriteAnimator : MonoBehaviour
    {
        readonly static ReRandom rng = new();
        static int rngIndex = 1;

        public enum RendererType
        {
            Sprite,
            UiImage,
        }

        public enum PlayMode
        {
            Once = 1,
            Loop = 2,
            PingPong = 4,
            Random = 8,
        }

        [HideLabel, EnumToggleButtons]
        public RendererType rendererType;

        [SerializeField, ShowIf("@rendererType == RendererType.Sprite", false), LabelText("Target")]
        SpriteRenderer targetSpriteRenderer;

        [SerializeField, ShowIf("@rendererType == RendererType.UiImage", false), LabelText("Target")]
        Image targetImage;

        [Space]
        [EnumToggleButtons]
        public PlayMode playMode = PlayMode.Loop;

        public float spritesPerSecond = 30;
        public bool playOnEnable = true;

        public bool IsPlaying { get; set; }

        [ListDrawerSettings(ShowFoldout = false)]
        public List<Sprite> sprites = new();

        int instanceId;
        int frame;
        float time;

        private void OnEnable()
        {
            instanceId = rngIndex++ * 13;
            time = 0;

            if (playOnEnable)
            {
                IsPlaying = true;
                SetFrame(0);
            }
        }

        private void OnDisable()
        {
            IsPlaying = false;
        }

        private void Update()
        {
            if (sprites.Count == 0 || !IsPlaying) return;

            time += Time.deltaTime * spritesPerSecond;

            var spriteId = Mathf.FloorToInt(time);

            switch (playMode)
            {
                case PlayMode.Once:
                    spriteId = Mathf.Min(spriteId, sprites.Count - 1);
                    break;

                case PlayMode.Loop:
                    spriteId %= sprites.Count;
                    break;

                case PlayMode.PingPong:
                    var pingPongLoop = (spriteId / sprites.Count % 2) == 1;
                    spriteId %= sprites.Count;
                    if (pingPongLoop) spriteId = sprites.Count - spriteId - 1;
                    break;

                case PlayMode.Random:
                    rng.NewSeed(instanceId + spriteId * 97);
                    spriteId = (int)(rng.NextUint() % sprites.Count);
                    break;
            }

            if (frame != spriteId)
            {
                SetFrame(spriteId);
            }
        }

        void SetFrame(int index)
        {
            frame = index;

            var sprite = index >= 0 && index < sprites.Count ? sprites[frame] : null;

            if (targetImage)
            {
                targetImage.overrideSprite = sprite;
            }

            if (targetSpriteRenderer)
            {
                targetSpriteRenderer.sprite = sprite;
            }
        }

        private void Reset()
        {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
            targetImage = GetComponent<Image>();
            rendererType = targetImage ? RendererType.UiImage : RendererType.Sprite;
        }
    }
}

using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Razorhead.Core
{
    [HideMonoScript]
    [AddComponentMenu("Razorhead/ReAnimator")]
    [DefaultExecutionOrder(10)]
    public sealed class ReAnimator : MonoBehaviour, IAnimationClipSource
    {
        public enum OnEnableBehaviour
        {
            Resume,
            Reset,
            DoNothing
        }

        [SerializeField, HideInInspector]
        ReAnimatorController controller;

        [SerializeField, HideInInspector]
        DirectorUpdateMode updateMode = DirectorUpdateMode.GameTime;

        [SerializeField, PropertyOrder(-20)]
        [Tooltip("Resume: Plays last queued animation \nReset: Plays first animation in the controller when object is enabled")]
        OnEnableBehaviour onEnable = OnEnableBehaviour.Resume;

        [SerializeField, HideInInspector]
        float speed = 1f;

        public Action<string> OnAnimationStarted;
        public Action<string> OnAnimationEnded;

        [ShowInInspector, PropertyOrder(-10)]
        public ReAnimatorController Controller
        {
            get => controller;
            set
            {
                if (controller == value) return;
                DestroyGraph();
                controller = value;
                if (isActiveAndEnabled) OnEnable();
            }
        }

        [ShowInInspector, PropertyOrder(-20)]
        public DirectorUpdateMode UpdateMode
        {
            get => updateMode;
            set
            {
                if (updateMode == value) return;
                updateMode = value;
                if (playableGraph.IsValid()) playableGraph.SetTimeUpdateMode(value);
            }
        }

        [ShowInInspector, PropertyOrder(-10)]
        public float Speed
        {
            get => speed;
            set
            {
                if (speed == value) return;
                speed = value;
                if (mixerPlayable.IsValid()) mixerPlayable.SetSpeed(value);
            }
        }

        Animator internalAnimator;
        PlayableGraph playableGraph;
        PlayableOutput playableOutput;
        AnimationMixerPlayable mixerPlayable;

        [TitleGroup("Runtime Controls"), ShowInInspector, HideInEditorMode, PropertyOrder(1000)]
        ReAnimatorState[] states = Array.Empty<ReAnimatorState>();
        int statesLength;

        [Serializable]
        public struct ReAnimatorState
        {
            public string name;
            public AnimationClipPlayable clip;
            public float duration;
            public float weight;
            public bool loop;
        }

        [TitleGroup("Runtime Controls"), ShowInInspector, HideInEditorMode, PropertyOrder(1000)]
        public readonly HashSet<string> triggers = new();

        [TitleGroup("Runtime Controls"), ShowInInspector, HideInEditorMode, PropertyOrder(1000)]
        public readonly Dictionary<string, bool> parametersBool = new();

        [TitleGroup("Runtime Controls"), ShowInInspector, HideInEditorMode, PropertyOrder(1000)]
        public readonly Dictionary<string, float> parametersFloat = new();

        [TitleGroup("Runtime Controls"), ShowInInspector, HideInEditorMode, PropertyOrder(1000)]
        public readonly Dictionary<string, int> parametersInt = new();

        #region Playback Variables
        bool isPlaying;
        int currentAnimation = 0;
        float blend;
        #endregion

        [TitleGroup("Runtime Controls"), ShowInInspector, HideInEditorMode, PropertyOrder(1000)]
        public string ActiveAnimationName
        {
            get
            {
                if (controller == null) return null;
                if (currentAnimation < 0 || currentAnimation >= controller.states.Length) return null;
                return controller.states[currentAnimation].Name;
            }
        }

        [TitleGroup("Runtime Controls"), ShowInInspector, HideInEditorMode, PropertyOrder(1000)]
        public float ActiveAnimationTimeNormalized
        {
            get
            {
                if (controller == null) return default;
                if (currentAnimation < 0 || currentAnimation >= states.Length) return default;
                var state = states[currentAnimation];
                if (state.clip.IsNull() || !state.clip.IsValid()) return default;
                return state.duration == 0f ? 1f : Mathf.Clamp01((float)state.clip.GetTime() / state.duration);
            }
        }

        public bool IsPlaying => isPlaying;

        public void GetAnimationClips(List<AnimationClip> results)
        {
            if (controller) foreach (var state in controller.states) if (state.Clip) results.Add(state.Clip);
        }

        void DestroyGraph()
        {
            if (!playableGraph.IsValid()) return;

            for (int i = 0; i < states.Length; i++)
            {
                if (states[i].clip.IsValid()) states[i].clip.Destroy();
            }

            statesLength = 0;

            mixerPlayable.Destroy();
            playableGraph.Destroy();

            mixerPlayable = default;
            playableGraph = default;
        }

        void Awake()
        {
            internalAnimator = gameObject.GetOrAddComponent<Animator>();
            internalAnimator.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
        }

        void OnEnable()
        {
            switch (onEnable)
            {
                case OnEnableBehaviour.Reset:
                    Play(0);
                    break;

                case OnEnableBehaviour.Resume:
                    if (currentAnimation >= 0)
                    {
                        Play(currentAnimation);
                    }
                    break;
            }
        }

        void OnDisable()
        {
            Stop();
            DestroyGraph();
        }

        bool AssertGraph()
        {
            if (playableGraph.IsValid()) return true;
            if (!controller || !isActiveAndEnabled) return false;

            playableGraph = PlayableGraph.Create();
            playableGraph.SetTimeUpdateMode(updateMode);

            playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", internalAnimator);

            var stateInfos = controller.States;

            mixerPlayable = AnimationMixerPlayable.Create(playableGraph, stateInfos.Length);
            playableOutput.SetSourcePlayable(mixerPlayable);

            if (stateInfos.Length > states.Length)
            {
                Array.Resize(ref states, stateInfos.Length);
            }

            statesLength = states.Length;

            for (int i = 0; i < statesLength; i++)
            {
                var clip = stateInfos[i].Clip;
                var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
                clipPlayable.SetSpeed(stateInfos[i].Speed);
                playableGraph.Connect(clipPlayable, 0, mixerPlayable, i);

                states[i] = new ReAnimatorState()
                {
                    name = stateInfos[i].Name,
                    clip = clipPlayable,
                    duration = clip.length,
                    loop = clip.isLooping,
                };
            }

            for (int i = 0; i < statesLength; i++)
            {
                mixerPlayable.SetInputWeight(i, 1f);
            }

            mixerPlayable.SetSpeed(speed);

            playableGraph.Play();

            return true;
        }

        public bool TryGetAnimationIndex(string stateName, out int stateIndex)
        {
            if (controller)
            {
                for (int i = 0; i < controller.states.Length; i++)
                {
                    if (controller.states[i].Name == stateName)
                    {
                        stateIndex = i;
                        return true;
                    }
                }
            }

            stateIndex = -1;
            return false;
        }

        public void Play(string name)
        {
            if (TryGetAnimationIndex(name, out int stateIndex))
            {
                Play(stateIndex, 0f, 0f);
            }
        }

        [TitleGroup("Runtime Controls")]
        [Button, ShowIf("@UnityEngine.Application.isPlaying")]
        public void Play(string name, float startTime, float fadeInDuration)
        {
            if (TryGetAnimationIndex(name, out int stateIndex))
            {
                Play(stateIndex, startTime, fadeInDuration);
            }
        }

        public void Play(int index, float startTime = 0f, float fadeInDuration = 0f)
        {
            if (!AssertGraph())
            {
                currentAnimation = index;
                return;
            }

            if (index < 0 || index >= statesLength) return;

            currentAnimation = index;
            isPlaying = true;

            ref var stateInfo = ref states[index];

            stateInfo.clip.SetTime(startTime);

            if (fadeInDuration == 0f)
            {
                blend = -1f;
                BlendInState(index, 1f);
            }
            else
            {
                blend = 1f / fadeInDuration;
            }

            if (!playableGraph.IsPlaying())
            {
                playableGraph.Play();
            }

            try { playableGraph.Evaluate(); }
            catch (Exception) { }

            OnAnimationStarted?.Invoke(stateInfo.name);
        }

        [TitleGroup("Runtime Controls")]
        [Button, ShowIf("@UnityEngine.Application.isPlaying")]
        public void Stop()
        {
            isPlaying = false;
            if (playableGraph.IsValid()) playableGraph.Stop();
        }

        void Update()
        {
            if (!isPlaying) return;
            if (updateMode == DirectorUpdateMode.GameTime) EvaluateGraph(Time.deltaTime);
            else if (updateMode == DirectorUpdateMode.UnscaledGameTime) EvaluateGraph(Time.unscaledDeltaTime);
        }

        void LateUpdate()
        {
            if (controller && controller.TryTransition(this, out var toState))
            {
                Play(toState);
            }
        }

        void EvaluateGraph(float deltaTime)
        {
            var currentState = states[currentAnimation];

            if (blend > 0f) BlendInState(currentAnimation, blend * deltaTime);

            if (!currentState.loop && currentState.weight >= 1f && currentState.clip.GetTime() > currentState.duration)
            {
                OnAnimationEnded?.Invoke(currentState.name);

                if (!TryAutoTransition()) Stop();
            }
        }

        bool TryAutoTransition()
        {
            if (controller && controller.TryGetAutoTransition(states[currentAnimation].name, out var transition))
            {
                if (TryGetAnimationIndex(transition.To, out var toIndex))
                {
                    Play(toIndex, 0f, transition.Blend);
                    return true;
                }
            }

            return false;
        }

        void BlendInState(int state, float blend)
        {
            for (int i = 0; i < statesLength; i++)
            {
                ref var stateInfo = ref states[i];
                stateInfo.weight = math.saturate(stateInfo.weight + ((i == state) ? blend : -blend));
                mixerPlayable.SetInputWeight(i, stateInfo.weight);
            }
        }

        public void SetBool(string name, bool value)
        {
            parametersBool[name] = value;
        }

        public void SetInt(string name, int value)
        {
            parametersInt[name] = value;
        }

        public void SetFloat(string name, float value)
        {
            parametersFloat[name] = value;
        }
    }
}

using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

namespace Razorhead.Core
{
    [CreateAssetMenu(menuName = "Razorhead/Animator Controller")]
    public class ReAnimatorController : ScriptableObject, ISerializationCallbackReceiver
    {
        [Serializable]
        [InlineProperty]
        public class Transition
        {
            [ValueDropdown("@((ReAnimatorController)$property.SerializationRoot.ValueEntry.WeakSmartValue).StateNames")]
            [SerializeField] string from;
            [ValueDropdown("@((ReAnimatorController)$property.SerializationRoot.ValueEntry.WeakSmartValue).StateNames")]
            [SerializeField] string to;
            [SerializeField] float blendTime;

            public string From => from;
            public string To => to;
            public float Blend => blendTime;
        }

        [Serializable]
        public class ConditionalTransition
        {
            [ValueDropdown("@((ReAnimatorController)$property.SerializationRoot.ValueEntry.WeakSmartValue).StateNames")]
            [SerializeField] string from;
            [ValueDropdown("@((ReAnimatorController)$property.SerializationRoot.ValueEntry.WeakSmartValue).StateNames")]
            [SerializeField] string to;

            [SerializeField, Range(0, 1)]
            float minExitTime = 0f;

            [PolymorphicDrawerSettings(ShowBaseType = false)]
            [SerializeField, SerializeReference]
            ICondition condition = new ReAnimatorTriggerCondition();

            public ICondition Condition => condition;
            public string From => from;
            public string To => to;
            public float MinExitTime => minExitTime;

            public bool Evaluate(ReAnimator animator)
            {
                if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to)) return false;
                if (from != animator.ActiveAnimationName) return false;
                if (minExitTime > animator.ActiveAnimationTimeNormalized) return false;
                if (condition == null) return false;
                return condition.Evaluate(animator);
            }
        }

        [InlineProperty(LabelWidth = 80)]
        public interface ICondition
        {
            public bool Evaluate(ReAnimator animator);
        }

        string[] StateNames => states.Select(x => x.Name).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToArray();

        [TableList(AlwaysExpanded = true, ShowPaging = false)]
        [SerializeField] internal ReAnimatorStateInfo[] states = Array.Empty<ReAnimatorStateInfo>();

        [Space]
        [SerializeField]
        [TableList(AlwaysExpanded = true, ShowPaging = false)]
        internal Transition[] autoTransitions = Array.Empty<Transition>();

        [Space]
        [SerializeField]
        internal ConditionalTransition[] conditionalTransitions = Array.Empty<ConditionalTransition>();

        [NonSerialized]
        ReAnimatorStateInfo[] validStates = Array.Empty<ReAnimatorStateInfo>();

        public ReadOnlySpan<ReAnimatorStateInfo> States => validStates;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            validStates = states.Where(x => !string.IsNullOrWhiteSpace(x.Name) && x.Clip).ToArray();
        }

        public bool TryGetAutoTransition(string fromState, out Transition transition)
        {
            for (int i = 0; i < autoTransitions.Length; i++)
            {
                if (autoTransitions[i].From == fromState)
                {
                    transition = autoTransitions[i];
                    return true;
                }
            }

            transition = default;
            return false;
        }

        public bool TryTransition(ReAnimator animator, out string toState)
        {
            for (var i = 0; i < conditionalTransitions.Length; i++)
            {
                var condition = conditionalTransitions[i];

                if (condition.Evaluate(animator))
                {
                    toState = condition.To;
                    return true;
                }
            }

            toState = default;
            return false;
        }
    }

    [Serializable]
    public class ReAnimatorStateInfo
    {
        [SerializeField] string name;
        [SerializeField] AnimationClip clip;
        [SerializeField, TableColumnWidth(60, false)] float speed = 1f;

        public string Name => name;
        public AnimationClip Clip => clip;
        public float Speed => speed;
    }

    [Serializable, TypeRegistryItem("Trigger")]
    public class ReAnimatorTriggerCondition : ReAnimatorController.ICondition
    {
        public string trigger;

        public bool Evaluate(ReAnimator animator)
        {
            if (!string.IsNullOrEmpty(trigger) && animator.triggers.Contains(trigger))
            {
                animator.triggers.Remove(trigger);
                return true;
            }

            return false;
        }
    }

    [Serializable, TypeRegistryItem("Bool")]
    public class ReAnimatorBoolCondition : ReAnimatorController.ICondition
    {
        public string parameter;

        [LabelText("Is")]
        public bool value = true;

        public bool Evaluate(ReAnimator animator)
        {
            if (string.IsNullOrEmpty(parameter)) return false;

            if (animator.parametersBool.TryGetValue(parameter, out var value))
            {
                return value == this.value;
            }

            return false;
        }
    }

    [Serializable, TypeRegistryItem("Integer")]
    public class ReAnimatorIntCondition : ReAnimatorController.ICondition
    {
        public enum Op
        {
            Equals,
            NotEquals,
            GreaterThan,
            GreaterThanOrEquals,
            LessThan,
            LessThanOrEquals,
        }

        public string parameter;

        public Op operation;

        public int value;

        public bool Evaluate(ReAnimator animator)
        {
            if (string.IsNullOrEmpty(parameter)) return false;

            if (animator.parametersInt.TryGetValue(parameter, out var value))
            {
                return operation switch
                {
                    Op.Equals => value == this.value,
                    Op.NotEquals => value != this.value,
                    Op.GreaterThan => this.value > value,
                    Op.GreaterThanOrEquals => this.value >= value,
                    Op.LessThan => this.value < value,
                    Op.LessThanOrEquals => this.value <= value,
                    _ => false,
                };
            }

            return false;
        }
    }

    [Serializable, TypeRegistryItem("Float")]
    public class ReAnimatorFloatCondition : ReAnimatorController.ICondition
    {
        public enum Op
        {
            Equals,
            NotEquals,
            GreaterThan,
            GreaterThanOrEquals,
            LessThan,
            LessThanOrEquals,
        }

        public string parameter;

        public Op operation;

        public float value;

        public bool Evaluate(ReAnimator animator)
        {
            if (string.IsNullOrEmpty(parameter)) return false;

            if (animator.parametersFloat.TryGetValue(parameter, out var value))
            {
                return operation switch
                {
                    Op.Equals => value == this.value,
                    Op.NotEquals => value != this.value,
                    Op.GreaterThan => this.value > value,
                    Op.GreaterThanOrEquals => this.value >= value,
                    Op.LessThan => this.value < value,
                    Op.LessThanOrEquals => this.value <= value,
                    _ => false,
                };
            }

            return false;
        }
    }
}

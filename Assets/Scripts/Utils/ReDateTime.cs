using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Razorhead.Core
{
    [Serializable]
    [InlineProperty]
    public struct ReDateTime : IEquatable<ReDateTime>,
        ISerializationCallbackReceiver
    {
        enum Kind { Utc, Local }

        [SerializeField]
        [HideInInspector]
        long ticks;

        /// <summary>Always serialized as UTC</summary>
        [NonSerialized]
        DateTime dateTime;

#if UNITY_EDITOR
        static Kind EditKind
        {
            get => UnityEditor.EditorPrefs.GetBool("ReDateTime_EditKind", false) ? Kind.Local : Kind.Utc;
            set => UnityEditor.EditorPrefs.SetBool("ReDateTime_EditKind", value == Kind.Local);
        }

        static bool IsUtc => EditKind == Kind.Utc;

        [ShowInInspector, HideLabel, DisplayAsString, PropertyOrder(100)]
        readonly string DateAsString => GetDate().ToString();

        [HideIf("IsUtc")]
        [Button("Local", ButtonHeight = 21), PropertyOrder(10), HorizontalGroup("HM", width: 50, marginLeft: 13)]
        static void EditAsUtc() => EditKind = Kind.Utc;

        [ShowIf("IsUtc")]
        [Button("UTC", ButtonHeight = 21), PropertyOrder(10), HorizontalGroup("HM", width: 50, marginLeft: 13)]
        static void EditAsLocal() => EditKind = Kind.Local;

        void SetDate(int? year = null, int? month = null, int? day = null, int? hour = null, int? minute = null)
        {
            try
            {
                var newDate = new DateTime(
                    year ?? Year,
                    month ?? Month,
                    day ?? Day,
                    hour ?? Hour,
                    minute ?? Minute,
                    default
                );

                dateTime = IsUtc ? newDate : newDate.ToUniversalTime();
            }
            catch (Exception)
            {
            }
        }

        readonly DateTime GetDate()
        {
            return IsUtc ? dateTime : dateTime.ToLocalTime();
        }

        [ShowInInspector, HorizontalGroup("DMY", width: 46), HideLabel, LabelText("D"), LabelWidth(12), Tooltip("Day")]
        int Day
        {
            readonly get => GetDate().Day;
            set => SetDate(day: value);
        }

        [ShowInInspector, HorizontalGroup("DMY", width: 46), LabelText("M"), LabelWidth(12), Tooltip("Month")]
        int Month
        {
            readonly get => GetDate().Month;
            set => SetDate(month: value);
        }

        [ShowInInspector, HorizontalGroup("DMY", width: 62), LabelText("Y"), LabelWidth(12), Tooltip("Year")]
        int Year
        {
            readonly get => GetDate().Year;
            set => SetDate(year: value);
        }

        [ShowInInspector, HorizontalGroup("HM", width: 46), LabelText("h"), LabelWidth(12), Tooltip("Hour")]
        int Hour
        {
            readonly get => GetDate().Hour;
            set => SetDate(hour: value);
        }

        [ShowInInspector, HorizontalGroup("HM", width: 46), LabelText("m"), LabelWidth(12), Tooltip("Minute")]
        int Minute
        {
            readonly get => GetDate().Minute;
            set => SetDate(minute: value);
        }
#endif

        public DateTime DateTime
        {
            readonly get => dateTime;
            set => dateTime = value;
        }

        public override readonly bool Equals(object obj) => obj is ReDateTime time && Equals(time);
        public readonly bool Equals(ReDateTime other) => ticks == other.ticks;
        public override readonly int GetHashCode() => HashCode.Combine(ticks);

        public void OnAfterDeserialize()
        {
            dateTime = DateTime.SpecifyKind(new DateTime(ticks), DateTimeKind.Utc);
        }

        public void OnBeforeSerialize()
        {
            ticks = dateTime.Ticks;
        }

        public static bool operator ==(ReDateTime left, ReDateTime right) => left.Equals(right);
        public static bool operator !=(ReDateTime left, ReDateTime right) => !(left == right);

        public static implicit operator DateTime(ReDateTime d) => d.dateTime;
    }
}

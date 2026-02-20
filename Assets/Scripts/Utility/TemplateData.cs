using Assets.Scripts.Data.Utility;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameplay.States
{
    [Serializable]
    [InlineProperty]
    public class TemplateData
    {
        public MinMaxRange<float> range;

        [HideLabel, InlineProperty]
        public AnimationCurve curve;

        public float GetData(int currentLevel, int maxLevel)
        {
            var percent = Mathf.Clamp01((currentLevel - 1) / Mathf.Clamp(((float)maxLevel - 1), 1, int.MaxValue));
            var evaluation = curve.Evaluate(percent);

            return range.min + (range.max - range.min) * evaluation;
        }
    }
}
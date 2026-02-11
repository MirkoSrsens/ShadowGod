using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Razorhead.Core
{
    public class ScrollableSlider : MonoBehaviour
    {
        public ScrollRect rect;
        public Slider scrollSlider;
        public bool horizontal = true;
        public bool Vertical = false;
    
        public void Awake()
        {
            scrollSlider.onValueChanged.AddListener(OnScrollerMoved);
            rect.onValueChanged.AddListener(OnRectMoved);
        }
    
        private void OnRectMoved(Vector2 arg0)
        {
            if (horizontal && scrollSlider.value != rect.horizontalNormalizedPosition)
            {
                scrollSlider.value = GetPosition(rect.horizontalNormalizedPosition);
            }
            if (Vertical && scrollSlider.value != rect.verticalNormalizedPosition)
            {
                scrollSlider.value = GetPosition(rect.verticalNormalizedPosition);
            }
        }
    
        private void OnScrollerMoved(float arg0)
        {
            if (horizontal && scrollSlider.value != rect.verticalNormalizedPosition)
            {
                rect.horizontalNormalizedPosition = GetPosition(scrollSlider.value);
            }
            if (Vertical && scrollSlider.value != rect.verticalNormalizedPosition)
            {
                rect.verticalNormalizedPosition = GetPosition(scrollSlider.value);
            }
        }
    
        public float GetPosition(float value)
        {
            return Mathf.Lerp(scrollSlider.maxValue, scrollSlider.minValue, value);
        }
    
    }
}
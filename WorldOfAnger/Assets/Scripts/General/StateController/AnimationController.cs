using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace General.State
{
    public class AnimationController:MonoBehaviour
    {
        /// <summary>
        /// Gets or sets animator; 
        /// </summary>
        Animator Anima { get; set; }
        string ActiveAnimation { get; set; }

        private void Awake()
        {
            Anima = GetComponentInChildren<Animator>();
        }

        public void SetAnimation(string animationName)
        {
            if(!string.IsNullOrEmpty(ActiveAnimation)) Anima.SetBool(ActiveAnimation, false);
            Anima.SetBool(animationName, true);
            ActiveAnimation = animationName;
        }

        public void SetStateAnimation(string stateAnimationName)
        {
            if (Anima.GetBool(stateAnimationName)) Anima.SetBool(stateAnimationName, false);
            else
            {
                Anima.SetBool(stateAnimationName, true);
            }
        }

    }
}

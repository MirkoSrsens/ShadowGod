using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace General.State
{
    public class AnimationController:MonoBehaviour
    {
        Animator animator;
        string ActiveAnimation { get; set; }

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        public void SetAnimation(string animationName)
        {
            if(!string.IsNullOrEmpty(ActiveAnimation)) animator.SetBool(ActiveAnimation, false);
            animator.SetBool(animationName, true);
            ActiveAnimation = animationName;
        }

        public void SetStateAnimation(string stateAnimationName)
        {
            if (animator.GetBool(stateAnimationName)) animator.SetBool(stateAnimationName, false);
            else
            {
                animator.SetBool(stateAnimationName, true);
            }
        }

    }
}

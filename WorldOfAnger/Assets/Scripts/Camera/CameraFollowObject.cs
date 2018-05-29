using System.Collections;
using System.Collections.Generic;
using General.State;
using Implementation.Data;
using Player.Movement;
using UnityEngine;
using Zenject;

namespace Camera
{
    /// <summary>
    /// Scripts for following the object.
    /// </summary>
    public class CameraFollowObject : MonoBehaviour
    {
        [Inject]
        /// Holds all value about camera movement.
        ICameraData cameraData { get; set; }

        /// <summary>
        /// Gets or sets object that is being followed by camera.
        /// </summary>
        static GameObject ActiveObjectToFollow { get; set; }

        /// <summary>
        /// Gets or sets timeout of following.
        /// </summary>
        static float TimeToFollow { get; set; }

        /// <summary>
        /// Gets or sets flag that prevents update from calling coroutine multiple times.
        /// </summary>
        static bool swapFlag { get; set; }

        // Use this for initialization
        void Awake()
        {
            cameraData.Player = GameObject.FindGameObjectWithTag("Player");
            ActiveObjectToFollow = cameraData.Player;
            cameraData.Controller = cameraData.Player.GetComponent<StateController>();
            cameraData.StopMoving = cameraData.Player.GetComponent<StopMoving>();
            cameraData.FollowDistance = ActiveObjectToFollow.transform.position.z - cameraData.ZAxisOffset;
            this.transform.position = new Vector3(ActiveObjectToFollow.transform.position.x, transform.position.y, cameraData.FollowDistance);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (ActiveObjectToFollow != null)
            {
                this.transform.position = Vector3.Lerp(transform.position, new Vector3(ActiveObjectToFollow.transform.position.x, ActiveObjectToFollow.transform.position.y, cameraData.FollowDistance), cameraData.MovementSpeed * Time.deltaTime);
                if (ActiveObjectToFollow != cameraData.Player && !swapFlag)
                {
                    swapFlag = true;
                    StartCoroutine(timeToComeBack(TimeToFollow));
                }
            }
        }

        /// <summary>
        /// Sets follow target of the camera.
        /// </summary>
        /// <param name="followTarget">New target to follow.</param>
        /// <param name="timeOfFollow">Time of following.</param>
        public static void SetFollowTarget(GameObject followTarget, float timeOfFollow)
        {
            ActiveObjectToFollow = followTarget;
            TimeToFollow = timeOfFollow;
        }

        /// <summary>
        /// Sets focus back to player after some time.
        /// </summary>
        /// <param name="timeOfFollow">Secounds to follow other object.</param>
        /// <returns>Yield.</returns>
        private IEnumerator timeToComeBack(float timeOfFollow)
        {
            cameraData.Controller.SwapState(cameraData.StopMoving);
            yield return new WaitForSeconds(timeOfFollow);
            ActiveObjectToFollow = cameraData.Player;
            cameraData.Controller.EndState(cameraData.StopMoving);
            swapFlag = false;
        }
    }
}

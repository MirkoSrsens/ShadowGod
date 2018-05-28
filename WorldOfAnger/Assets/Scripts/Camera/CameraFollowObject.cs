using System.Collections;
using System.Collections.Generic;
using General.State;
using Player.Movement;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour {

    private static GameObject Player { get; set; }
    private StateController controller { get; set; }
    private StopMoving stopMoving { get; set; }
    private static GameObject ActiveObjectToFollow { get; set; }
    private static float TimeToFollow { get; set; }
    private static bool swapFlag { get; set; }

    public float followDistance;
    public float distanceToMove;
    private float movementSpeed { get; set; }

    // Use this for initialization
    void Awake () {
        Player = GameObject.FindGameObjectWithTag("Player");
        controller = Player.GetComponent<StateController>();
        stopMoving = Player.GetComponent<StopMoving>();
        ActiveObjectToFollow = Player;
        followDistance = 15;
        distanceToMove = 1;
        movementSpeed = 10;
        this.transform.position = new Vector3(ActiveObjectToFollow.transform.position.x, transform.position.y, ActiveObjectToFollow.transform.position.z - followDistance);
    }

    // Update is called once per frame
    void Update () {
        if (ActiveObjectToFollow != null)
        {
            if ((ActiveObjectToFollow == Player && controller.ActiveStateMovement is PlayerIdle) || ActiveObjectToFollow != Player)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(ActiveObjectToFollow.transform.position.x, ActiveObjectToFollow.transform.position.y, ActiveObjectToFollow.transform.position.z - followDistance), movementSpeed * Time.deltaTime);
            }
            else
            {
                if (Vector3.Distance(new Vector3(transform.position.x, 0, 0), new Vector3(ActiveObjectToFollow.transform.position.x, 0, 0)) > distanceToMove)
                {
                    if (ActiveObjectToFollow.transform.position.x > transform.position.x)
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(ActiveObjectToFollow.transform.position.x - distanceToMove, transform.position.y, ActiveObjectToFollow.transform.position.z - followDistance), movementSpeed);
                    }
                    if (ActiveObjectToFollow.transform.position.x < transform.position.x)
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(ActiveObjectToFollow.transform.position.x + distanceToMove, transform.position.y, ActiveObjectToFollow.transform.position.z - followDistance), movementSpeed);
                    }
                }
                if (Vector3.Distance(new Vector3(0, this.transform.position.y, 0), new Vector3(0, ActiveObjectToFollow.transform.position.y, 0)) > distanceToMove)
                {
                    if (ActiveObjectToFollow.transform.position.y < transform.position.y)
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(transform.position.x, ActiveObjectToFollow.transform.position.y + distanceToMove, ActiveObjectToFollow.transform.position.z - followDistance), movementSpeed);
                    }
                    if (ActiveObjectToFollow.transform.position.y > transform.position.y)
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(transform.position.x, ActiveObjectToFollow.transform.position.y - distanceToMove, ActiveObjectToFollow.transform.position.z - followDistance), movementSpeed);
                    }
                }
            }
            if (ActiveObjectToFollow != Player && !swapFlag)
            {
                swapFlag = true;
                StartCoroutine(timeToComeBack(TimeToFollow));
            }
        }
    }

    public static void SetFollowTarget(GameObject followTarget, float timeOfFollow)
    {
        ActiveObjectToFollow = followTarget;
        TimeToFollow = timeOfFollow;
    }

    private IEnumerator timeToComeBack(float timeOfFollow)
    {
        controller.SwapState(stopMoving);
        yield return new WaitForSeconds(timeOfFollow);
        ActiveObjectToFollow = Player;
        controller.EndState(stopMoving);
        swapFlag = false;
    }
}

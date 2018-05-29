using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallObject : MonoBehaviour {

    float duration = 5;
    float speed = 10;
	// Use this for initialization
	void Start () {
        StartCoroutine(timeOut());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
	}

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Collision");
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("EnemyHitted");
        }
        StopCoroutine(timeOut());
        Destroy(this.gameObject);
    }

    private IEnumerator timeOut()
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}

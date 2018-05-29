using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Defines sets of functions for spawning objects.
/// </summary>
public class SpawnObjects : MonoBehaviour {

    public GameObject objectToSpawn;
    public int NumberOfObjects;
    public float LifetimeOfObject;
    public float OffsetOfObjects;
    public float OffSetSpawn;
    public float YAxisOffset;
    private List<GameObject> SpawnedObjects { get; set; }

    private bool destroyFlag { get; set; }
    // Use this for initialization
    void Awake() {
        objectToSpawn = objectToSpawn == null ? Resources.Load("SpawnedObject", typeof(GameObject)) as GameObject : objectToSpawn;
        SpawnedObjects = new List<GameObject>();
        OffsetOfObjects = OffsetOfObjects==0 ? 1.5f : OffsetOfObjects;
        NumberOfObjects = NumberOfObjects==0 ? 4 : NumberOfObjects;
        LifetimeOfObject = LifetimeOfObject == 0 ? 25 : LifetimeOfObject;
        OffSetSpawn = OffSetSpawn == 0 ? 1f : OffSetSpawn;
        YAxisOffset = YAxisOffset == 0 ? 0.7f : YAxisOffset;
        destroyFlag = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((!destroyFlag || SpawnedObjects.Any(x=>x == null)) && collision.gameObject.tag == "Player")
        {
            ClearAllObjects();
            SpawnedObjects.Clear();
            destroyFlag = true;
            for (int i = 0; i < NumberOfObjects; i++)
            {
                if(SpawnedObjects.Count == 0 || i+1 == NumberOfObjects)
                {
                    SpawnedObjects.Add(Instantiate(objectToSpawn, new Vector3((transform.position.x + (transform.localScale.x / 2)) - (OffSetSpawn + OffsetOfObjects * i), transform.position.y, transform.position.z), Quaternion.identity));
                }
                else
                {
                    SpawnedObjects.Add(Instantiate(objectToSpawn, new Vector3((transform.position.x + (transform.localScale.x / 2)) - (OffSetSpawn + OffsetOfObjects * i), SpawnedObjects.Last().transform.position.y + Random.Range(-YAxisOffset/2, YAxisOffset/2), transform.position.z), Quaternion.identity));
                }
            }
            StartCoroutine(destroyObjectsAfter());
        }
    }

    private IEnumerator destroyObjectsAfter()
    {
        yield return new WaitForSeconds(LifetimeOfObject);
        ClearAllObjects();
        destroyFlag = false;
    }

    private void ClearAllObjects()
    {
        foreach (var spawnedObject in SpawnedObjects)
        {
            Destroy(spawnedObject);
        }
    }
}

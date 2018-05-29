using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnqueueText : MonoBehaviour {

    private GameObject textBoxInformationsResource { get; set; }
    private GameObject spawnedGameBox { get; set; }
    private Text displayerText { get; set; }
    private static Queue<string> textToDisplay;
	// Use this for initialization
	void Start () {
        textToDisplay = new Queue<string>();
        textBoxInformationsResource = Resources.Load("DialogBox", typeof(GameObject)) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(textToDisplay.Count>0)
        {
            if(spawnedGameBox == null)
            {
                spawnedGameBox = Instantiate(textBoxInformationsResource, this.transform);
                displayerText = spawnedGameBox.GetComponentInChildren<Text>();
                displayerText.text = textToDisplay.Dequeue();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                displayerText.text = textToDisplay.Dequeue();
            }
        }
        else if ( spawnedGameBox != null)
        {
            Destroy(spawnedGameBox);
            spawnedGameBox = null;
        }
	}

    public static void OpenTextBox(Queue<string> textToEnqueue)
    {
        textToDisplay = textToEnqueue;
    }
}

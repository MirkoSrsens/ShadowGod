using Camera;
using UnityEngine;

public class TestFocusSwap : MonoBehaviour {

    public void SwapFocus()
    {
        var testFocus = GameObject.Find("TestFocus");
        CameraFollowObject.SetFollowTarget(testFocus, 3);
    }
}

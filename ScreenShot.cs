using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour {

	int photo=0;
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.KeypadEnter)){
			ScreenCapture.CaptureScreenshot ("screenshot"+photo.ToString ());
			photo++;
		}
	}
}

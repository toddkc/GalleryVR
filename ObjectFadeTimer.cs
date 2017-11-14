using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectFadeTimer : MonoBehaviour {

	Image thisImage;
	Text thisText;
	Color color;

	void Start(){
		thisImage = GetComponent<Image> ();
		thisText = GetComponent<Text> ();
		if(thisImage==null){
			color = thisText.color;
		}else{
			color = thisImage.color;
		}
		StartCoroutine (FadeObject ());
	}

	IEnumerator FadeObject(){
		while (color.a != 0) {
			color -= new Color (0, 0, 0, 0.01f);
			yield return new WaitForEndOfFrame ();
		}
	}
}

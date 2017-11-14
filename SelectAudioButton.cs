using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class SelectAudioButton : MonoBehaviour {


	[HideInInspector]public SelectAudioButton clickedButton;
	[HideInInspector]public Button thisButton;
	[HideInInspector]public Text buttonText;
	Menu menu;
	AudioSource audioSource;
	WWW musicFile;

	void Start(){
		clickedButton = this;
		thisButton = GetComponent<Button> ();
		thisButton.onClick.AddListener (OnClick);
		menu = GameObject.Find ("Manager").GetComponent<Menu>();
		audioSource = GameObject.Find ("Speaker").GetComponentInChildren<AudioSource> ();
	}

	void OnClick(){
		SelectAudio ();
	}

	//called when select button clicked
	void SelectAudio(){
		audioSource.Stop ();
		audioSource.clip = null;
		menu.HideButtons ();
		PhotoBrowser.GetPhoto (menu.startingDirectory, GotFile, ".wav");
	}

	//callback from fileselector
	void GotFile(PhotoBrowser.Status status, string path){
		if (path != null) {
			StartCoroutine (PlayAudio (path));
		}
		menu.ShowButtons ();
	}

	IEnumerator PlayAudio(string _path){
		var newClip = new AudioClip ();
		var musicFile = new WWW (_path);
		newClip = musicFile.GetAudioClip (false, false, AudioType.WAV);
		while(newClip.loadState != AudioDataLoadState.Loaded){
			yield return null;
		}
		audioSource.clip = newClip;
		audioSource.Play ();
	}
}

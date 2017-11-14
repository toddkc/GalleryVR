using UnityEngine;
using UnityEngine.UI;

public class SelectPhotoButton : MonoBehaviour {

	[HideInInspector]public SelectPhotoButton clickedButton;
	[HideInInspector]public Button thisButton;
	[HideInInspector]public Text buttonText;
	Menu menu;
	public int arrayNumber;
	public bool isClickable = true;

	void Start(){
		clickedButton = this;
		thisButton = GetComponent<Button> ();
		thisButton.onClick.AddListener (OnClick);
		buttonText = GetComponentInChildren<Text> ();
		menu = GameObject.Find ("Manager").GetComponent<Menu>();
	}
		
	void OnClick(){
		if (isClickable) {
			SelectPhoto ();
		}
	}

	//called when select button clicked
	void SelectPhoto(){
		menu.HideButtons ();
		PhotoBrowser.GetPhoto (menu.startingDirectory, GotFile, ".*");
	}

	//callback from fileselector
	void GotFile(PhotoBrowser.Status status, string path){
		menu.ShowButtons ();
		if (path != null) {
			Gallery.current.photos [arrayNumber].photoAddress = path;
			buttonText.text = GetName ();
		}
	}

	string GetName(){
		var name = Gallery.current.photos [arrayNumber].photoAddress;
		string[] splitName = name.Split ("\\" [0]);
		var i = splitName.Length - 1;
		return splitName [i];
	}
}

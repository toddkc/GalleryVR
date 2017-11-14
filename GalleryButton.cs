using UnityEngine;
using UnityEngine.UI;

public class GalleryButton : MonoBehaviour {

	public bool isDeleteButton=false;
	public Gallery savedGallery;
	[HideInInspector]public Button thisButton;

	void Start(){
		thisButton = GetComponent<Button> ();
		thisButton.onClick.AddListener (OnClick);
	}

	void OnClick(){
		if(isDeleteButton){
			GameObject.Find ("Manager").GetComponent<Menu> ().DeleteLayout (savedGallery);
		}else{
			GameObject.Find ("Manager").GetComponent<Menu> ().LoadLayout (savedGallery);
		}
	}
}

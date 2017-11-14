using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class Menu : MonoBehaviour {

	#region Variables
	public GameObject menuCamera;
	[Tooltip("Input Field for naming gallery layout.")]
	public InputField layoutName;
	[Tooltip("Button Prefab for load/delete panels.")]
	public GameObject galleryButtonPrefab;
	[Tooltip("Load Panel to add Gallery Buttons to.")]
	public GameObject loadListPanel;
	[Tooltip("Delete Panel to add Gallery Buttons to.")]
	public GameObject deleteListPanel;
	[HideInInspector]
	public string startingDirectory;
	public Image mainPanelBackground;
	public GameObject mainMenuCanvas;
	public GameObject newPanel;
	[Tooltip("The parent loading panel.")]
	public GameObject loadPanel;
	[Tooltip("The parent delete panel.")]
	public GameObject deletePanel;
	[Tooltip("All the select photo buttons.")]
	public GameObject[]selectPhotoButtons;
	GalleryManager galleryManager;
	List<GameObject>galleryButtons;
	[Tooltip("This will delete the saved file.")]
	public bool eraseAll;

	#endregion

	void Awake(){
		if(eraseAll){
			DeleteSavedData ();
		}
	}

	void Start(){
		ToggleView ();
		galleryManager = GetComponent<GalleryManager> ();
		galleryButtons = new List<GameObject> ();
		NewLayout ();
		LoadLayoutList (loadListPanel, false);
		startingDirectory = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyPictures);
	}

	void Update(){
		if(menuHidden){
			if(Input.GetKeyDown(KeyCode.Space)){
				HideMenu ();
			}
		}
	}

	//delete the saved file
	void DeleteSavedData(){
		SaveLoad.DeleteSavedData ();
	}

	//quit the program
	public void Quit(){
		Application.Quit ();
	}

	//called during start, creates new gallery but doesn't save it
	public void NewLayout(){
		Gallery.current = new Gallery ();
	}

	//clears the gallery buttons list so it can be reloaded
	void ClearList(){
		foreach (var item in galleryButtons) {
			Destroy (item);
		}
		galleryButtons.Clear ();
	}

	//creates buttons for each gallery to load or delete, needs to know if loading or deleting
	void LoadLayoutList(GameObject _panel, bool _delete){
		ClearList ();
		SaveLoad.Load ();
		foreach (Gallery g in SaveLoad.savedGalleries) {
			var galleryButton = Instantiate (galleryButtonPrefab, _panel.transform);
			galleryButtons.Add (galleryButton);
			galleryButton.GetComponentInChildren<Text> ().text = g.galleryName;
			galleryButton.GetComponent<GalleryButton> ().savedGallery = g;
			if(_delete){
				galleryButton.GetComponent<GalleryButton> ().isDeleteButton = true;
			}
		}
	}

	//called after inputfield focus, sets the name of the gallery
	public void SaveName(){
		Gallery.current.galleryName = layoutName.text;
	}

	//called when Save button is clicked
	public void SaveLayout(){
		if (Gallery.current.galleryName != "" && Gallery.current.photos[0].photoAddress != "") {
			SaveLoad.Save ();
			layoutName.text = null;
			galleryManager.LoadGallery ();
			LoadLayoutList (loadListPanel, false);
			ClearButtonText ();
		}else{
			Debug.Log ("Gallery not named or photo not selected!");
		}
	}

	void ClearButtonText(){
		foreach (var item in selectPhotoButtons) {
			item.GetComponentInChildren<Text> ().text = "Select Photo";
		}
	}

	//called when gallery button is clicked on load panel
	public void LoadLayout(Gallery _gallery){
		Gallery.current = _gallery;
		galleryManager.LoadGallery ();
		ClearButtonText ();
	}

	//called when gallery button is clicked on delete panel
	public void DeleteLayout(Gallery _gallery){
		SaveLoad.DeleteGallery (_gallery);
		LoadLayoutList (deleteListPanel, true);
		ClearButtonText ();
	}

	//called when delete layout button clicked
	public void OpenDeletePanel(){
		deletePanel.SetActive (true);
		LoadLayoutList (deleteListPanel, true);
		loadPanel.SetActive (false);
	}

	//called when cancel button is clicked
	public void CloseDeletePanel(){
		loadPanel.SetActive (true);
		LoadLayoutList (loadListPanel, false);
		deletePanel.SetActive (false);
	}
		
	public void HideButtons(){
		newPanel.SetActive (false);
		loadPanel.SetActive (false);
	}
	public void ShowButtons(){
		newPanel.SetActive (true);
		loadPanel.SetActive (true);
	}

	bool showingVR=true;
	public void ToggleView(){
		if(showingVR){
			showingVR = false;
			mainPanelBackground.enabled = true;
			menuCamera.SetActive (true);
			VRSettings.showDeviceView = false;
		}else{
			showingVR = true;
			mainPanelBackground.enabled = false;
			menuCamera.SetActive (false);
			VRSettings.showDeviceView = true;
		}
	}

	bool menuHidden=false;
	public void HideMenu(){
		if(menuHidden){
			mainMenuCanvas.SetActive (true);
			menuHidden = false;
		}else{
			mainMenuCanvas.SetActive (false);
			menuHidden = true;
		}
	}
}

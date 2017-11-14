using UnityEngine;

public class GalleryManager : MonoBehaviour {

	public Mural[]murals;

	//loads all 16 photos
	public void LoadGallery(){
		for (int i = 0; i < murals.Length; i++) {
			murals [i].GetComponent<Mural> ().SetupMural (i);
		}
	}
}

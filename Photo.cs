using UnityEngine;

[System.Serializable]
public class Photo {

	public string photoAddress;
	public string photoOrientation;

	public Photo(){
		this.photoAddress="";
		this.photoOrientation = "";
	}
}

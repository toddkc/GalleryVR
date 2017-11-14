using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class Mural : MonoBehaviour {

	VideoPlayer vidPlayer;
	RawImage rawImage;
	GameObject vidPlayerObject;
	GameObject rawImageObject;

	void Start(){
		vidPlayer = GetComponentInChildren<VideoPlayer> ();
		rawImage = GetComponentInChildren<RawImage> ();

		vidPlayerObject = transform.Find ("VideoPlayer").gameObject;
		rawImageObject = transform.Find ("ImageCanvas").gameObject;

		rawImageObject.SetActive (false);
		vidPlayerObject.SetActive (false);
	}

	public void SetupMural(int _number){
		string photo = Gallery.current.photos [_number].photoAddress;

		if(photo!=""){
			CheckFile (photo);
		}
	}

	void CheckFile(string _file){
		string imgType = _file.Substring (_file.Length - 3);
		imgType=imgType.ToUpper ();
		Debug.Log (imgType);
		if (imgType == "MP4" || imgType == "GIF") {
			vidPlayerObject.SetActive (true);
			rawImageObject.SetActive (true);
			StartCoroutine (PlayVideo(_file));
		}else if(imgType=="JPG" || imgType=="PNG"){
			rawImageObject.SetActive (true);
			Texture2D tex = new Texture2D (2, 2);
			byte[] fileData = File.ReadAllBytes (_file);
			tex.LoadImage (fileData);
			rawImage.texture = tex;
		}
	}

	IEnumerator PlayVideo(string _video){
		vidPlayer.source = VideoSource.Url;
		vidPlayer.url = _video;
		vidPlayer.Prepare ();
		while(!vidPlayer.isPrepared){
			yield return null;
		}
		rawImage.texture = vidPlayer.texture;
		vidPlayer.Play ();
	}
}

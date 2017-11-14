using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoad : MonoBehaviour {

	public static List<Gallery>savedGalleries = new List<Gallery>();

	public static void DeleteSavedData(){
		if (File.Exists (Application.persistentDataPath + "/saved.gal")) {
			File.Delete (Application.persistentDataPath + "/saved.gal");
		}
	}

	public static void Save(){
		if (Gallery.current.galleryName != "") {
			SaveLoad.savedGalleries.Add (Gallery.current);
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + "/saved.gal");
			bf.Serialize (file, SaveLoad.savedGalleries);
			file.Close ();
		}
	}

	public static void Load(){
		if(File.Exists (Application.persistentDataPath + "/saved.gal")){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/saved.gal", FileMode.Open);
			SaveLoad.savedGalleries = (List<Gallery>)bf.Deserialize (file);
			file.Close ();
		}
	}

	public static void DeleteGallery(Gallery _gallery){
		savedGalleries.Remove (_gallery);
		DeleteSavedData ();
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/saved.gal");
		bf.Serialize (file, SaveLoad.savedGalleries);
		file.Close ();
	}
}

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Gallery {

	public static Gallery current;
	public string galleryName = "";
	public Photo photo01;
	public Photo photo02;
	public Photo photo03;
	public Photo photo04;
	public Photo photo05;
	public Photo photo06;
	public Photo photo07;
	public Photo photo08;
	public Photo photo09;
	public Photo photo10;
	public Photo photo11;
	public Photo photo12;
	public Photo photo13;
	public Photo photo14;
	public Photo photo15;
	public Photo photo16;

	public List<Photo> photos;

	public Gallery(){
		galleryName = "";
		photo01 = new Photo ();
		photo02 = new Photo ();
		photo03 = new Photo ();
		photo04 = new Photo ();
		photo05 = new Photo ();
		photo06 = new Photo ();
		photo07 = new Photo ();
		photo08 = new Photo ();
		photo09 = new Photo ();
		photo10 = new Photo ();
		photo11 = new Photo ();
		photo12 = new Photo ();
		photo13 = new Photo ();
		photo14 = new Photo ();
		photo15 = new Photo ();
		photo16 = new Photo ();

		photos = new List<Photo> ();
		photos.Add (photo01);
		photos.Add (photo02);
		photos.Add (photo03);
		photos.Add (photo04);
		photos.Add (photo05);
		photos.Add (photo06);
		photos.Add (photo07);
		photos.Add (photo08);
		photos.Add (photo09);
		photos.Add (photo10);
		photos.Add (photo11);
		photos.Add (photo12);
		photos.Add (photo13);
		photos.Add (photo14);
		photos.Add (photo15);
		photos.Add (photo16);
	}
}

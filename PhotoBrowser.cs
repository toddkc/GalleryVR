using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class PhotoBrowser : MonoBehaviour {

	public enum Status
	{
		Successful, //Used if we successfully got a file
		Cancelled, //Used if the 'Cancel' button is clicked
		Failed, //Used if the Close() method is called while the window is open
		Destroyed, //Used if the instance is destroyed while the window is open
	}

	public delegate void PhotoSelected(Status status, string path);
	public PhotoSelected Callback;

	public string extension = ".*";

	public bool open;

	string path = "";
	string file = "";
	int selection;

	static GameObject updater;

	public Rect windowDimensions = new Rect(0,0,1200,600);
	Vector2 scrollPosition;

	private static GUIStyle _windowStyle;
	public static GUIStyle windowStyle
	{
		get
		{
			if(_windowStyle == null) _windowStyle = GUI.skin.window;
			return _windowStyle;
		}
		set { _windowStyle = value ?? GUI.skin.window; }
	}

	/// <summary>
	/// 	- The style for buttons in the window.
	/// </summary>
	private static GUIStyle _buttonStyle;
	public static GUIStyle buttonStyle
	{
		get
		{
			if(_buttonStyle == null) _buttonStyle = GUI.skin.button;
			return _buttonStyle;
		}
		set { _buttonStyle = value ?? GUI.skin.button; }
	}

	/// <summary>
	/// 	- The style for labels in the window.
	/// </summary>
	private static GUIStyle _labelStyle;
	public static GUIStyle labelStyle
	{
		get 
		{ 
			if(_labelStyle == null) _labelStyle = GUI.skin.label;
			return _labelStyle;
		}
		set { _labelStyle = value ?? GUI.skin.label; }
	}

	/// <summary>
	/// 	- The style for titles in the window.
	/// </summary>
	private static GUIStyle _titleStyle;
	public static GUIStyle titleStyle
	{
		get 
		{ 
			if(_titleStyle == null) _titleStyle = GUI.skin.label;
			return _titleStyle;
		}
		set { _titleStyle = value ?? GUI.skin.label; }
	}

	/// <summary>
	/// 	- The style for text fields in the window.
	/// </summary>
	private static GUIStyle _textFieldStyle;
	public static GUIStyle textFieldStyle
	{
		get 
		{ 
			if(_textFieldStyle == null) _textFieldStyle = GUI.skin.textField;
			return _textFieldStyle;
		}
		set { _textFieldStyle = value ?? GUI.skin.textField; }
	}

	void OnDestroy(){
		if(open && Callback != null){
			Callback (Status.Destroyed, "");
		}
	}

	private void OnGUI()
	{	
		if(open)
		{
			windowDimensions.center = new Vector2(Screen.width*0.5f, Screen.height*0.5f);
			GUI.Window(0, windowDimensions, OpenPhotoBrowser, "Select a "+extension+" File");
		}
	}

	void OpenPhotoBrowser(int ID){
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);

		//Path Buttons
		GUILayout.Label("Path : ", titleStyle);

		GUILayout.BeginHorizontal();

		string[] parentDirectories = GetParentDirectories(path);

		float maximumWidth = windowDimensions.width - 30;
		float totalWidth = 0;
		float width = 0;
		float spacingWidth = 11; //public variable?
		float arrowWidth = labelStyle.CalcSize(new GUIContent(" > ")).x;
		float arrowSpacing = arrowWidth + spacingWidth;

		for(int i = 0; i < parentDirectories.Length; i++){
			width = buttonStyle.CalcSize(new GUIContent(parentDirectories[i])).x;

			totalWidth += (width + spacingWidth);
			if(totalWidth > maximumWidth)
			{
				totalWidth = width;
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}

			if(GUILayout.Button(parentDirectories[i], buttonStyle, GUILayout.Width(width)))
			{
				path = GetParentDirectories(path, true)[i];
				break;
			}

			GUILayout.Label(" > ", labelStyle, GUILayout.Width(arrowWidth));
			totalWidth += (arrowSpacing);

			if(totalWidth > maximumWidth)
			{
				totalWidth = 0;
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
		}

		string currentDirectory = new FileInfo(path).Name;
		width = buttonStyle.CalcSize(new GUIContent(currentDirectory)).x;
		GUILayout.Label(currentDirectory, buttonStyle, GUILayout.Width(width));

		GUILayout.EndHorizontal();

		GUILayout.Space(1);

		//Directory Buttons
		GUILayout.Label("Directories : ", titleStyle);

		GUILayout.BeginHorizontal();

		string[] childDirectories = GetChildDirectories(path);
		float buttonWidth = (windowDimensions.width - 80) / 10f;

		for(int i = 0; i < childDirectories.Length; i++){
			if(GUILayout.Button(childDirectories[i], buttonStyle, GUILayout.Width(buttonWidth)))
			{
				path = GetChildDirectories(path, true)[i];
				break;
			}

			GUILayout.Space(10);

			if((i % 4) == 3)
			{
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
		}

		GUILayout.EndHorizontal();

		GUILayout.Space(1);

		//File Buttons
		GUILayout.Label("Files : ", titleStyle);

		GUILayout.BeginHorizontal();

		string[] files = GetFiles(path, extension : extension);

		for(int i = 0; i < files.Length; i++){
			if(GUILayout.Button(files[i], buttonStyle, GUILayout.Width(buttonWidth)))
			{
				file = files[i];
				if(Callback != null) Callback(Status.Successful, path+@"\"+file);
				open = false;
				Destroy (gameObject);
			}

			GUILayout.Space(5);

			if((i % 9) == 8)
			{
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
	}

	public static void GetPhoto(string _startingDirectory, PhotoSelected Callback = null, string _extension = ".*"){
		if(updater==null){
			updater = new GameObject ("Select Photo");
			updater.AddComponent<PhotoBrowser_Cleanup> ();
		}

		PhotoBrowser instance = updater.AddComponent<PhotoBrowser> ();

		instance.Callback = Callback;
		instance.extension = _extension;
		instance.open = true;
		instance.path = _startingDirectory;
	}



	static string[] GetParentDirectories(string _filePath, bool _includePaths = false){
		List<string> parents = new List<string>();
		FileInfo fileInfo;

		while(true){
			try{
				fileInfo = new FileInfo(_filePath);
				if(!_includePaths) parents.Add(fileInfo.Directory.Name);
				else parents.Add(fileInfo.Directory.FullName);

				_filePath = fileInfo.Directory.FullName;
			}

			catch{ break; }
		}

		parents.Reverse();
		return parents.ToArray();		
	}

	static string[] GetChildDirectories(string directoryPath, bool includePaths = false){
		DirectoryInfo directory;

		if(Directory.Exists(directoryPath)) directory = new DirectoryInfo(directoryPath);
		else
		{
			try{ directory = new FileInfo(directoryPath).Directory;	}
			catch{ return new string[0]; }
		}

		List<string> children = new List<string>();

		try{
			DirectoryInfo[] directories = directory.GetDirectories();

			foreach(DirectoryInfo childDir in directories){
				if(!includePaths) children.Add(childDir.Name);
				else children.Add(childDir.FullName);
			}			
		}

		catch{
			children = new List<string>();
		}

		return children.ToArray();
	}

	static string[] GetFiles(string directoryPath, bool includePaths = false, string extension = ".*"){
		DirectoryInfo directory;

		if(Directory.Exists(directoryPath)) directory = new DirectoryInfo(directoryPath);
		else
		{
			try{ directory = new FileInfo(directoryPath).Directory;	}
			catch{ return new string[0]; }
		}

		List<string> files = new List<string>();

		try{
			FileInfo[] fileInfos = directory.GetFiles("*"+extension);

			foreach(FileInfo fileInfo in fileInfos){
				if(!includePaths) files.Add(fileInfo.Name);	
				else files.Add(fileInfo.FullName);	
			}
		}

		catch{
			files = new List<string>();
		}

		return files.ToArray();
	}

}

public class PhotoBrowser_Cleanup : MonoBehaviour
{
	void OnApplicationQuit()
	{
		Destroy(gameObject);
	}

	void OnLevelLoaded()
	{
		Destroy(gameObject);
	}
}

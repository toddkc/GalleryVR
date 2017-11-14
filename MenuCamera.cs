using UnityEngine;
using UnityEngine.VR;

public class MenuCamera : MonoBehaviour {

	bool showingVR;

	void Start(){
		VRSettings.showDeviceView = false;
		showingVR = false;
	}

	public void ToggleView(){
		if(showingVR){
			VRSettings.showDeviceView = false;
		}else{
			VRSettings.showDeviceView = true;
		}
	}
}

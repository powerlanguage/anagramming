using UnityEngine;
using System.Collections;

public class CloseModalScript : MonoBehaviour {

	public GameObject modal;

	// Use this for initialization
	void Start () {
	}

	void OnMouseUp(){
		if (modal.activeSelf) {
			modal.SetActive (false);
		}
	}
}

using UnityEngine;
using System.Collections;

public class SkipWordScript : MonoBehaviour {

	public GameObject gameManager;
	private GameManagerScript gameManagerScript;
	public GameObject modal;

	// Use this for initialization
	void Start () {
		gameManagerScript = gameManager.GetComponent<GameManagerScript> ();
	}

	void OnMouseUp(){
		gameManagerScript.SkipWord();
		modal.SetActive (false);
		Debug.Log("skip");
	}
}

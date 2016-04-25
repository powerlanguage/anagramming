using UnityEngine;
using System.Collections;

public class SkipWordScript : MonoBehaviour {

	public GameObject gameManager;
	private GameManagerScript gameManagerScript;

	// Use this for initialization
	void Start () {
		gameManagerScript = gameManager.GetComponent<GameManagerScript> ();
	}

	void OnMouseUp(){
		gameManagerScript.SkipWord();
		gameManagerScript.skipModal.SetActive (false);
		Debug.Log("skip");
	}
}

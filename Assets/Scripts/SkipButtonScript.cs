using UnityEngine;
using System.Collections;

public class SkipButtonScript : MonoBehaviour {

	public GameObject gameManager;
	private GameManagerScript gameManagerScript;

	// Use this for initialization
	void Start () {
		gameManagerScript = gameManager.GetComponent<GameManagerScript> ();
	}

	void OnMouseUp(){
		gameManagerScript.SkipWord();
		Debug.Log("skip");
	}

}

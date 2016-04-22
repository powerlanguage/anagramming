using UnityEngine;
using System.Collections;

public class ShuffleButtonScript : MonoBehaviour {

	public GameObject handRack;
	private RackScript handRackScript;

	// Use this for initialization
	void Start () {
		handRackScript = handRack.GetComponent<RackScript> ();
	}

	void OnMouseUp(){
		handRackScript.shuffleRack();
		Debug.Log("shuffle");
	}

}

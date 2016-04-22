using UnityEngine;
using System.Collections;

public class RecallButtonScript : MonoBehaviour {

	public GameObject handRack;
	private RackScript handRackScript;

	// Use this for initialization
	void Start () {
		handRackScript = handRack.GetComponent<RackScript> ();
	}

	void OnMouseUp(){
		handRackScript.RecallTilesToRack();
		Debug.Log("shuffle");
	}

}

using UnityEngine;
using System.Collections;

public class ShuffleButtonScript : MonoBehaviour {

	public GameObject handRack;
	private RackScript handRackScript;
	private BoxCollider2D bc;
	private SpriteRenderer sr;
	public Sprite tileInactive;
	public Sprite tileActive;


	// Use this for initialization
	void Start () {
		handRackScript = handRack.GetComponent<RackScript> ();
		bc = this.GetComponent<BoxCollider2D> ();
		sr = this.GetComponent<SpriteRenderer> ();
	}

	void Update(){
		//Enable/disable shuffle and change sprite if hand rack has less than one tile
		if (handRackScript.GetNumOccupiedSlots () > 1) {
			bc.enabled = true;
			sr.sprite = tileActive;
		} else {
			sr.sprite = tileInactive;
			bc.enabled = false;
		}
	}

	void OnMouseUp(){
		handRackScript.shuffleRack();
		Debug.Log("shuffle");
	}

}

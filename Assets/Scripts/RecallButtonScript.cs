using UnityEngine;
using System.Collections;

public class RecallButtonScript : MonoBehaviour {

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
		if (handRackScript.GetNumUnoccupiedSlots() >= 1) {
			bc.enabled = true;
			sr.sprite = tileActive;
		} else {
			bc.enabled = false;
			sr.sprite = tileInactive;
		}
	}

	void OnMouseUp(){
		handRackScript.RecallTilesToRack();
		Debug.Log("recall");
	}

}

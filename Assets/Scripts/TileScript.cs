using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

	public string letter;

	//Vars for detecting taps vs drags
	public float maxTapDuration = 0.09f;
	private float tapDuration = 0.0f;
	private bool detectingTap = false;
	private Vector3 offset;
	private Vector3 screenPoint;
	private TileState tileState = TileState.MOVING;

	void Awake(){
		
	}

	void Update(){

		//If tap is underway, add last frame time to tap duration
		if (detectingTap) {
			tapDuration += Time.deltaTime;
		}
	}

	public void SetLetter(string s){
		letter = s;
	}

	public string GetLetter(){
		return letter;
	}

	void OnMouseDown(){
		detectingTap = true;
		// Set the offset of the tile, meaning that the tile won't have to be dragged based on it's center
		// Should the z offset be
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
	}

	void OnMouseDrag(){
		this.tileState = TileState.DRAGGED;
		// Get the current position of the mouse, add the offset and then set that to the tile's position
		Vector3 curMouseScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0.0f);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curMouseScreenPoint) + offset;
		transform.position = curPosition;
	}

	void OnMouseUp(){
		detectingTap = false;
		if (tapDuration < maxTapDuration) {
			//Tap detected!
			Debug.Log("Tap Detected");
			SendMessageUpwards ("TileTapped", this.gameObject);
		}
		tapDuration = 0;
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log (this.gameObject.name + " collided with " + other.gameObject.name);
	}
	
	//Get the slot this tile is a child of
	private GameObject GetSlot(){
		return this.transform.parent.gameObject;
	}

	//Get the rack this tile is a child of
	private GameObject GetRack(){
		return this.transform.parent.parent.gameObject;
	}

}

public enum TileState {
	HOME,
	MOVING,
	DRAGGED
}
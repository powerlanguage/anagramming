using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {

	public string letter;

	//Dtecting taps vs drags
	public float maxTapDuration = 0.09f;
	private float tapDuration = 0.0f;
	private bool detectingTap = false;
	//Dragging with offset
	private Vector3 offset;
	//Snapping to target
	public float homeRadius;
	public float moveDelta;
	public TileState tileState = TileState.MOVING;
	public GameObject target;
	//Collisions
	private HashSet<GameObject> collisions = new HashSet<GameObject>();

	void Awake(){
		
	}

	void Update(){

		switch (this.tileState) {
		case TileState.MOVING:
			//Ease towards home

			Vector3 currPos = this.transform.position;
			Vector3 targetPos = target.transform.position;

			this.transform.position = Vector3.Lerp (currPos, targetPos, moveDelta);

			//Check if Tile is now at home
			float homeDistance = Vector3.Distance(this.transform.position, targetPos);
			if (homeDistance < homeRadius) {
				this.tileState = TileState.HOME;
			}


			break;
		case TileState.DRAGGED:
			break;
		case TileState.HOME:
			break;
		
		}

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
		this.transform.position = curPosition;
	}

	void OnMouseUp(){
		detectingTap = false;
		if (tapDuration < maxTapDuration) {
			//Tap detected!
			Debug.Log ("Tap Detected");
			SendMessageUpwards ("TileTapped", this.gameObject);
		} else if (collisions.Count > 0) {
			Debug.Log (collisions.Count);
			//Drag collision detected!
			GameObject closestSlot = null;
			//Must be a better way to do this?  Maybe with NaN?
			float shortestDist = 9999999f;

			foreach (GameObject slot in collisions){
				float dist = Vector3.Distance (this.transform.position, slot.transform.position);
				if (dist < shortestDist) {
					Debug.Log (slot.name);
					shortestDist = dist;
					closestSlot = slot;
				}
			}

			GameObject[] collisionParams = {this.gameObject, closestSlot};
			SendMessageUpwards ("TileCollided", collisionParams);
			collisions.Clear ();
		}
			
		tapDuration = 0;
		this.tileState = TileState.MOVING;
	}
		
	//When a tile overlaps a slot, we add it to the collisions list.
	//When a tile no longer overlaps a slot, we remove it from the collisions list.
	//When the mouse is released, we check the collisions list and set the target to the closest collision.

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log (this.gameObject.name + " collided with " + other.gameObject.name);
		collisions.Add (other.gameObject);
	}

	void OnTriggerExit2D(Collider2D other){
		collisions.Remove (other.gameObject);
	}
	
	//Get the slot this tile is a child of
	private GameObject GetSlot(){
		return this.transform.parent.gameObject;
	}

	//Get the rack this tile is a child of
	private GameObject GetRack(){
		return this.transform.parent.parent.gameObject;
	}

	public void SetTarget(GameObject target){
		//send message upwards?
		//SendMessageUpwards("SetTileTarget", this.gameObject, target);
		this.target = target;
		//Let's move in case we were at home
		this.tileState = TileState.MOVING;
	}

}

public enum TileState {
	HOME,
	MOVING,
	DRAGGED
}
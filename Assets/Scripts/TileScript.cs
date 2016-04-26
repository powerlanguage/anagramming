using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {

	public string letter;
	public GameObject letterMesh;
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
	//
	public float staticZ;
	public float movingZ;

	void Awake(){
		
	}

	void Update(){

		switch (this.tileState) {
		case TileState.MOVING:
			//Ease towards home

			//Don't try and ease on the Z axis - note we use this.z as the target
			Vector3 currPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
			Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y, this.transform.position.z);

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
			//Reset z-index if the tile was being dragged
			if (this.transform.position.z < staticZ) {
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, staticZ);
			}
			break;
		
		}

		//If tap is underway, add last frame time to tap duration
		if (detectingTap) {
			tapDuration += Time.deltaTime;
		}
	}

	public void SetLetter(string s){
		letterMesh.GetComponent<TextMesh> ().text = s.ToUpper();
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
		// Using movingZ to bring the dragged tile infront of everything else
		Vector3 curMouseScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, movingZ);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curMouseScreenPoint) + offset;
		this.transform.position = curPosition;
	}

	void OnMouseUp(){
		detectingTap = false;
		if (tapDuration < maxTapDuration) {
			//Tap detected!
			Debug.Log ("Tap Detected " + GetRack ().name + " " + this.gameObject.name);
			SendMessageUpwards ("TileTapped", this.gameObject);
		} else if (collisions.Count > 0) {
			//Drag collision detected!
			GameObject closestSlot = null;
			//Must be a better way to do this?  Maybe with NaN?
			float shortestDist = float.MaxValue;

			foreach (GameObject slot in collisions){
				float dist = Vector3.Distance (this.transform.position, slot.transform.position);
				if (dist < shortestDist) {
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
		
	//When a tile overlaps a slot, and tile is being dragged, we add it to the collisions list.
	//When a tile no longer overlaps a slot, we remove it from the collisions list.
	//When the mouse is released, we check the collisions list and set the target to the closest collision.

	void OnTriggerEnter2D(Collider2D other){
		if (this.tileState == TileState.DRAGGED) {
			Debug.Log (GetRack ().name + " " + this.gameObject.name + " collided with " + other.gameObject.transform.parent.name + " " + other.gameObject.name);
			collisions.Add (other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		collisions.Remove (other.gameObject);
	}
	
	//Get the slot this tile is a child of
	public GameObject GetSlot(){
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
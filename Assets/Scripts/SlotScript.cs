using UnityEngine;
using System.Collections;

public class SlotScript : MonoBehaviour {

	public bool isOccupied = false;
	private GameObject tile = null;
	private BoxCollider2D bc;

	public void Awake(){
		bc = this.GetComponent<BoxCollider2D> ();
	}

	void Update(){

		//Disable box Collider if slot is occupied
		//Stops collisions from happening
//		if (isOccupied) {
//			bc.enabled = false;
//		} else {
//			bc.enabled = true;
//		}
			
	}

	//Sets the tile as target, with easing
	public void AddTile(GameObject tile){
		this.tile = tile;
		isOccupied = true;
		tile.GetComponent<TileScript> ().SetTarget (this.gameObject);

		//This is required for moving with set target to work correctly
		//Don't fully understand how the relative coords work
		tile.transform.SetParent (this.transform);
		Debug.Log("Setting " + tile.name + " to target " + this.transform.parent.name + " " + this.name);
	}

	// Sets the tile without Easing
	// Could be Set tile?
	public void AddTileDirectly(GameObject tile){
		this.tile = tile;
		isOccupied = true;
		//Don't keep world position when setting parent slot
		//Not sure if this will break things
		tile.transform.SetParent (this.transform, false);
	}

	public void ClearSlot(){
		this.tile = null;
		isOccupied = false;
	}

	public bool IsSlotOccupied(){
		return isOccupied;
	}

	public GameObject GetTile(){
		return tile;
	}

	//Get the rack this slot is a child of
	public GameObject GetRack(){
		return this.transform.parent.gameObject;
	}
}

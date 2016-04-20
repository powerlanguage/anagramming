using UnityEngine;
using System.Collections;

public class SlotScript : MonoBehaviour {

	private bool isOccupied = false;
	private GameObject tile = null;

	public void Awake(){
		//box collider stuff
	}

	//Sets the tile as target, with easing
	public void AddTile(GameObject tile){
		this.tile = tile;
		isOccupied = true;
		tile.GetComponent<TileScript> ().SetTarget (this.gameObject);

		//This is required for moving with set target to work correctly
		tile.transform.SetParent (this.transform);
	}

	// Sets the tile without Easing
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

	public bool isSlotOccupied(){
		return isOccupied;
	}

	public GameObject GetTile(){
		return tile;
	}
}

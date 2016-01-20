using UnityEngine;
using System.Collections;

public class RackScript : MonoBehaviour {

	public int numSlots;
	public float slotWidth;
	public float spacerWidth;
	public GameObject slotPrefab;

	GameObject[] slots;

	//Worth storing SlotScript references in array?

	void Start(){
		Setup (7);
	}

	public void Setup(int numSlots){
		this.numSlots = numSlots;
		CreateSlots ();
	}

	public void CreateSlots(){

		slots = new GameObject [numSlots];

		// Create the slots for the rack
		for (int i = 0; i < numSlots; i++) {
			GameObject newSlot = (GameObject)Instantiate(slotPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
			slots[i] = newSlot;
			//Slots are positioned relative to their parent rack
			Vector3 targetPosition = new Vector3 ((i * slotWidth) + (i * spacerWidth), 0, 0 );
			newSlot.transform.position = targetPosition;
			//Don't keep world position when setting parent rack
			newSlot.transform.SetParent (this.transform, false);
			newSlot.tag = "slot";
			newSlot.name = newSlot.transform.parent.name + " slot " + i;
		}
	}

	private GameObject GetFirstEmptySlot(){
		foreach (GameObject slot in slots) {
			if(!slot.GetComponent<SlotScript>().isOccupied){
				return slot;
			}
		}
		// No empty slots
		return null;
	}

	//Add a tile to the first empty slot
	public void AddTileToFirstEmptySlot(GameObject tile){
		GameObject emptySlot = GetFirstEmptySlot ();
		if (emptySlot != null) {
			emptySlot.GetComponent<SlotScript>().AddTile(tile);
		}
	}

	//Add a tile to a specific slot
	//Checks if slot is in rack, that slot is empty then adds tile
	public void AddTileToSlot(GameObject tile, GameObject slot){
		if(ContainsSlot(slot)){
			SlotScript slotScript = slot.GetComponent<SlotScript>();
			if(!slotScript.isOccupied){
				slotScript.AddTile(tile);
			}
		}
	}

	//Looping over array to find element.  V.inefficient.  IndexOf works with Android?
	//Remove tile from a specific slot
	public void RemoveTileFromSlot(GameObject tile, GameObject slot){
		if(ContainsTile(tile) && ContainsSlot(slot)){
			SlotScript slotScript = slot.GetComponent<SlotScript>();
			if(slotScript.tile == tile){
				slotScript.ClearSlot();
			}
		}
	}

	public void RemoveTile(GameObject tile){
		if(ContainsTile(tile)){
			GameObject slot = GetSlotContainingTile(tile);
			slot.GetComponent<SlotScript>().ClearSlot();
		}
	}

	//Loop over all slots and to check for tile
	public bool ContainsTile(GameObject tileToFind){
		foreach (GameObject slot in slots) {
			if(slot.GetComponent<SlotScript>().tile == tileToFind){
				return true;
			}
		}
		return false;
	}

	//Check if slot is in this rack
	private bool ContainsSlot(GameObject slotToFind){
		foreach (GameObject slot in slots) {
			if(slot == slotToFind){
				return true;
			}
		}
		return false;
	}
	
	public GameObject GetSlotContainingTile(GameObject tile){
		foreach (GameObject slot in slots) {
			if(slot.GetComponent<SlotScript>().tile == tile){
				return slot;
			}
		}
		return null;
	}

	//Returns null if there is no tile in the slot
	public GameObject GetTileInSlot(GameObject slot){
		return slot.GetComponent<SlotScript>().tile;
	}

	//Loop over slots.  For those that have a tile, add its letter to the string
	public string GetRackString(){
		string rackString = "";
		foreach (GameObject slot in slots) {
			SlotScript slotScript = slot.GetComponent<SlotScript>();
			if(slotScript.isOccupied){
				rackString += slotScript.tile.GetComponent<TileScript>().letter;
			}
		}
		return rackString;
	}

	public void ClearRack(){
		foreach (GameObject slot in slots) {
			slot.GetComponent<SlotScript>().ClearSlot();
		}
	}

}

using UnityEngine;
using System.Collections;

public class RackScript : MonoBehaviour {

	public int numSlots;
	public float slotWidth;
	public float spacerWidth;
	public GameObject slotPrefab;

	GameObject[] slots;

	public void AddSlot(){
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
		if(ContainsSlot(slot){
			SlotScript slotScript = slot.GetComponent<SlotScript>();
			if(!slotScript.isOccupied){
				slotScript.AddTile(tile);
			}
		}
	}

	public void RemoveTile(GameObject tile){

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

	public string GetRackString(){

	}

	public void ClearRack(){

	}

}

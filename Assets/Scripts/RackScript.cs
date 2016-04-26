using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RackScript : MonoBehaviour {

	public float slotWidth;
	public float spacerWidth;
	public GameObject slotPrefab;
	public GameObject[] slots;
	public GameObject otherRack;
	private RackScript otherRackScript;



	//Worth storing SlotScript references in array?

	void Start(){
		otherRackScript = otherRack.GetComponent<RackScript> ();
	}

	private GameObject GetFirstEmptySlot(){
		foreach (GameObject slot in slots) {
			if(!slot.GetComponent<SlotScript>().IsSlotOccupied()){
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
			if(!slotScript.IsSlotOccupied()){
				slotScript.AddTile(tile);
			}
		}
	}

	//Looping over array to find element.  V.inefficient.  IndexOf works with Android?
	//Remove tile from a specific slot
	public void RemoveTileFromSlot(GameObject tile, GameObject slot){
		if(ContainsTile(tile) && ContainsSlot(slot)){
			SlotScript slotScript = slot.GetComponent<SlotScript>();
			if(slotScript.GetTile() == tile){
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
			if(slot.GetComponent<SlotScript>().GetTile() == tileToFind){
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
			if(slot.GetComponent<SlotScript>().GetTile() == tile){
				return slot;
			}
		}
		return null;
	}

	//Returns null if there is no tile in the slot
	public GameObject GetTileInSlot(GameObject slot){
		return slot.GetComponent<SlotScript>().GetTile();
	}

	//Returns all the tiles in the current rack
	public GameObject[] GetTiles(){
		List<GameObject> tiles = new List<GameObject> ();
		foreach (GameObject slot in slots) {
			GameObject tile = slot.GetComponent<SlotScript> ().GetTile ();
			if (tile != null) {
				tiles.Add (tile);
			}
		}
		return tiles.ToArray ();

	}

	//TODO:
	//Compare shuffled list to starting list
	//Keep shuffling until they are different
	//handle case of 1 rack shuffle
	public void shuffleRack(){
		List<GameObject> tilesToShuffle = new List<GameObject> ();
		//Remove tiles from slots and add to a list
		foreach (GameObject slot in slots) {
			SlotScript slotScript = slot.GetComponent<SlotScript>();
			if(slotScript.IsSlotOccupied()){
				tilesToShuffle.Add(slotScript.GetTile());
				slotScript.ClearSlot();
			}
		}
		//Shuffle the list
		GameObject[] tiles = tilesToShuffle.ToArray();
		for (int i = 0; i < tiles.Length; i++) {
			GameObject temp = tiles[i];
			int randomIndex = Random.Range(i, tiles.Length);
			tiles[i] = tiles[randomIndex];
			tiles[randomIndex] = temp;
		}
		//Add back to the slots

		int tilesToAddIndex = 0;

		//Only add tiles slots that are empty in the play rack 
		for (int i = 0; i < slots.Length; i++) {
			Debug.Log(otherRackScript.slots [i].GetComponent<SlotScript> ().isOccupied);
			if (otherRackScript.slots [i].GetComponent<SlotScript> ().isOccupied == false) {
				AddTileToSlot (tiles [tilesToAddIndex], slots [i]);
				tilesToAddIndex++;
			}
		}
	}

	//Loop over slots.  For those that have a tile, add its letter to the string
	public string GetRackString(){
		string rackString = "";
		foreach (GameObject slot in slots) {
			SlotScript slotScript = slot.GetComponent<SlotScript>();
			if(slotScript.IsSlotOccupied()){
				rackString += slotScript.GetTile().GetComponent<TileScript>().letter;
			}
		}
		return rackString;
	}

	//Delete all the tiles and clear all the slots
	public void ClearRack(){
		foreach (GameObject slot in slots) {
			GameObject tile = GetTileInSlot (slot);
			Destroy (tile);
			slot.GetComponent<SlotScript>().ClearSlot();
		}
	}

	public int GetNumOccupiedSlots(){
		int occupiedSlots = 0;
		foreach (GameObject slot in slots) {
			if (slot.GetComponent<SlotScript> ().IsSlotOccupied ()) {
				occupiedSlots++;
			}
		}
		return occupiedSlots;
	}

	public int GetNumUnoccupiedSlots(){
		int unoccupiedSlots = 0;
		foreach (GameObject slot in slots) {
			if (slot.GetComponent<SlotScript> ().IsSlotOccupied () == false) {
				unoccupiedSlots++;
			}
		}
		return unoccupiedSlots;
	}

	//Recall all tiles from other rack to this rack
	public void RecallTilesToRack(){
		GameObject[] tiles = otherRackScript.GetTiles ();
		foreach (GameObject tile in tiles) {
			otherRackScript.RemoveTile (tile);
			AddTileToFirstEmptySlot (tile);
		}
	}

	public void TileTapped(GameObject tile){
		RemoveTile (tile);
		otherRackScript.AddTileToFirstEmptySlot (tile);
	}

	//Not sure if this way of passing params is the best
	public void TileCollided(GameObject[] collisionParams){
		GameObject currentTile = collisionParams [0];
		GameObject targetSlot = collisionParams [1];
		GameObject currentSlot = currentTile.GetComponent<TileScript> ().GetSlot ();

		SlotScript currentSlotScript = currentSlot.GetComponent<SlotScript> ();
		SlotScript targetSlotScript = targetSlot.GetComponent<SlotScript> ();

		//Fetch the rack scripts each slot belongs to, so we can add/remove appropriately
		RackScript currentRackScript = currentSlotScript.GetRack ().GetComponent<RackScript> ();
		RackScript targetRackScript = targetSlotScript.GetRack ().GetComponent<RackScript> ();

		if (targetSlotScript.isOccupied) {
			//target slot is occupied, so swap tiles
			GameObject tileInTargetSlot = targetSlotScript.GetTile();

			currentRackScript.RemoveTile(currentTile);
			targetRackScript.RemoveTile(tileInTargetSlot);
			currentRackScript.AddTileToSlot (tileInTargetSlot, currentSlot);
			targetRackScript.AddTileToSlot (currentTile, targetSlot);

			Debug.Log ("Target slot is occupied!");
		} else {
			//target slot is empty, just remove tile from current slot, and add to new slot
			currentRackScript.RemoveTile (currentTile);
			targetRackScript.AddTileToSlot (currentTile, targetSlot);

		}
	}
}

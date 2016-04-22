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
		for (int i = 0; i < tiles.Length; i++) {
			AddTileToFirstEmptySlot (tiles [i]);
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

	public void TileTapped(GameObject tile){
		RemoveTile (tile);
		otherRackScript.AddTileToFirstEmptySlot (tile);
	}

	//Not sure if this way of passing params is the best
	public void TileCollided(GameObject[] collisionParams){
		GameObject tile = collisionParams [0];
		GameObject newSlot = collisionParams [1];
		GameObject currSlot = tile.GetComponent<TileScript> ().GetSlot ();

		//First remove the tile from it's current slot
		if (ContainsSlot (currSlot)) {
			RemoveTile (tile);
		} else {
			otherRackScript.RemoveTile (tile);
		}

		//Then add it to the new slot
		if (ContainsSlot (newSlot)) {
			AddTileToSlot (tile, newSlot);
		} else {
			otherRackScript.AddTileToSlot (tile, newSlot);
		}
	}

}

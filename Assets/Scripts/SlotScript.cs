using UnityEngine;
using System.Collections;

public class SlotScript : MonoBehaviour {

	public bool isOccupied = false;
	public GameObject tile = null;

	GameObject rack;

	public void Awake(){
		this.tag = "slot";
	}

	public void AddTile(GameObject tile){
		this.tile = tile;
		isOccupied = true;
		//Don't keep world position when setting parent slot
		tile.transform.SetParent (this.transform, false);
	}

	public void ClearSlot(){
		this.tile = null;
		isOccupied = false;
	}
}

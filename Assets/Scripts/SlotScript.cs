using UnityEngine;
using System.Collections;

public class SlotScript : MonoBehaviour {

	public bool isOccupied = false;
	public GameObject tile = null;

	public void AddTile(GameObject tile){
		this.tile = tile;
		isOccupied = true;
	}

	public void ClearSlot(){
		this.tile = null;
		isOccupied = false;
	}
}

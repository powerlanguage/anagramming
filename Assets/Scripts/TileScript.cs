using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

	public string letter;

	//Vars for detecting taps vs drags
	public float maxTapDuration = 0.09f;
	private float tapDuration = 0.0f;
	private bool detectingTap = false;

	void Awake(){
		
	}

	void Update(){

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
	}

	void OnMouseUp(){
		detectingTap = false;
		if (tapDuration < maxTapDuration) {
			//Tap detected!
			Debug.Log("Tap Detected");
		}
		tapDuration = 0;
	}
	
	//Get the slot this tile is a child of
	private GameObject GetSlot(){
		return this.transform.parent.gameObject;
	}

	//Get the rack this tile is a child of
	private GameObject GetRack(){
		return this.transform.parent.parent.gameObject;
	}

}

///* Click on a tile
// * 	Is it at home?
// * 	What rack is it in?
// *  What is the other rack? (what slot is it's rack in?)
// *  Get the firstemptyslot in that rack
// *  Clear current slot
// *  Add tile to new slot
// *  
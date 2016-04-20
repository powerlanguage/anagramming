using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public GameObject tilePrefab;
	public GameObject handRack;
	private RackScript handRackScript;
	public GameObject playRack;
	private RackScript playRackScript;

	void Awake(){
		handRackScript = handRack.GetComponent<RackScript> ();
	}

	void Start(){

		//Create tiles and add them to Rack
		for (int i = 0; i < handRackScript.slots.Length; i++) {
			GameObject newTile = (GameObject)Instantiate(tilePrefab, new Vector3(0, 0, 0), transform.rotation);
			TileScript tileScript = newTile.GetComponent<TileScript>();
			tileScript.SetLetter("j");
			newTile.name = "tile " + i;
			handRackScript.AddTileToFirstEmptySlot(newTile);
		}
	}

	void Update(){
		if (Input.GetKey("space")) {
			handRackScript.shuffleRack();
			Debug.Log("shuffle");
		}
	}

}

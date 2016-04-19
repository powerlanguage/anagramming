using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public GameObject rack;
	public GameObject tilePrefab;
	private RackScript rs;

	void Awake(){
		rs = rack.GetComponent<RackScript> ();
	}

	void Start(){

		//Create tiles and add them to Rack
		for (int i = 0; i < rs.slots.Length; i++) {
			GameObject newTile = (GameObject)Instantiate(tilePrefab, new Vector3(0, 0, 0), transform.rotation);
			TileScript tileScript = newTile.GetComponent<TileScript>();
			tileScript.SetLetter("j");
			newTile.name = "tile " + i;
			rs.AddTileToFirstEmptySlot(newTile);
		}
	}

	void Update(){
		if (Input.GetKey("space")) {
			rs.shuffleRack();
			Debug.Log("shuffle");
		}
	}

}

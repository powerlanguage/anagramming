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

		string sampleWord = "wisguct";

		//Create tiles and add them to Rack
		foreach(char c in sampleWord) {
			GameObject newTile = (GameObject)Instantiate(tilePrefab, new Vector3(0, 0, 0), transform.rotation);
			TileScript tileScript = newTile.GetComponent<TileScript>();
			tileScript.SetLetter(c.ToString());
			newTile.name = c.ToString ();
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

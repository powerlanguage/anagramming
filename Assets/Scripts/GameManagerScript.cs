using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public GameObject tilePrefab;
	public GameObject handRack;
	private RackScript handRackScript;
	public GameObject playRack;
	private RackScript playRackScript;
	public GameObject progessDisplay;
	//Managers
	private WordManagerScript wordManager;
	private PersistentDataManagerScript dataManager;
	private Hashtable data;
	public string currentWord;
	//Game Over Modal
	public GameObject gameCompleteModal;
	//Debug
	public bool debug;

	void Awake(){
		handRackScript = handRack.GetComponent<RackScript> ();
		playRackScript = playRack.GetComponent<RackScript> ();
		//
		wordManager = this.GetComponent<WordManagerScript> ();
		dataManager = this.GetComponent<PersistentDataManagerScript> ();

		//Attempt to load stored data
		data = dataManager.Load();

		// No data, first time the app has run or local storage cleared
		if (data == null || debug) {
			wordManager.Setup ();
		} else {
			// Data found. Set up app using stored data.
			wordManager.Setup(data);
		}

		//Store current word in local variable for easy access
		currentWord = wordManager.GetCurrentWord ();
		SetupGame ();
	}

	void SetupGame(){
		//Clear racks if this isn't a new game
		playRackScript.ClearRack ();
		handRackScript.ClearRack ();

		string shuffledWord = wordManager.ShuffleWord (currentWord);

		//Create tiles and add them to Rack
		foreach(char c in shuffledWord) {
			GameObject newTile = (GameObject)Instantiate(tilePrefab, new Vector3(0, 0, tilePrefab.transform.position.z), transform.rotation);
			TileScript tileScript = newTile.GetComponent<TileScript>();
			tileScript.SetLetter(c.ToString());
			newTile.name = c.ToString ();
			//TODO: Maybe directly add these tiles so it doesn't look visually weird
			handRackScript.AddTileToFirstEmptySlot(newTile);
		}

		//Display Progress
		progessDisplay.GetComponentInChildren<TextMesh>().text = GetNumSolvedWords() + "/" + (GetNumSolvedWords() + GetNumUnsolvedWords());
	}

	void Update(){
		if (Input.GetKey("c")) {
			dataManager.Clear ();
		}

		//Could do this every time we add a tile?

		if (playRackScript.GetRackString () == currentWord) {
			//Game is won
			//Get new word. Should maybe just leave it all in the word manager?
			wordManager.MarkWordAsSolved(currentWord);


			if (GetNumUnsolvedWords() == 0) {
				//If there are no more unsolved words, game is complete
				gameCompleteModal.SetActive(true);
			} else {
				//Fetch a new word, save progress and setup the racks
				wordManager.SetCurrentWord (wordManager.GetUnsolvedWord ());
				currentWord = wordManager.GetCurrentWord ();
				SaveProgress ();
				//Setup the new game
				SetupGame ();
			}
		}
	}

	//Gets a new word without marking the current one as complete
	public void SkipWord(){
		wordManager.SetCurrentWord (wordManager.GetUnsolvedWord ());
		currentWord = wordManager.GetCurrentWord ();
		SaveProgress ();
		SetupGame ();

	}

	private void SaveProgress(){
		//Save Progress
		Hashtable progress = new Hashtable();
		progress.Add ("solvedWords", wordManager.solvedWords);
		progress.Add ("unsolvedWords", wordManager.unsolvedWords);
		progress.Add ("currentWord", currentWord);
		dataManager.Save (progress);

	}

	public int GetNumSolvedWords(){
		return wordManager.GetNumSolvedWords ();
	}

	public int GetNumUnsolvedWords(){
		return wordManager.GetNumUnsolvedWords ();
	}

}

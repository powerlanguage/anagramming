using UnityEngine;
using System.Collections;

public class WordManagerScript : MonoBehaviour {

	public TextAsset rawTextFile;

	public Hashtable unsolvedWords;
	public Hashtable solvedWords;
	public string currentWord;
	public string shuffledWord;

	// Run when we have no app data in storage
	public void Setup(){
		LoadWordsFromTextFile ();
		SetCurrentWord (GetUnsolvedWord ());
		solvedWords = new Hashtable ();
	}

	// Run when we have app data in storage
	public void Setup(Hashtable data){
		solvedWords = (Hashtable) data["solvedWords"];
		unsolvedWords = (Hashtable) data["unsolvedWords"];
		currentWord = (string) data["currentWord"];
	}

	public int GetNumSolvedWords(){
		//not sure why this happens on android, should always be set
		if (solvedWords != null) {
			return solvedWords.Count;
		}
		return 0;
	}

	public int GetNumUnsolvedWords(){
		if (unsolvedWords != null) {
			return unsolvedWords.Count;
		}
		return 0;
	}

	//Really ugly method for getting a random value from a hashtable
	public string GetUnsolvedWord(){

		ICollection keys = unsolvedWords.Keys;

		string [] keyArray = new string[keys.Count];
		keys.CopyTo(keyArray, 0);
		string randomKey = keyArray[UnityEngine.Random.Range(0, keys.Count)];

		return (string)unsolvedWords[randomKey];
	}

	//Move a word from one hashtable to the other
	public void MarkWordAsSolved(string word){
		unsolvedWords.Remove(word);
		solvedWords.Add (word, word);
	}

	public string GetCurrentWord(){
		return currentWord;
	}

	public void SetCurrentWord(string newCurrentWord){
		currentWord = newCurrentWord;
	}

	public string ShuffleWord(string wordToShuffle){

		string shuffledWord = wordToShuffle;

		//Loop until the word being returned doesn't match the one provided
		while (shuffledWord == wordToShuffle) {
			char[] chars = wordToShuffle.ToCharArray ();

			for (int i = 0; i < chars.Length; i++) {
				char temp = chars [i];
				int randomIndex = Random.Range (i, chars.Length);
				chars [i] = chars [randomIndex];
				chars [randomIndex] = temp;
			}

			shuffledWord = new string (chars);
		}

		return shuffledWord;
	}

	//Load from a line delinated attached text file asset
	//Stores in Unsolved words
	public void LoadWordsFromTextFile(){
		//Create new hashtable
		unsolvedWords = new Hashtable ();
		//should handle duplicates

		string rawWords = rawTextFile.text;
		string[] rawLines = rawWords.Split ('\n');
		foreach (string line in rawLines) {
			unsolvedWords.Add(line.ToUpper(), line.ToUpper());
		}
	}
}

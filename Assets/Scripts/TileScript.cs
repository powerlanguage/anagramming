using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

	public string letter;

	public void SetLetter(string s){
		letter = s;
	}

	public string GetLetter(){
		return letter;
	}

}

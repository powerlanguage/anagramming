using UnityEngine;
using System.Collections;

public class MusicControlScript : MonoBehaviour {

	void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}

}

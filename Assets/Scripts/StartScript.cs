using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class StartScript : MonoBehaviour {

	void OnMouseUp(){
		SceneManager.LoadScene ("TestScene");
	}
}

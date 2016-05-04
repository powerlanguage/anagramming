using UnityEngine;
using System.Collections;

public class SFXControllerScript : MonoBehaviour {

	public AudioClip[] clips;
	private AudioSource source;


	void Awake () {
		source = this.GetComponent<AudioSource> ();

	}


	public void playClip(int index){
		if (!source.isPlaying || (source.isPlaying && source.clip == clips[index])) {
			Debug.Log ("Playing clip at: " + index);
			source.clip = clips [index];
			source.Play ();
		} 
	}
}

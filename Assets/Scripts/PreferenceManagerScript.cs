using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreferenceManagerScript : MonoBehaviour {

	public AudioSource music;
	public AudioSource sfx;

	public Slider musicSlider;
	public Slider sfxSlider;

	public float musicVolume;
	public float sfxVolume;

	private GameManagerScript gameManager;

	void Awake(){
		gameManager = this.gameObject.GetComponent<GameManagerScript> ();
	}

	public void Setup(Hashtable data){

		//Check if the audio values have been set before, if so extract and set
		//Doing it this way because audio prefs were added to data after the word data
		//pretty crappy
		//Set the controlling slider value to match

		if (data.ContainsKey("musicVolume")){
			//Setting the slider value calls the Setters below
			//Using the 'onsliderchanged' function in the UI
			musicSlider.value = (float)data ["musicVolume"];
		}

		if (data.ContainsKey("sfxVolume")){
			//Setting the slider value calls the Setters below
			//Using the 'onsliderchanged' function in the UI
			sfxSlider.value = (float)data ["sfxVolume"];
		}
	}

	public void SetMusicVolume(float vol){
		musicVolume = vol;
		music.volume = musicVolume;

		//As this is done at start up, we need to check if the gamemanager has been awoken
		if (gameManager != null) {
			gameManager.SaveProgress ();
		}
	}

	//As this is done at start up, we need to check if the gamemanager has been awoken
	public void SetSFXVolume(float vol){
		sfxVolume = vol;
		sfx.volume = sfxVolume;
		if (gameManager != null) {
			gameManager.SaveProgress ();
		}
	}
}

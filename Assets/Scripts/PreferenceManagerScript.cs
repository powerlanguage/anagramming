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
		gameManager = this.GetComponent<GameManagerScript> ();
	}

	public void Setup(Hashtable data){

		//Check if the audio values have been set before, if so extract and set
		//Doing it this way because audio prefs were added to data after the word data
		//pretty crappy
		//Set the controlling slider value to match

		if (data.ContainsKey("musicVolume")){
			float vol = (float)data ["musicVolume"];
			SetMusicVolume(vol);
			musicSlider.value = vol;
		}

		if (data.ContainsKey("sfxVolume")){
			float vol = (float)data ["sfxVolume"];
			SetSFXVolume(vol);
			sfxSlider.value = vol;
		}

		music.volume = musicVolume;
		sfx.volume = sfxVolume;
	}

	public void SetMusicVolume(float vol){
		musicVolume = vol;
		music.volume = musicVolume;

		gameManager.SaveProgress ();
	}

	public void SetSFXVolume(float vol){
		sfxVolume = vol;
		sfx.volume = sfxVolume;

		gameManager.SaveProgress ();
	}
}

using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class PersistentDataManagerScript : MonoBehaviour {
	private Hashtable persistentData;

	//https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/persistence-data-saving-loading

	//Saves to local storage
	public void Save(Hashtable newPersistentData){

		Debug.Log ("Saving...");
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/persistent-data.dat");

		PersistentData data = new PersistentData ();
		data.persistentData = newPersistentData;

		bf.Serialize (file, data);
		file.Close ();
		Debug.Log ("Saved.");
	}

	//Loads and returns local storage
	public Hashtable Load(){
		if (File.Exists (Application.persistentDataPath + "/persistent-data.dat")) {
			Debug.Log ("Loading...");
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/persistent-data.dat", FileMode.Open);
			PersistentData dataFile = (PersistentData)bf.Deserialize (file);

			persistentData = dataFile.persistentData;
			file.Close ();
			Debug.Log ("Loaded");
			return(persistentData);
		} else {
			Debug.Log("Persistent Data file does not exist");
			return null;
		}
	}

	//Deletes local storage
	public void Clear(){
		if (File.Exists (Application.persistentDataPath + "/persistent-data.dat")) {
			Debug.Log("Clearing...");
			File.Delete (Application.persistentDataPath + "/persistent-data.dat");
			Debug.Log("Cleared");
		} else {
			Debug.Log("Persistent Data file does not exist");
		}
	}
}

[Serializable]
class PersistentData{
	public Hashtable persistentData;
}
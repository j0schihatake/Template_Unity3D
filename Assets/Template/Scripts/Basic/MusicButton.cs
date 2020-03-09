using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicButton : MonoBehaviour {
	public string Enabled;
	public string Disabled;
	// Use this for initialization
	void Start () 
	{
		UserSettings.ReadPrefs();
		
		if(UserSettings.MusicEnabled){
			GetComponent<Text>().text = Enabled;
		}else{
			GetComponent<Text>().text = Disabled;
		}
		
	}
	
	public void SwichButtom()
	{

		if(UserSettings.MusicEnabled){
			UserSettings.SetMusicEnabled(false);
			GetComponent<Text>().text = Disabled;
		}else{
			UserSettings.SetMusicEnabled(true);
			GetComponent<Text>().text = Enabled;
		}
		
	}
}

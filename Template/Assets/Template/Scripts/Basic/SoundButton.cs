using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundButton : MonoBehaviour {
	public string Enabled;
	public string Disabled;
	
	void Start () {
		UserSettings.ReadPrefs();
		
		if(UserSettings.SoundEnabled){
			GetComponent<Text>().text = Enabled;
		}else{
			GetComponent<Text>().text = Disabled;
		}
	}
	

	public void SwichButtom()
	{
		if(UserSettings.SoundEnabled){
			UserSettings.SetSoundEnabled(false);
			GetComponent<Text>().text = Disabled;
		}else{
			UserSettings.SetSoundEnabled(true);
			GetComponent<Text>().text = Enabled;
		}
	}
}

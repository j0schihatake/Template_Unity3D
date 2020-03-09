using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicBar : MonoBehaviour {
	Slider bar;
	public Button button;
	public Sprite Enabled;
	public Sprite Disabled;
	
	float prevValue = 0.5f;
	
	void Start () {

		bar = GetComponent<Slider>();
		UserSettings.MusicChanged += HandleMusicChanged;
		if(UserSettings.MusicVolume == 0)
			button.GetComponent<Image>().sprite = Disabled;
		else
			button.GetComponent<Image>().sprite = Enabled;

		bar.value = UserSettings.MusicVolume;
	}
	
	void HandleMusicChanged ()
	{		
		if(UserSettings.MusicEnabled)
		{
			if(bar.value == 0)
			{
				UserSettings.SetMusicVolume(prevValue);
				bar.value = UserSettings.MusicVolume;
			}
		}
		else
		{
			if(bar.value > 0)
			{
				prevValue = bar.value;
				UserSettings.SetMusicVolume(0);
				bar.value = UserSettings.MusicVolume;
			}
		}
	}
	
	public void OnValueChange () {
		UserSettings.SetMusicVolume(bar.value);
		if(UserSettings.MusicEnabled)
		{
			if(bar.value == 0){
				
				UserSettings.SetMusicEnabled(false);
				button.GetComponent<Image>().sprite = Disabled;
			}
		}
		else
		{
			if(bar.value > 0){
				
				UserSettings.SetMusicEnabled(true);
				button.GetComponent<Image>().sprite = Enabled;
			}
		}
	}
	
	public void SwichButtom()
	{
		if(UserSettings.MusicEnabled){
			UserSettings.SetMusicEnabled(false);
			button.GetComponent<Image>().sprite = Disabled;
		}else{
			UserSettings.SetMusicEnabled(true);
			button.GetComponent<Image>().sprite = Enabled;
		}
	}
}

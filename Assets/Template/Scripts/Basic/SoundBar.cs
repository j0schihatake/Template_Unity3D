using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundBar : MonoBehaviour {
	Slider bar;
	public Button button;
	public Sprite Enabled;
	public Sprite Disabled;

	float prevValue = 0.5f;

	void Start () {

		bar = GetComponent<Slider>();
		UserSettings.SoundChanged += HandleSoundChanged;
		if(UserSettings.SoundVolume == 0)
			button.GetComponent<Image>().sprite = Disabled;
		else
			button.GetComponent<Image>().sprite = Enabled;

		bar.value = UserSettings.SoundVolume;

	}
	void Update ()
	{

	}
	void HandleSoundChanged ()
	{		
		if(UserSettings.SoundEnabled)
		{
			if(bar.value == 0)
			{
				UserSettings.SetSoundVolume(prevValue);
				bar.value = UserSettings.SoundVolume;
			}
		}
		else
		{
			if(bar.value > 0)
			{
				prevValue = bar.value;
				UserSettings.SetSoundVolume(0);
				bar.value = UserSettings.SoundVolume;
			}
		}
	}
	
	public void OnValueChange () {
		UserSettings.SetSoundVolume(bar.value);
		if(UserSettings.SoundEnabled)
		{
			if(bar.value == 0){

			    UserSettings.SetSoundEnabled(false);
				button.GetComponent<Image>().sprite = Disabled;
			}
		}
		else
		{
			if(bar.value > 0){

			    UserSettings.SetSoundEnabled(true);
				button.GetComponent<Image>().sprite = Enabled;
			}
		}
	}

	public void SwichButtom()
	{
		if(UserSettings.SoundEnabled){
			UserSettings.SetSoundEnabled(false);
			button.GetComponent<Image>().sprite = Disabled;
		}else{
			UserSettings.SetSoundEnabled(true);
			button.GetComponent<Image>().sprite = Enabled;
		}
	}
}

using UnityEngine;
using System.Collections;

public static class UserSettings {
	public delegate void ChangeSettingsEvent();
	
	public static event ChangeSettingsEvent SoundChanged = delegate {};
	public static event ChangeSettingsEvent MusicChanged = delegate {};
	
	public static event ChangeSettingsEvent SoundVolumeChanged = delegate {};
	public static event ChangeSettingsEvent MusicVolumeChanged = delegate {};
	
	static bool soundEnabled;
	public static bool SoundEnabled
	{
		get { return soundEnabled; }
	}
	
	static bool musicEnabled;
	public static bool MusicEnabled
	{
		get { return musicEnabled; }
	}
	
	static float soundVolume;
	public static float SoundVolume 
	{
		get { return soundVolume;}
	}
	
	static float musicVolume;
	public static float MusicVolume 
	{
		get { return musicVolume;}
	}

	public static void ReadPrefs()
	{
		SecurePlayerPrefs.Init();
		int sound = SecurePlayerPrefs.GetInt("sound", 1);
		int music = SecurePlayerPrefs.GetInt("music", 1);
		
		soundEnabled = IntToBool(sound);
		musicEnabled = IntToBool(music);
		soundVolume = SecurePlayerPrefs.GetFloat("sound_vol", 0.5f);
		musicVolume = SecurePlayerPrefs.GetFloat("music_vol", 0.5f);
	}
	
	public static void SetSoundEnabled(bool value)
	{
		soundEnabled = value;
		SoundChanged();
		SecurePlayerPrefs.SetInt("sound", BoolToInt(value));
	}
	
	public static void SetMusicEnabled(bool value)
	{
		musicEnabled = value;
		MusicChanged();
		SecurePlayerPrefs.SetInt("music", BoolToInt(value));
	}
	
	public static void SetSoundVolume(float value)
	{
		soundVolume = value;
		SecurePlayerPrefs.SetFloat("sound_vol", value);
		SoundVolumeChanged();
	}
	
	public static void SetMusicVolume(float value)
	{
		musicVolume = value;
		SecurePlayerPrefs.SetFloat("music_vol", value);
		MusicVolumeChanged();
	}

	static bool IntToBool(int value)
	{
		return value > 0;
	}
	
	static int BoolToInt(bool value)
	{
		if (value) return 1;
		else return 0;
	}
}

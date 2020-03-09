using UnityEngine;
using System.Collections;

public static class GameSettings {

	public delegate void ChangeGameSettingEvent();
	
	public static event ChangeGameSettingEvent ChangeScores = delegate {};
	
	
	static int score;
	public static float Score 
	{
		get { return score;}
	}
	
	
	public static void ReadPrefs()
	{
		SecurePlayerPrefs.Init();
		score = SecurePlayerPrefs.GetInt("score_vol");
	}
	
	
	public static void SetScoreVolume(int value)
	{
		score = value;
		SecurePlayerPrefs.SetInt("score_vol", value);
		ChangeScores();
	}
	
	public static void AddScoreVolume(int value)
	{
		score += value+SecurePlayerPrefs.GetInt("score_vol");
		SecurePlayerPrefs.SetInt("score_vol", score);
		ChangeScores();
	}
}

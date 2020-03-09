using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class test : MonoBehaviour {
	public static test Instance;
	public AudioClip[] muiscList;
//	public List<AudioClip> muiscList = new List<AudioClip>();


	public enum MusicMode
	{
		Menu,
		Game
	}
	public MusicMode mode;

	ObjectPool pool;

	void Awake()
	{
		if (Instance == null)
			Init();
		else 
			Destroy(gameObject);
	}

	void Init()
	{
		Instance = this;
		DontDestroyOnLoad (this);
		muiscList = Resources.LoadAll<AudioClip>("music");
		mode = MusicMode.Menu;
//		HandleNewSceneEvent ();
		if (Application.loadedLevel == 1)
		{
			mode = MusicMode.Game;
		}
		//Instantiate(Resources.Load<GameObject>("Panel")as GameObject);
	}
	void Start()
	{
		pool = GetComponent<ObjectPool>();
//		for(int i = 0; i < muiscList.Count; i++){
//    		pool.startupPools[i].
//		}
	}
	void HandleNewSceneEvent ()
	{
		if(CMenu.Instance != null)
			mode = MusicMode.Menu;
		else
			mode = MusicMode.Game;

	}
	void Update () {
		//Debug.Log(UserSettings.LevelName);
	}
}

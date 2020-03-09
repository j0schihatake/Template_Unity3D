using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CGame : MonoBehaviour  {
	public static CGame Instance;
	
	public enum LoadState
	{
		none,
		load
	}
	[Header("Awake")]
	public LoadState Load;

	public SettingButtonPanels showSettings;
	[System.Serializable]
	public class SettingButtonPanels
	{
		public bool ShowNormal;
		public bool ShowPause;
//		public bool ShowPreviousPanel;
	}


	[Header("Parameters")]
	public float winTimeScale = 0.5f;
	public float lossTimeScale = 0.5f;
	
//	[Header("Pause Events")]
//	public bool EventAppPause = true;
//	public bool UnityAppPause = true;
//	public bool EditorPause = true;
//	public bool BannerClickPause = true;
//	public bool FullscreenShowPause = true;
//	public bool AutoResume = false;


	[Header("Components")]
	public GameObject pauseButton;
	public GameObject menuButton;
	public GameObject restartButton;
	public GameObject backButton;
	public GameObject settingButtom;
	public GameObject nextLevelButton;
	public GameObject loadingLabel;
	public GameObject winLabel;
	public GameObject lossLabel;
	public GameObject settingPanel;


	public enum CGameState
	{
		Normal,
		Pause,
		Settings,
		Win,
		Loss,
		Loading
	}
	
	CGameState st;
	CGameState state
	{
		get { return st; }
		set
		{
			st = value;
			switch (st)
			{
			case CGameState.Normal:
				Time.timeScale = 1;
				ShowNormal();
				break;
			
			case CGameState.Pause:
				Time.timeScale = 0;
				ShowPause();
				break;

			case CGameState.Settings:
				Time.timeScale = 0;
				ShowSettings();
				break;

			case CGameState.Win:
				Time.timeScale = winTimeScale;
				ShowWin();
				break;

			case CGameState.Loss:
				Time.timeScale = lossTimeScale;
				ShowLoss();
				break;

			case CGameState.Loading:
				Time.timeScale = 1;
				ShowLoading();
				break;
				
//			case CGameState.AddPanel:
//				ShowAddPanel();
//				break;
			}
		}
	}
	
	public CGameState State
	{
		get { return state; }
	}
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else GameObject.Destroy(this);
		SecurePlayerPrefs.Init();
		UserSettings.ReadPrefs(); 
	}
	void Start () 
	{
		state = CGameState.Normal;
	}

	void Update () 
	{
	
	}

	void ShowNormal()
	{
		DisableAllControls();
		pauseButton.SetActive(true);
		if(showSettings.ShowNormal)
			settingButtom.SetActive(true);
	}
	void ShowPause()
	{
		DisableAllControls();
		backButton.SetActive(true);
		menuButton.SetActive(true);
		restartButton.SetActive(true);
		if(showSettings.ShowPause)
			settingButtom.SetActive(true);
	}
	void ShowSettings()
	{
		DisableAllControls();
		settingPanel.SetActive(true);
		backButton.SetActive(true);
	}
	void ShowWin()
	{
		DisableAllControls();
		winLabel.SetActive(true);
		menuButton.SetActive(true);
		restartButton.SetActive(true);
	}
	void ShowLoss()
	{
		DisableAllControls();
		lossLabel.SetActive(true);
		menuButton.SetActive(true);
		restartButton.SetActive(true);
	}
	void ShowLoading()
	{
		DisableAllControls();
		if(Load == LoadState.load)
			LoadingComponent.Instance.LoadNextLevel("menu");
		if(Load == LoadState.none)
			Application.LoadLevel("menu");
	}
	void DisableAllControls()
	{
		pauseButton.SetActive(false);
		backButton.SetActive(false);
		menuButton.SetActive(false);
		restartButton.SetActive(false);
		lossLabel.SetActive(false);
		winLabel.SetActive(false);
		if(settingButtom.activeSelf == true)
			settingButtom.SetActive(false);
		if(settingPanel.activeSelf == true)
			settingPanel.SetActive(false);
	}
	public void OnRestartButton_Click()
	{
		if(Load == LoadState.load)
			LoadingComponent.Instance.LoadNextLevel("game");
		if(Load == LoadState.none)
			Application.LoadLevel("game");
	} 

	public void OnPauseButton_Click()
	{
		state = CGameState.Pause;
	} 
	public void OnBackButton_Click()
	{
		if(state == CGameState.Pause)
		state = CGameState.Normal;

		if(state == CGameState.Settings && showSettings.ShowNormal)
			state = CGameState.Normal;
		if(state == CGameState.Settings && showSettings.ShowPause)
			state = CGameState.Pause;
	} 
	public void OnSettingButton_Click()
	{
		state = CGameState.Settings;
	} 
	public void OnMenuButton_Click()
	{
		state = CGameState.Loading;
	} 
	public void OnLoss()
	{
		state = CGameState.Loss;
	}
	public void OnWon()
	{
		state = CGameState.Win;
	}
}

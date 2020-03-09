using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SGame : MonoBehaviour  {
	public static SGame Instance;
	
	public enum LoadStates
	{
		none,
		load
	}
	[Header("Awake")]
	public LoadStates Load;
	
	public SettingButtonPanels showSettings;
	[System.Serializable]
	public class SettingButtonPanels
	{
		public bool ShowNormal;
		public bool ShowPause;
	}
	
	
	[Header("Parameters")]
	public float winTimeScale = 0.5f;
	public float lossTimeScale = 0.5f;
	
	[Header("Components")]
	public GameObject menuButton;
	public GameObject restartButton;
	public GameObject playBackButton;
	public GameObject backButton;
	public GameObject settingButtom;
	public GameObject nextLevelButton;
	public GameObject loadingLabel;
	public GameObject winLabel;
	public GameObject lossLabel;
	public GameObject settingPanel;
	public GameObject Chooser;
	public GameObject pausePanel;
	Transform choose;
	
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
		GameSettings.ReadPrefs();
	}
	void Start () 
	{
		state = CGameState.Normal;
		
		menuButton.GetComponent<Button>().onClick.AddListener(delegate{OnMenuButton_Click();});
		settingButtom.GetComponent<Button>().onClick.AddListener(delegate{OnSettingButton_Click();});
		backButton.GetComponent<Button>().onClick.AddListener(delegate{OnBackButton_Click();});
		playBackButton.GetComponent<Button>().onClick.AddListener(delegate{OnBackButton_Click();});
		restartButton.GetComponent<Button>().onClick.AddListener(delegate{OnRestartButton_Click();});
	}
	
	void Update () 
	{
		if(choose)
			if(Chooser.transform.position != choose.transform.position)
			{
				Chooser.transform.position = Vector3.Lerp(Chooser.transform.position,new Vector3(Chooser.transform.position.x,choose.transform.position.y,0),0.25f);
			}
			
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			OnEscapeButton_Click();
		}
		
		  #if UNITY_EDITOR


		if(Input.GetKeyDown(KeyCode.Keypad1))
		{
			OnWon();
		}
		if(Input.GetKeyDown(KeyCode.Keypad2))
		{
			OnLoss();
		}
		
		  #endif
		
	}
	
	public void SetChoose(Transform t)
	{
		choose = t;
	}
	
	void ShowNormal()
	{
		DisableAllControls();
		if(showSettings.ShowNormal)
			settingButtom.SetActive(true);
	}
	void ShowPause()
	{
		DisableAllControls();
		pausePanel.SetActive(true);
		playBackButton.SetActive(true);
		menuButton.SetActive(true);
		restartButton.SetActive(true);
		if(showSettings.ShowPause)
			settingButtom.SetActive(true);
		Chooser.SetActive(true);
		SetChoose(playBackButton.transform);
		
	}
	void ShowSettings()
	{
		DisableAllControls();
		settingPanel.SetActive(true);
		backButton.SetActive(true);
		Chooser.SetActive(true);
		pausePanel.SetActive(true);
		SetChoose(backButton.transform);
		
	}
	void ShowWin()
	{
		DisableAllControls();
		winLabel.SetActive(true);
		menuButton.SetActive(true);
		restartButton.SetActive(true);
		Chooser.SetActive(true);
		pausePanel.SetActive(true);
		SetChoose(restartButton.transform);
		
	}
	void ShowLoss()
	{
		DisableAllControls();
		lossLabel.SetActive(true);
		menuButton.SetActive(true);
		restartButton.SetActive(true);
		Chooser.SetActive(true);
		pausePanel.SetActive(true);
		SetChoose(restartButton.transform);
	}
	void ShowLoading()
	{
		DisableAllControls();
		if(Load == LoadStates.load)
			LoadingComponent.Instance.LoadNextLevel("menu");
		if(Load == LoadStates.none)
			Application.LoadLevel("menu");
	}
	void DisableAllControls()
	{
		pausePanel.SetActive(false);
		backButton.SetActive(false);
		playBackButton.SetActive(false);
		menuButton.SetActive(false);
		restartButton.SetActive(false);
		lossLabel.SetActive(false);
		winLabel.SetActive(false);
		Chooser.SetActive(false);
		if(settingButtom.activeSelf == true)
			settingButtom.SetActive(false);
		if(settingPanel.activeSelf == true)
			settingPanel.SetActive(false);
	}
	public void OnRestartButton_Click()
	{
		if(Load == LoadStates.load)
			LoadingComponent.Instance.LoadNextLevel("game");
		if(Load == LoadStates.none)
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
	
	public void OnEscapeButton_Click()
	{
		if(state == CGameState.Pause)
			state = CGameState.Normal;
		else
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


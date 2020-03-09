using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SMenu : MonoBehaviour {
	public static SMenu Instance;
	[Header("Awake")]
	public bool ShowPlayButton = true;
	public bool ShowSettingsButton = true;
	public bool ShowStatisticsButton = true;
	public bool ShowExitButton = false;
	public enum LoadStates
	{
		none,
		loading
	}
	public LoadStates Load;
	
	[Header("Components")]
	public GameObject playButton;
	public GameObject exitButton;
	public GameObject backButton;
	public GameObject settingsButton;
	public GameObject statisticsButton;
	public GameObject SettingsPanel;
	public GameObject StatisticsPanel;
	public GameObject Chooser;
	Transform choose;
	
	[System.Serializable]
	public class SettingButtonPanels
	{
		public bool ShowNormal;
		public bool ShowSetting;
		public bool ShowPreviousPanel;
		public int PreviousPanel;
	}
	
	[System.Serializable]
	public class Panels
	{
		[Header("Show standard buttons")]
		public bool Show_playBatton;
		public bool Show_backBatton;
		public bool Show_settingBatton;
		[Header("Insert your panel")]
		public GameObject Panel;
		[Header("Insert your buttom")]
		public GameObject ButtonAdditionalPanel;
		[Header("which will show a button?")]
		public SettingButtonPanels ButtonPanels;
	}
	
	[Header("Additional Panel")]
	public List<Panels> Add_Panels = new List<Panels>();
	
	
	int indexPanel;
	
	public enum CMenuState
	{
		Normal,
		Settings,
		Loading,
		Statistics,
		AddPanel,
		Exit
	}
	
	CMenuState st;
	CMenuState state
	{
		get { return st; }
		set
		{
			Time.timeScale = 1;
			st = value;
			switch (st)
			{
			case CMenuState.Normal:
				ShowNormal();
				break;
				
			case CMenuState.Settings:
				ShowSettings();
				break;
				
			case CMenuState.Loading:
				ShowLoading();
				break;
				
			case CMenuState.Statistics:
				ShowStatistics();
				break;
				
			case CMenuState.AddPanel:
				ShowAddPanel();
				break;
			}
		}
	}
	
	public CMenuState State
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
		state = CMenuState.Normal;
		
		if(Add_Panels.Count != 0)
			for(int i = 0; i < Add_Panels.Count; i++)
			{
				int a = i;
				Add_Panels[i].ButtonAdditionalPanel.GetComponent<Button>().onClick.AddListener(delegate {
					OnAdditional_PanelButton_Click(a);});
			}
		playButton.GetComponent<Button>().onClick.AddListener(delegate{OnPlayButton_Click();});
		settingsButton.GetComponent<Button>().onClick.AddListener(delegate{OnSettingsButton_Click();});
		backButton.GetComponent<Button>().onClick.AddListener(delegate{OnBackButton_Click();});
		exitButton.GetComponent<Button>().onClick.AddListener(delegate{OnExitButton_Click();});
		statisticsButton.GetComponent<Button>().onClick.AddListener(delegate{OnStatisticsButton_Click();});
		
		
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
			OnBackButton_Click();
		}
	}
	
	public void SetChoose(Transform t)
	{
		choose = t;
	}
	
	void ShowNormal()
	{
		DisableAllControls();
		if(ShowPlayButton)
			playButton.SetActive(true);
		else
			playButton.SetActive(false);
		if(ShowSettingsButton)
			settingsButton.SetActive(true);
		else
			settingsButton.SetActive(false);
		if(ShowStatisticsButton)
			statisticsButton.SetActive(true);
		else
			statisticsButton.SetActive(false);
		if(ShowExitButton)
			exitButton.SetActive(true);
		else
			exitButton.SetActive(false);
		
		if(Add_Panels.Count != 0)
			for(int i = 0; i < Add_Panels.Count; i ++)
			{
				if(Add_Panels[i].ButtonPanels.ShowNormal)
					Add_Panels[i].ButtonAdditionalPanel.SetActive(true);
			}
		
		SetChoose(playButton.transform);
		
		
	}
	void ShowSettings()
	{
		DisableAllControls();
		SettingsPanel.SetActive(true);
		backButton.SetActive(true);
		
		if(Add_Panels.Count != 0)
			for(int i = 0; i < Add_Panels.Count; i ++)
			{
				if(Add_Panels[i].ButtonPanels.ShowSetting)
					Add_Panels[i].ButtonAdditionalPanel.SetActive(true);
			}
		
		SetChoose(backButton.transform);
		
	}
	
	void ShowStatistics()
	{
		DisableAllControls();
		StatisticsPanel.SetActive(true);
		backButton.SetActive(true);
		
		if(Add_Panels.Count != 0)
			for(int i = 0; i < Add_Panels.Count; i ++)
			{
				if(Add_Panels[i].ButtonPanels.ShowSetting)
					Add_Panels[i].ButtonAdditionalPanel.SetActive(true);
			}
		
		SetChoose(backButton.transform);
		
	}
	
	void ShowLoading()
	{
		DisableAllControls();
		if(Load == LoadStates.loading)
			LoadingComponent.Instance.LoadNextLevel("game");
		if(Load == LoadStates.none)
			Application.LoadLevel("game");
	}
	void ShowAddPanel()
	{
		DisableAllControls();
		Add_Panels[indexPanel].Panel.SetActive(true);
		if(Add_Panels[indexPanel].Show_backBatton)
			backButton.SetActive(true);
		if(Add_Panels[indexPanel].Show_playBatton)
			playButton.SetActive(true);
		if(Add_Panels[indexPanel].Show_settingBatton)
			settingsButton.SetActive(true);
		
		
		for(int i = 0; i < Add_Panels.Count; i ++)
		{
			if(Add_Panels[i].ButtonPanels.ShowPreviousPanel){
				if(Add_Panels[i].ButtonPanels.PreviousPanel == indexPanel)
					Add_Panels[i].ButtonAdditionalPanel.SetActive(true);
			}
		}
	}
	void DisableAllControls()
	{
		if(ShowExitButton || exitButton.activeSelf == true)
			exitButton.SetActive(false);
		playButton.SetActive(false);
		settingsButton.SetActive(false);
		SettingsPanel.SetActive(false);
		backButton.SetActive(false);
		statisticsButton.SetActive(false);
		StatisticsPanel.SetActive(false);
		
		if(Add_Panels.Count != 0){
			for(int i = 0; i<Add_Panels.Count;i++)
			{
				Add_Panels[i].Panel.SetActive(false);
				Add_Panels[i].ButtonAdditionalPanel.SetActive(false);
			}
		}
	}
	
	public void OnPlayButton_Click()
	{
		state = CMenuState.Loading;
	}
	
	public void OnAdditional_PanelButton_Click(int index)
	{
		indexPanel = index;
		state = CMenuState.AddPanel;
	}
	
	public void OnBackButton_Click()
	{
		if(state != CMenuState.Normal)
			state = CMenuState.Normal;
		else
			OnExitButton_Click();
	}
	public void OnSettingsButton_Click()
	{
		state = CMenuState.Settings;
	}
	public void OnStatisticsButton_Click()
	{
		state = CMenuState.Statistics;
	}
	public void OnExitButton_Click()
	{
		Application.Quit();
	}
}

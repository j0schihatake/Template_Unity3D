using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CMenu : MonoBehaviour {
	public static CMenu Instance;
	[Header("Awake")]
	public bool ShowPlayButton = true;
	public bool ShowSettingsButton = true;
	public bool ShowExitButton = false;
	public enum LoadState
	{
		none,
		loading
	}
	public LoadState Load;

	[Header("Components")]
	public GameObject playButton;
	public GameObject exitButton;
	public GameObject backButton;
	public GameObject settingsButton;
	public GameObject SettingsPanel;

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
	}

	void Update () 
	{

	}

	void ShowNormal()
	{
		DisableAllControls();
		if(ShowPlayButton)
		playButton.SetActive(true);
		if(ShowSettingsButton)
		settingsButton.SetActive(true);
		if(ShowExitButton)
			exitButton.SetActive(true);

		if(Add_Panels.Count != 0)
		for(int i = 0; i < Add_Panels.Count; i ++)
		{
			if(Add_Panels[i].ButtonPanels.ShowNormal)
				Add_Panels[i].ButtonAdditionalPanel.SetActive(true);
		}


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
	}
	void ShowLoading()
	{
		DisableAllControls();
		if(Load == LoadState.loading)
			LoadingComponent.Instance.LoadNextLevel("game");
		if(Load == LoadState.none)
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
//		if(state == CMenuState.Settings){
		    state = CMenuState.Normal;
//		}
//		if(state == CMenuState.AddPanel)
//		{
//			for(int i = 0; i < Add_Panels.Count; i ++)
//			{
//				if(Add_Panels[i].ButtonPanels.ShowPreviousPanel){
//					if(Add_Panels[i].ButtonPanels.PreviousPanel == indexPanel)
//						Add_Panels[i].ButtonAdditionalPanel.SetActive(true);
//				}
//			}
//		}
//		if(indexPanel != 0){
//			indexPanel--;
//			if(indexPanel == 0)
//				state = CMenuState.Normal;
//			else
//			    state = CMenuState.AddPanel;
//		}
	}
	public void OnSettingsButton_Click()
	{
		state = CMenuState.Settings;
	}
	public void OnExitButton_Click()
	{
		Application.Quit();
	}
}

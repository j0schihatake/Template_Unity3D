using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class LoadingComponent : MonoBehaviour
{	
	public static LoadingComponent Instance;
    private AsyncOperation asy;
	public Text TextProgres;
	public Image LoadingBar;
	public GameObject LoadObject;
    public float startDelay = 1.0f;
	float i;
	float d;

	void Awake()
	{
		Instance = this;
	}
	

    void Update()
    {
		if(asy != null){
		    d = asy.progress*100;
			i = Mathf.LerpUnclamped(i, d, 0.5f);
			TextProgres.text = "" + i.ToString("00")+ "%";
			LoadingBar.fillAmount = i/100;
			if (!asy.isDone){
				    Dest();
			}
		}
    }

	public void LoadNextLevel(string levelName)
    {
		gameObject.transform.parent = null;
        DontDestroyOnLoad(this);
		LoadObject.SetActive(true);
		StartCoroutine("Assembly");
		asy = Application.LoadLevelAsync(levelName);
    }
	void Dest()
	{
		Destroy (gameObject, startDelay);
	}

	IEnumerator Assembly()
	{
		if(asy != null)
		{
			while(!asy.isDone){
				System.GC.Collect();
			    Resources.UnloadUnusedAssets();
			}
				yield return null;
		}
	}
}
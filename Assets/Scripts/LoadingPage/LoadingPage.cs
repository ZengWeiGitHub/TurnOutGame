using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPage : MonoBehaviour {

	public ProgressBarNoMask progressBar;

	private float curProgressValue;
	// Use this for initialization
	void Start () {
		Init();
	}

	public void Init()
	{
		curProgressValue = 0;
		progressBar.SetProgress(0);
		StartCoroutine("LoadScene");
		PlayerInfoPanel.Instance.Post ();
		CurrentInfo.Instance.Post ();
		LotteryRecord.Instance.Post ();
		Queryusers.Instance.Post ();
		GetSetting.Instance.Post ();

	}

	void SetProgress()
	{
		if (curProgressValue < 50)
			curProgressValue += 3;
		else {
			curProgressValue +=1;
		}
			
		float value = curProgressValue / 100;
		progressBar.SetProgress(value);
	}

	IEnumerator LoadScene()
	{
		//AsyncOperation asy = SceneManager.LoadSceneAsync("GameScene");
		while(curProgressValue < 100)
		{
			SetProgress();
			yield return new WaitForEndOfFrame();
		}
		while(!GameData.Instance.isGetUserInfoSuccess || !GameData.Instance.isGetCurrentInfoSuccess 
		      || !GameData.Instance.isGetHistoryInfoSuccess || !GameData.Instance.isGetTheUsersInfoSuccess)
		{
			yield return null;
		}

		yield return new WaitForSeconds (1f);
		LoadSceneComplete();
	}

	void LoadSceneComplete()
	{
		gameObject.SetActive(false);
		UIManager.Instance.ShowPanel (PanelType.BetPanel);
		GameData.Instance.Init ();
	}
}

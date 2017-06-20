using UnityEngine;
using System.Collections;

public class PersonalCenterPanel : PanelBase {

	public static PersonalCenterPanel Instance;

	public GameObject  userInfoLightObj;
	public GameObject  balanceLightObj;
	public GameObject  pointLightObj;
	public GameObject  settingLightObj;
	public GameObject  feekbackLightObj;

	public GameObject userInfoContentObj;
	public GameObject balanceContentObj;
	public GameObject pointContentObj;
	public GameObject settingContentObj;
	public GameObject feekbackContentObj;

	public BalanceManager balanceManagerScr;
	public UserInfoManager userInfoManagerScr;
	public SettingMagnager settingMagnagerScr;
	public PointManager PointManagerScr;

	private GameObject currentSelectObj;
	private GameObject currentShowCotent;






	public override void InitPanel ()
	{
		Instance = this;

		base.InitPanel ();
		
		userInfoLightObj.SetActive (true);
		currentSelectObj = userInfoLightObj;
		userInfoContentObj.SetActive (true);
		currentShowCotent = userInfoContentObj;


	}

	public override void ShowPanel ()
	{
		InitData ();
		base.ShowPanel ();
	}

	void InitData()
	{
		balanceManagerScr.Init ();
		userInfoManagerScr.Init ();
		settingMagnagerScr.Init ();
		PointManagerScr.Init ();
	}


	public void UserInfoOnClick()
	{
		if (currentSelectObj != userInfoLightObj) {
			AudioManager.Instance.PlaySound (SoundName.ButtonClick);
			currentSelectObj.SetActive(false);
			userInfoLightObj.SetActive(true);
			currentSelectObj = userInfoLightObj;
			currentShowCotent.SetActive(false);
			userInfoContentObj.SetActive(true);
			currentShowCotent = userInfoContentObj;
		}
	}

	public void BalanceOnClick()
	{
		if (currentSelectObj != balanceLightObj) {
			AudioManager.Instance.PlaySound (SoundName.ButtonClick);
			currentSelectObj.SetActive(false);
			balanceLightObj.SetActive(true);
			currentSelectObj = balanceLightObj;
			currentShowCotent.SetActive(false);
			balanceContentObj.SetActive(true);
			currentShowCotent = balanceContentObj;
		}
	}

	public void PointOnClick()
	{
		if (!GameData.Instance.IsAgent)
			return;
		if (currentSelectObj != pointLightObj) {
			AudioManager.Instance.PlaySound (SoundName.ButtonClick);
			currentSelectObj.SetActive(false);
			pointLightObj.SetActive(true);
			currentSelectObj = pointLightObj;
			currentShowCotent.SetActive(false);
			pointContentObj.SetActive(true);
			currentShowCotent = pointContentObj;
		}
	}

	public void SettingOnClick()
	{
		if (currentSelectObj != settingLightObj) {
			AudioManager.Instance.PlaySound (SoundName.ButtonClick);
			currentSelectObj.SetActive(false);
			settingLightObj.SetActive(true);
			currentSelectObj = settingLightObj;
			currentShowCotent.SetActive(false);
			settingContentObj.SetActive(true);
			currentShowCotent = settingContentObj;
		}
	}

	public void FeekBackOnClick()
	{
		if (currentSelectObj != feekbackLightObj) {
			AudioManager.Instance.PlaySound (SoundName.ButtonClick);
			currentSelectObj.SetActive(false);
			feekbackLightObj.SetActive(true);
			currentSelectObj = feekbackLightObj;
			currentShowCotent.SetActive(false);
			feekbackContentObj.SetActive(true);
			currentShowCotent = feekbackContentObj;
		}
	}


	public void CloseOnClick()
	{
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);
		UIManager.Instance.HidePanel (PanelType.PersonalCenterPanel);
	}
}

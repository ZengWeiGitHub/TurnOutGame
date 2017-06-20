using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MiniJSON;

public class BetPanel : PanelBase {

	public static BetPanel Instance = null;

	public GameObject moreFunctionContent;

	public HistoryManager historyManager;

	public GameObject openAwardingObj;

	public EasyFontTextMesh continueCountTex;
	private int continueCount;
	public int ContinueCount
	{
		set
		{
			continueCount = value;
			if(continueCount > 0)
			{
				continueCountTex.gameObject.SetActive(true);
				continueCountTex.text = continueCount.ToString();
			}
			else
			{
				continueCountTex.gameObject.SetActive(false);
			}
		}
		get
		{
			return continueCount;
		}
	}

	public EasyFontTextMesh userTotalMyPointText, platformTotalPointText, platformTotalUsersText, userPointText,termNoText;
	public EasyFontTextMesh countDownTermNoText;

	public EasyFontTextMesh countDown;

	public ChipType currentBetChipType = ChipType.Ten;

	public bool isOpenHistory = false;

	private Area _currentSelectArea;
	public Area CurrentSelectArea
	{
		set
		{
			_currentSelectArea = value;
			//signTip.position = _currentSelectArea.transform.position;
		}
		get
		{
			return _currentSelectArea;
		}
	}
	//已经押注的区域
	public Transform signTip;
	public List<Transform> chipsArray = new List<Transform> ();
	private Transform currentChip;
	private List<Area> selectedAreaList = new List<Area> ();
	public Transform areaTran;
	private int areaCount;

	public void ShowOpenAwarding()
	{
		openAwardingObj.SetActive (true);
		openAwardingObj.transform.localScale = Vector3.zero;
		openAwardingObj.transform.DOScale (Vector3.one,0.4f).SetEase(Ease.OutBack);
	}
	public void HideOpenAwarding()
	{
		openAwardingObj.transform.DOScale (Vector3.zero,0.4f);
		openAwardingObj.SetActive (false);
	}

	public void AddToSelectedAreaList(Area area)
	{
		if(!selectedAreaList.Contains(area))
		{
			selectedAreaList.Add (area);
		}
	}
	public void RemoveFromSelectedAreaList(Area area)
	{
		if(selectedAreaList.Contains(area))
		{
			selectedAreaList.Remove (area);
		}
	}
	public void ClearSelectedAreaList()
	{
		selectedAreaList.Clear ();
	}

	public override void InitPanel ()
	{
		Instance = this;

		base.InitPanel ();

	}

	public void InitData()
	{
		AnalysisRate ();
		areaCount = areaTran.childCount;
		for(int i = 0; i < areaCount; i++)
		{
			Area area = areaTran.GetChild (i).GetComponent<Area>();
			area.Id = i + 1;
		}
		
		currentBetChipType = ChipType.Ten;
		currentChip = chipsArray [0];
		isOpenHistory = false;
		chipsArray [0].transform.localScale = Vector3.one * 1.2f;
		chipsArray [1].transform.localScale = Vector3.one * 0.8f;
		chipsArray [2].transform.localScale = Vector3.one * 0.8f;
		chipsArray [3].transform.localScale = Vector3.one * 0.8f;

		continueCountTex.gameObject.SetActive (false);

		InitLotteryRecord (GameData.Instance.LotteryRecordList);
		GameData.Instance.LottryRecordChangeEvent += InitLotteryRecord;

		ContinueCount = 0;
		curMoreFunctionState = MoreFunctionState.Close;
		moreFunctionContent.SetActive (false);
	}

	public void InitLotteryRecord(List<object> lotteryRecordList)
	{
		historyManager.InitLotteryRecordDic (lotteryRecordList);
		historyManager.InitPanel ();
	}

	public Dictionary<string,float> rateDic = new Dictionary<string, float>();
	void AnalysisRate()
	{
		rateDic.Clear ();
		for(int i = 0; i < GameData.Instance.SettingList.Count; i++)
		{
			Dictionary<string,object> temp = GameData.Instance.SettingList[i] as Dictionary<string,object>;
			rateDic.Add(temp["code"].ToString(),((int)temp["odds"]) * 1.0f / 1000 - 1);
		}
	}

	void CountDown(float time)
	{
		int hour = (int)time / 3600;
		int minute = ((int)time - hour * 3600) / 60;
		int second = (int)time - hour * 3600 - minute * 60;
		string minuteStr = minute.ToString();
		string secondStr = second.ToString();
		minuteStr = minuteStr.PadLeft(2,'0');
		secondStr = secondStr.PadLeft(2,'0');
		countDown.text = minuteStr + ":" + secondStr;
	}

	public override void ShowPanel ()
	{
		base.ShowPanel ();


		platformTotalPointText.text = GameData.Instance.TotalPoint.ToString ();
		platformTotalUsersText.text = GameData.Instance.TotalUsers.ToString ();
		userPointText.text = GameData.Instance.Point.ToString ();
		userTotalMyPointText.text = GameData.Instance.MyPoint.ToString();
		termNoText.text = GameData.Instance.TermNo.Substring (GameData.Instance.TermNo.Length - 3) + "/" + "288";
		countDownTermNoText.text = "距" + GameData.Instance.TermNo + "期";

		GameData.Instance.CountDown += CountDown;
		GameData.Instance.PlatformTotalPointEvent += PlatformTotalPointChange;
		GameData.Instance.PlatformTotalUsersEvent += PlatformTotalUsersChange;
		GameData.Instance.MyPointEvent += UsersTotalMyPointChange;
		GameData.Instance.TermNoChangeEvent += TermNoChange;
		GameData.Instance.PointChangeEvent += UsersPointChange;

	}

	public override void HidePanel ()
	{
		GameData.Instance.CountDown -= CountDown;
		base.HidePanel ();
	}

	public void BetSucsess()
	{
		for(int i = 0; i < selectedAreaList.Count; i++)
		{
			selectedAreaList[i].SureBet();
		}
		selectedAreaList.Clear ();
		ContinueCount = 0;
	}
	public void BetFail()
	{
		for(int i = 0; i < selectedAreaList.Count; i++)
		{
			selectedAreaList[i].StopBet();
		}
		selectedAreaList.Clear ();
		ContinueCount = 0;
	}

	public void Clear()
	{
		for(int i = 0; i < areaCount; i++)
		{
			Area area = areaTran.GetChild (i).GetComponent<Area>();
			area.Clear();
		}
		ContinueCount = 0;
	}

	public void PersonalCenterOnClick()
	{
		if (isOpenHistory)
			return;
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);
		UIManager.Instance.ShowPanel (PanelType.PersonalCenterPanel);
	}

	public void Chip1OnClick()
	{
		if (isOpenHistory)
			return;
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);
		if (currentBetChipType != ChipType.Ten) {
			currentBetChipType = ChipType.Ten;
			currentChip.localScale = Vector3.one * 0.8f;
			currentChip = chipsArray[0];
			chipsArray [0].transform.localScale = Vector3.one * 1.2f;
		}
	}
	public void Chip5OnClick()
	{
		if (isOpenHistory)
			return;
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);
		if (currentBetChipType != ChipType.Fifty) {
			currentBetChipType = ChipType.Fifty;
			currentChip.localScale = Vector3.one * 0.8f;
			currentChip = chipsArray[1];
			chipsArray [1].transform.localScale = Vector3.one * 1.2f;
		}
	}
	public void Chip1O0nClick()
	{
		if (isOpenHistory)
			return;
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);
		if (currentBetChipType != ChipType.OneHundred) {
			currentBetChipType = ChipType.OneHundred;
			currentChip.localScale = Vector3.one * 0.8f;
			currentChip = chipsArray[2];
			chipsArray [2].transform.localScale = Vector3.one * 1.2f;
		}
	}
	public void Chip5O0nClick()
	{
		if (isOpenHistory)
			return;
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);
		if (currentBetChipType != ChipType.FiveHundred) {
			currentBetChipType = ChipType.FiveHundred;
			currentChip.localScale = Vector3.one * 0.8f;
			currentChip = chipsArray[3];
			chipsArray [3].transform.localScale = Vector3.one * 1.2f;
		}
	}

	private bool isCanSureOnClick = true;
	public void SureOnClick()
	{
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);

		if (isOpenHistory)
			return;

		if (GameData.Instance.isStopBet)
			return;
		if (!isCanSureOnClick)
			return;
		CurrentSelectArea = null;
		Post ();
		
	}
	public void CancelOnClick()
	{
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);

		if (isOpenHistory)
			return;

		if (GameData.Instance.isStopBet)
			return;

		for(int i = 0; i < selectedAreaList.Count; i++)
		{
			selectedAreaList[i].CancleBet();
		}
		selectedAreaList.Clear ();
		CurrentSelectArea = null;
			
	}
	public void ContinueBetOnClick()
	{
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);

		if (isOpenHistory)
			return;
		if (GameData.Instance.isStopBet)
			return;
		int count = GameData.Instance.continueBetDic.Count;
		for(int i = 0; i < areaCount; i++)
		{
			Area area = areaTran.GetChild (i).GetComponent<Area>();
			if(GameData.Instance.continueBetDic.ContainsKey(area.Id))
			{
				area.ContinueBet(GameData.Instance.continueBetDic[area.Id]);
			}
		}
	}
	private MoreFunctionState curMoreFunctionState = MoreFunctionState.Close;
	public void MoreFunctionOnClick()
	{
		if(curMoreFunctionState == MoreFunctionState.Close)
		{
			curMoreFunctionState = MoreFunctionState.Open;
			moreFunctionContent.SetActive (true);
		}
		else if(curMoreFunctionState == MoreFunctionState.Open)
		{
			curMoreFunctionState = MoreFunctionState.Close;
			moreFunctionContent.SetActive (false);
		}

	}
	public enum MoreFunctionState
	{
		Open,
		Close
	}

	#region 积分显示
	void PlatformTotalPointChange(int point)
	{
		platformTotalPointText.text = point.ToString ();
	}
	void PlatformTotalUsersChange(int num)
	{
		platformTotalUsersText.text = num.ToString ();
	}
	void UsersPointChange(int point)
	{
		userPointText.text = point.ToString ();
	}
	void UsersTotalMyPointChange(int point)
	{
		userTotalMyPointText.text = point.ToString ();
	}
	void TermNoChange(string str)
	{
		countDownTermNoText.text = "距" + str + "期";
		termNoText.text = str.Substring (str.Length - 3) + "/" + "288";
	}
	#endregion

	public void Post()
	{
		int count = selectedAreaList.Count;
		if (count <= 0)
			return;
		isCanSureOnClick = false;
		string command = "";
		for(int i = 0; i < count; i++)
		{
			if(i == 0)
			{
				command = GameData.Instance.areaNameDic[selectedAreaList[i].Id] + '/' + selectedAreaList[i].chipCount.ToString();
			}
			else
			{
				command += ";" + GameData.Instance.areaNameDic[selectedAreaList[i].Id] + '/' + selectedAreaList[i].chipCount.ToString();
			}
			GameData.Instance.tempContinueCount += selectedAreaList[i].chipCount;

		}
		GameData.Instance.tempContinueCount += ContinueCount;
		string url = "/api/ft/bt";
		Dictionary<string,string> tempDic = new Dictionary<string, string>();
		Dictionary<string,object> registerInfoDic = new Dictionary<string, object>();
		tempDic.Add("termNo",GameData.Instance.TermNo);
		tempDic.Add("command",command);
		registerInfoDic.Add("data",tempDic);
		registerInfoDic.Add ("token", GameData.Instance.Token);
		
		string data = Json.Serialize(registerInfoDic);
		
		RestHttpAPI.HttpAPI.Instance.Post(url,data,CallBack);
		
	}
	
	private void CallBack(string wwwText,string wwwError)
	{
		isCanSureOnClick = true;
		Dictionary<string,object> resultJsonDic = Json.Deserialize(wwwText) as Dictionary<string,object>;
		try
		{
			int flag = (int)resultJsonDic["flag"];
			if(flag == 1)
			{
				Debug.Log("bet sucsess");
				AudioManager.Instance.PlaySound(SoundName.BetSuccess);
				TipsManager.Instance.ShowTips ("下注成功");
				for(int i = 0; i < selectedAreaList.Count; i++)
				{
					if(GameData.Instance.tempContinueBetDic.ContainsKey(selectedAreaList[i].Id))
					{
						GameData.Instance.tempContinueBetDic[selectedAreaList[i].Id] += selectedAreaList[i].chipCount;
					}
					else
					{
						GameData.Instance.tempContinueBetDic.Add(selectedAreaList[i].Id,selectedAreaList[i].chipCount);
					}
					
				}
				BetSucsess();
			}
			else if(flag == 0)
			{
				string error = resultJsonDic["err"].ToString();
				Debug.LogError("bet fail:" + error);
				BetFail();
				TipsManager.Instance.ShowTips(error);
			}
		}
		catch
		{
			
		}
	}
}
public enum ChipType
{
	Ten = 10,
	Fifty = 50,
	OneHundred = 100,
	FiveHundred = 500,
	None
}

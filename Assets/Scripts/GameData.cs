using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData:SingletonMonoBehaviour<GameData>{

	public Dictionary<int,string> areaNameDic = new Dictionary<int, string>(){
		{1,"12"},{2,"41"},{3,"34"},{4,"23"},{5,"1-2"},{6,"1-3"},{7,"1-4"},{8,"4-1"},
		{9,"4-2"},{10,"4-3"},{11,"3-4"},{12,"3-2"},{13,"3-1"},{14,"2-4"},{15,"2-3"},{16,"2-1"},
		{17,"1"},{18,"4"},{19,"3"},{20,"2"},{21,"13"},{22,"24"}};

	[HideInInspector]public bool isGetUserInfoSuccess =  false;
	[HideInInspector]public bool isGetCurrentInfoSuccess =  false;
	[HideInInspector]public bool isGetHistoryInfoSuccess =  false;
	[HideInInspector]public bool isGetTheUsersInfoSuccess =  false;

	public delegate void CountDownHandler(float time);
	public CountDownHandler CountDown;

	public delegate void ListChangeHander(List<object> list);
	public delegate void StringChangeHander(string name);
	public delegate void PointChangeHander(int point);
	public PointChangeHander PointChangeEvent;
	public PointChangeHander BalancePointChangeEvent;
	public PointChangeHander PlatformTotalPointEvent;
	public PointChangeHander PlatformTotalUsersEvent;
	public PointChangeHander MyPointEvent;

	public StringChangeHander UserNameChangeEvent;
	public StringChangeHander NickNameChangeEvent;
	public StringChangeHander PasswordChangeEvent;
	public StringChangeHander TermNoChangeEvent;

	public ListChangeHander LottryRecordChangeEvent;

	public bool isStopBet = false;
	public bool bLoginSuccess = false;
	public GameState currentGameState;


	public void Init()
	{
		tempContinueCount = 0;
		currentGameState = GameState.None;
		BetPanel.Instance.InitPanel ();
		BetPanel.Instance.InitData ();
		RealTimeHttp.Instance.Init();
		StartCountDown ();
	}

	private bool bStartCountDown = false;
	private float spanTime;
	void Update()
	{
		if(bStartCountDown)
		{
			if(LeftDrawTime > 0)
			{
				LeftDrawTime -= Time.deltaTime;
				if(LeftDrawTime <= spanTime && !isStopBet)
				{
					Debug.Log("Stop Bet");
					isStopBet = true;
					AudioManager.Instance.PlaySound (SoundName.BetStop);
					TipsManager.Instance.ShowTips("停止押注");
					BetPanel.Instance.BetFail ();
					currentGameState = GameState.StopBet;
				}
				if(CountDown != null)
					CountDown(LeftDrawTime);
			}
			else
			{
				LottryResult();
			}
		}
	}
	private EventLayer curEventLayer = EventLayer.EveryThing;
	void LottryResult()
	{
		if(tempContinueBetDic.Count > 0)
		{
			continueBetDic.Clear();
			foreach(int key in tempContinueBetDic.Keys)
			{
				continueBetDic.Add(key,tempContinueBetDic[key]);
			}
		}
		tempContinueBetDic.Clear();
		bStartCountDown = false;
		BetPanel.Instance.Clear ();
		//BetPanel.Instance.ShowOpenAwarding();
		//curEventLayer = EventLayerController.Instance.cueEventLayer;
		//EventLayerController.Instance.SetEventLayer (EventLayer.Nothing);
		TipsManager.Instance.ShowTips ("开奖同步中……");
		currentGameState = GameState.Resulting;
	}

	public void StartCountDown()
	{
		//BetPanel.Instance.HideOpenAwarding();
		//EventLayerController.Instance.SetEventLayer (curEventLayer);
		currentGameState = GameState.None;
		spanTime = LeftDrawTime - LeftBuyTime;
		Debug.Log("LeftDrawTime:" + LeftDrawTime);
		Debug.Log("LeftBuyTime:" + LeftBuyTime);
		if (LeftDrawTime <= spanTime) {
			isStopBet = true;
			AudioManager.Instance.PlaySound (SoundName.BetStop);
			BetPanel.Instance.BetFail ();
			if(LeftDrawTime == 0)
			{
				TipsManager.Instance.ShowTips ("开奖同步中……");
				currentGameState = GameState.Resulting;
			}
			else
			{
				TipsManager.Instance.ShowTips ("停止押注");
				currentGameState = GameState.StopBet;
			}
				
		} else {
			isStopBet = false;
			AudioManager.Instance.PlaySound (SoundName.BetStart);
			TipsManager.Instance.ShowTips("开始押注");
			currentGameState = GameState.CountDown;
		}
		bStartCountDown = true;

	}

	public void StopCountDown()
	{
		bStartCountDown = false;
		CancelInvoke ("StartCountDown");
	}

	//登录返回的token
	private string _token;
	public string Token
	{
		get
		{
			return _token;
		}
		set
		{
			_token = value;
		}
	}
	//用户名
	private string _userName = "";
	public string UserName
	{
		get
		{
			return _userName;
		}
		set
		{
			if(_userName != value)
			{
				_userName = value;
				if(UserNameChangeEvent != null)
				{
					UserNameChangeEvent(_userName);
				}
			}

		}
	}
	//密码
	private string _password = "";
	public string PassWord
	{
		get
		{
			return _password;
		}
		set
		{
			if(_password != value)
			{
				_password = value;
				if(PasswordChangeEvent != null)
				{
					PasswordChangeEvent(_password);
				}
			}
		}
	}

	//用户别称
	private string _nickName = "";
	public string NickName
	{
		get
		{
			return _nickName;
		}
		set
		{
			_nickName = value;
			if(NickNameChangeEvent != null)
			{
				NickNameChangeEvent(_nickName);
			}
		}
	}
	//用户识别码
	private string _userCode = "";
	public string UserCode
	{
		get
		{
			return _userCode;
		}
		set
		{
			_userCode = value;
		}
	}
	//账户积分
	private int _point = 0;
	public int Point
	{
		get
		{
			return _point;
		}
		set
		{
			if(_point != value)
			{
				_point = value;
				if(PointChangeEvent != null)
				{
					PointChangeEvent(_point);
				}
			}
		}
	}
	//银行积分
	private int _balance = 0;
	public int Balance
	{
		get
		{
			return _balance;
		}
		set
		{
			if(_balance != value)
			{
				_balance = value;
				if(BalancePointChangeEvent != null)
				{
					BalancePointChangeEvent(_balance);
				}
			}
		}
	}
	//推荐人识别码
	private string _referrerCode = "";
	public string ReferrerCode
	{
		get
		{
			return _referrerCode;
		}
		set
		{
			_referrerCode = value;
		}
	}
	//所属代理识别码
	private string _agentCode = "";
	public string AgentCode
	{
		get
		{
			return _agentCode;
		}
		set
		{
			_agentCode = value;
		}
	}

	#region 当期信息
	//期号
	private string _termNo = "";
	public string TermNo
	{
		get
		{
			return _termNo;
		}
		set
		{
			if(_termNo != value)
			{
				if(!string.IsNullOrEmpty(_termNo))
					StartCountDown();
				_termNo = value;
				if(TermNoChangeEvent != null)
				{
					TermNoChangeEvent(_termNo);
				}
			}
		}
	}
	//开奖时间
	private float _drawTime = 0;
	public float DrawTime
	{
		get
		{
			return _drawTime;
		}
		set
		{
			_drawTime = value;
		}
	}
	//剩余开奖时间
	private float _leftdrawTime = 0;
	public float LeftDrawTime
	{
		get
		{
			return _leftdrawTime;
		}
		set
		{
			_leftdrawTime = value;
		}
	}
	//剩余押注时间
	private float _leftBuyTime = 0;
	public float LeftBuyTime
	{
		get
		{
			return _leftBuyTime;
		}
		set
		{
			_leftBuyTime = value;
		}
	}
	//上期期号
	private string _preTermNo = "";
	public string PreTermNo
	{
		get
		{
			return _preTermNo;
		}
		set
		{
			_preTermNo = value;
		}
	}
	//上期开奖号码
	private string _preNumbers = "";
	public string PreNumbers
	{
		get
		{
			return _preNumbers;
		}
		set
		{
			_preNumbers = value;
		}
	}
	//当前系统时间
	private int _serverTime = 0;
	public int ServerTime
	{
		get
		{
			return _serverTime;
		}
		set
		{
			_serverTime = value;
		}
	}
	//平台总押分
	private int _totalPoint = 0;
	public int TotalPoint
	{
		get
		{
			return _totalPoint;
		}
		set
		{
			if(_totalPoint != value)
			{
				_totalPoint = value;
				if(PlatformTotalPointEvent != null)
				{
					PlatformTotalPointEvent(_totalPoint);
				}
			}

		}
	}
	//平台总用户
	private int _totalUsers = 0;
	public int TotalUsers
	{
		get
		{
			return _totalUsers;
		}
		set
		{
			if(_totalUsers != value)
			{
				_totalUsers = value;
				if(PlatformTotalUsersEvent != null)
				{
					PlatformTotalUsersEvent(_totalUsers);
				}
			}
			
		}
	}
	//平台总用户
	private int _myPoint = 0;
	public int MyPoint
	{
		get
		{
			return _myPoint;
		}
		set
		{
			if(_myPoint != value)
			{
				_myPoint = value;
				if(MyPointEvent != null)
				{
					MyPointEvent(_myPoint);
				}
			}
			
		}
	}
	#endregion
	#region 开奖记录
	private string _lotteryNumbers;
	public string LotteryNumbers
	{
		set
		{
			_lotteryNumbers = value;
			string[] splits = _lotteryNumbers.Split(':');
			if(splits.Length < 2)
			{
				Debug.Log("没获取到本期数据");
				return;
			}
			LotteryArea = splits[1];
			string draw = TermNo + ":" + _lotteryNumbers;
			int count = LotteryRecordList.Count;
			if(!LotteryRecordList.Contains(draw))
			{
				if(count <= 0)
				{
					LotteryRecordList.Add(draw);
				}
				else
				{
					for(int i = count - 1;i >= 0; i--)
					{
						if(i > 0)
							LotteryRecordList[i] = LotteryRecordList[i - 1];
						else 
							LotteryRecordList[i] = draw;
					}
				}
			}

			if(LottryRecordChangeEvent != null)
			{
				LottryRecordChangeEvent(LotteryRecordList);
			}
		}
		get
		{
			return _lotteryNumbers;

		}
	}

	private string lotteryArea;
	private string LotteryArea
	{
		set
		{
			lotteryArea = value;
		}
		get
		{
			return lotteryArea;
		}
	}

	public List<object> LotteryRecordList = new List<object> ();
	#endregion

	#region 续押
	public int tempContinueCount = 0;
	private int _continueCount = 0;
	public int ContinueCount
	{
		set
		{
			_continueCount = value;
		}
		get
		{
			return _continueCount;
		}
	}
	public Dictionary<int,int> tempContinueBetDic = new Dictionary<int, int> ();
	public Dictionary<int,int> continueBetDic = new Dictionary<int, int> ();
	#endregion

	#region 用户
	private bool isAgent;
	public bool IsAgent
	{
		set
		{
			isAgent = value;
		}
		get
		{
			return isAgent;
		}
	}
	public List<object> TheUsers = new List<object> ();
	public Dictionary<string,Dictionary<string,object>> TheUesrDic = new Dictionary<string, Dictionary<string, object>> ();
	#endregion

	#region
	public List<object> SettingList = new List<object> ();
	#endregion


	public void SaveUserName(string name)
	{
		PlayerPrefs.SetString ("UsersName",name);
	}
	public string GetUserName()
	{
		if (PlayerPrefs.HasKey ("UsersName")) {
			return PlayerPrefs.GetString ("UsersName");
		} else
			return "";

	}
	public void ClearUserName()
	{
		PlayerPrefs.DeleteKey ("UsersName");
	}

	public void SaveAudioSetting(int state)
	{
		PlayerPrefs.SetInt ("AudioState",state);
	}
	public int GetAudioSetting()
	{
		if (PlayerPrefs.HasKey ("AudioState")) {
			return PlayerPrefs.GetInt ("AudioState");
		} else
			return 1;
	}
}
public enum GameState
{
	None,
	CountDown,
	StopBet,
	Resulting
}

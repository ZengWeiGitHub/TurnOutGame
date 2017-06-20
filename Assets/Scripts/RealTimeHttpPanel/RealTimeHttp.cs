using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class RealTimeHttp : SingletonMonoBehaviour<RealTimeHttp> {
	
	private string url = "/api/ft/rtinfo";
	private Dictionary<string,int> tempDic = new Dictionary<string, int>();
	private Dictionary<string,object> jsonDic = new Dictionary<string, object>();
	private bool startPost = false;

	public void Init()
	{
		tempDic.Clear ();
		tempDic.Add("lastId",0);
		jsonDic.Add("data",tempDic);
		jsonDic.Add ("token", GameData.Instance.Token);
		startPost = true;
	}
	// Update is called once per frame
	void Update () {

		if (startPost) {
			startPost = false;
			Post();
		}
	}

	public void Post(bool restart = false)
	{
		string data = Json.Serialize(jsonDic);
		RestHttpAPI.HttpAPI.Instance.Post(url,data,CallBack);
	}
	private void CallBack(string wwwText,string errorText)
	{
		Dictionary<string,object> resultJsonDic = Json.Deserialize(wwwText) as Dictionary<string,object>;
		int flag = (int)resultJsonDic["flag"];
		startPost = true;
		if(flag == 1)
		{
			Dictionary<string,object> temp;
			try
			{
				temp = resultJsonDic["data"] as Dictionary<string,object>;
			}
			catch
			{
				Debug.Log("data is empty");
				return;
			}
//			if((int)tempDic["lastId"] == 0)
//				return;
			List<object> msgs = temp["msgs"] as List<object>;
			tempDic["lastId"] =  (int)temp["lastId"];
			jsonDic["data"] = tempDic;
			CallBackData(msgs);
		}
		else if(flag == 0)
		{
			string error = resultJsonDic["err"].ToString();
			Debug.LogError("RealTimeHttp fail:" + error);
			TipsManager.Instance.ShowTips(error);
		}
	}
	void CallBackData(List<object> msgs)
	{
		for(int i = 0; i < msgs.Count; i++)
		{
			Dictionary<string,object> tempMsgs = msgs[i] as Dictionary<string,object>;
			int intType = (int)(tempMsgs["type"]);
			Debug.Log("intType: " + intType);
			switch(intType)
			{
			case 1:

				GameData.Instance.DrawTime = float.Parse(System.Convert.ToString(tempMsgs["drawTime"]));
				GameData.Instance.LeftDrawTime = float.Parse(System.Convert.ToString(tempMsgs["leftDrawTime"]));
				GameData.Instance.LeftBuyTime = float.Parse(System.Convert.ToString(tempMsgs["leftBuyTime"]));
				GameData.Instance.TotalPoint = (int)tempMsgs["totalPoint"];
				GameData.Instance.TermNo = tempMsgs["termNo"].ToString();
				GameData.Instance.PreTermNo = tempMsgs["preTermNo"].ToString();
				try
				{
					GameData.Instance.TotalUsers = (int)tempMsgs["totalUsers"];
				}
				catch
				{
					Debug.Log("totalUsers is empty");
				}
				try
				{
					GameData.Instance.MyPoint = (int)tempMsgs["myPoint"];
				}
				catch
				{
					Debug.Log("myPoint is empty");
				}

				break;
			case 2:
//				if(!GameData.Instance.SystemInfoDic.ContainsKey("content"))
//					GameData.Instance.SystemInfoDic.Add("content",tempMsgs["content"]);
//				else
//					GameData.Instance.SystemInfoDic["content"] = tempMsgs["content"];
				break;
			case 3:
				break;
			case 4:
				try
				{
					GameData.Instance.Point = (int)tempMsgs["point"];
				}
				catch
				{

				}
				try
				{
					GameData.Instance.Balance = (int)tempMsgs["balance"];
				}
				catch
				{

				}

				break;
			case 5:
				GameData.Instance.LotteryNumbers = tempMsgs["numbers"].ToString();
				break;
			case 6:
				GameData.Instance.Point = (int)tempMsgs["point"];
				break;
			}
		}
	}
}

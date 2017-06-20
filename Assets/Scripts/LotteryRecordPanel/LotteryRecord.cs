using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class LotteryRecord : SingletonBase<LotteryRecord> {


	public void Post()
	{
		string url = "/api/ft/draws";
		Dictionary<string,object> tempDic = new Dictionary<string, object>();
		Dictionary<string,object> jsonDic = new Dictionary<string, object>();
		tempDic.Add("pageSize",10);
		tempDic.Add("sort",0);
		jsonDic.Add("data",tempDic);

		jsonDic.Add ("token", GameData.Instance.Token);

		string data = Json.Serialize(jsonDic);

		RestHttpAPI.HttpAPI.Instance.Post(url,data,CallBack);
	}
	private void CallBack(string wwwText,string errorText)
	{
		Dictionary<string,object> resultJsonDic = Json.Deserialize(wwwText) as Dictionary<string,object>;
		int flag = (int)resultJsonDic["flag"];
		if(flag == 1)
		{
			Debug.Log("LotteryRecord sucsess");
			try
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
				GameData.Instance.LotteryRecordList.Clear();
				GameData.Instance.LotteryRecordList = temp["draws"] as List<object>;
				GameData.Instance.isGetHistoryInfoSuccess = true;
			}
			catch
			{

			}
		}
		else if(flag == 0)
		{
			string error = resultJsonDic["err"].ToString();
			Debug.LogError("PointOperation fail:" + error);
		}
	}
}

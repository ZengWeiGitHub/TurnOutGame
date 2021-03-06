﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class GetSetting : SingletonBase<GetSetting> {

	public void Post()
	{
		string url = "/api/getsetting"; ;
		Dictionary<string,object> tempDic = new Dictionary<string, object>();
		Dictionary<string,object> jsonDic = new Dictionary<string, object>();
		tempDic.Add("gameId",1);
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
			Debug.Log("GetSetting sucsess");
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
				GameData.Instance.SettingList = temp["rules"] as List<object>;
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

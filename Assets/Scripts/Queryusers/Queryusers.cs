using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class Queryusers : SingletonBase<Queryusers> {

	public void Post()
	{
		string url = "/api/queryusers";
		Dictionary<string,object> tempDic = new Dictionary<string, object>();
		Dictionary<string,object> jsonDic = new Dictionary<string, object>();
		tempDic.Add ("pageSize",60);
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
			Debug.Log("Queryusers sucsess");
			Dictionary<string,object> temp;
			try
			{
				temp = resultJsonDic["data"] as Dictionary<string,object>;
				List<object> tempObjectList = new List<object>();
				tempObjectList = temp["users"] as List<object>;
				GameData.Instance.TheUesrDic.Clear();
				for(int i = 0; i < tempObjectList.Count; i++)
				{
					Dictionary<string,object> tempDic = tempObjectList[i] as Dictionary<string,object>;
					GameData.Instance.TheUesrDic.Add(tempDic ["code"].ToString (),tempDic);
				}
				//GameData.Instance.TheUsers = temp["users"] as List<object>;
				GameData.Instance.isGetTheUsersInfoSuccess = true;
			}
			catch
			{
				Debug.Log("data is empty");
				return;
			}

		}
		else if(flag == 0)
		{
			string error = resultJsonDic["err"].ToString();
			Debug.LogError("Queryusers fail:" + error);
			TipsManager.Instance.ShowTips(error);
		}
	}
}

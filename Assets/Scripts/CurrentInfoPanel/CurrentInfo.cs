using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class CurrentInfo : SingletonBase<CurrentInfo> {

	public void Post()
	{
		string url = "/api/ft/current";

		Dictionary<string,object> jsonDic = new Dictionary<string, object>();

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
			Debug.Log("PointOperation sucsess");
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
			GameData.Instance.TermNo = temp["termNo"].ToString();

			GameData.Instance.DrawTime = float.Parse(System.Convert.ToString(temp["drawTime"]));
			GameData.Instance.LeftBuyTime = float.Parse(System.Convert.ToString(temp["leftBuyTime"]));
			GameData.Instance.LeftDrawTime = float.Parse(System.Convert.ToString(temp["leftDrawTime"]));
			try
			{
				GameData.Instance.MyPoint = (int)temp["myPoint"];
			}
			catch
			{
				Debug.Log("myPoint is empty");
			}

			GameData.Instance.TotalPoint = (int)temp["totalPoint"];
			try
			{
				GameData.Instance.TotalUsers = (int)temp["totalUsers"];
			}
			catch
			{
				Debug.Log("totalUsers is empty");
			}
			try
			{
				GameData.Instance.PreNumbers = temp["preNumbers"].ToString();
			}
			catch
			{
				Debug.Log("preNumbers is empty");
			}

			GameData.Instance.PreTermNo = temp["preTermNo"].ToString();
			//int serverTime = (int)temp["serverTime"];

			GameData.Instance.isGetCurrentInfoSuccess = true;
		}
		else if(flag == 0)
		{
			string error = resultJsonDic["err"].ToString();
			Debug.LogError("PointOperation fail:" + error);
			TipsManager.Instance.ShowTips(error);
		}
	}
}

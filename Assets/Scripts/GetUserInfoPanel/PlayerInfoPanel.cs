using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class PlayerInfoPanel : SingletonBase<PlayerInfoPanel> {

	public void Post()
	{
		string url = "/api/userinfo";
		Dictionary<string,string> tempDic = new Dictionary<string, string>();
		Dictionary<string,object> jsonDic = new Dictionary<string, object>();
		//tempDic.Add("userCode","123480");
		jsonDic.Add("data",tempDic);
		jsonDic.Add ("token",GameData.Instance.Token);

		string data = Json.Serialize(jsonDic);

		RestHttpAPI.HttpAPI.Instance.Post(url,data,CallBack);
	}
	private void CallBack(string wwwText,string errorText)
	{
		Dictionary<string,object> resultJsonDic = Json.Deserialize(wwwText) as Dictionary<string,object>;
		int flag = (int)resultJsonDic["flag"];
		if(flag == 1)
		{
			Debug.Log("Get UserInfo sucsess");
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

			GameData.Instance.UserName = temp["username"].ToString();
			GameData.Instance.NickName = temp["nickname"].ToString();
			GameData.Instance.UserCode = temp["userCode"].ToString();
			GameData.Instance.Balance = (int)temp["balance"];
			GameData.Instance.Point = (int)temp["point"];
			int createTime = (int)temp["createTime"];
			List<object> rolesList = temp["roles"] as List<object>;
			if(rolesList.Count > 1)
			{
				GameData.Instance.IsAgent = true;
			}
			else
			{
				GameData.Instance.IsAgent = false;
			}

			GameData.Instance.isGetUserInfoSuccess = true;
			try
			{
				GameData.Instance.AgentCode = temp["agentCode"].ToString();
			}
			catch
			{
				Debug.Log("agentCode is empty");
			}
			try
			{
				string agentNickName= temp["agentNickName"].ToString();
			}
			catch
			{
				Debug.Log("agentNickName is empty");
			}
			try
			{
				GameData.Instance.ReferrerCode = temp["referrerCode"].ToString();
			}
			catch
			{
				Debug.Log("referrerCode is empty");
			}
			try
			{
				string referrerNickName = temp["referrerNickName"].ToString();
			}
			catch
			{
				Debug.Log("referrerNickName is empty");
			}
		}
		else if(flag == 0)
		{
			string error = resultJsonDic["err"].ToString();
			Debug.LogError("Get UserInfo fail:" + error);
			TipsManager.Instance.ShowTips(error);
		}
	}
}

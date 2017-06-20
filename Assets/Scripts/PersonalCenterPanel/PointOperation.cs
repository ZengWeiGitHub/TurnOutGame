using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class PointOperation : MonoBehaviour {

	public void OnClick()
	{
		string url = "/api/changeuserpoint";
		Dictionary<string,object> tempDic = new Dictionary<string, object>();
		Dictionary<string,object> jsonDic = new Dictionary<string, object>();
		tempDic.Add("userCode",123481);
		tempDic.Add("type",2);
		tempDic.Add("point",200);
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
			Debug.Log("PointOperation sucsess");
			int point = (int)resultJsonDic["point"];
		}
		else if(flag == 0)
		{
			string error = resultJsonDic["err"].ToString();
			Debug.LogError("PointOperation fail:" + error);
			TipsManager.Instance.ShowTips(error);
		}
	}
}

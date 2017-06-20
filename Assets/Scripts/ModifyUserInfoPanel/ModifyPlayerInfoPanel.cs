using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class ModifyPlayerInfoPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnClick()
	{
		string url = "/api/changeuserinfo";
		Dictionary<string,string> tempDic = new Dictionary<string, string>();
		Dictionary<string,object> jsonDic = new Dictionary<string, object>();
		tempDic.Add("userCode","123580");
		tempDic.Add("nickName","newNickName");
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
			Debug.Log("ModifyPlayerInfo sucsess");
		}
		else if(flag == 0)
		{
			string error = resultJsonDic["err"].ToString();
			Debug.LogError("ModifyPlayerInfo fail:" + error);
			TipsManager.Instance.ShowTips(error);
		}
	}
}

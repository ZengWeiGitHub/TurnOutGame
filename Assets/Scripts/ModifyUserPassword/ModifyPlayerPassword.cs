using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class ModifyPlayerPassword : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnClick()
	{
		string url = "/api/changepw";
		Dictionary<string,string> tempDic = new Dictionary<string, string>();
		Dictionary<string,Dictionary<string,string>> jsonDic = new Dictionary<string, Dictionary<string,string>>();
		tempDic.Add("userName","ilovethegame");
		tempDic.Add("oldPassword","password4thegame");
		jsonDic.Add("data",tempDic);

		string data = Json.Serialize(jsonDic);

		RestHttpAPI.HttpAPI.Instance.Post(url,data,CallBack);
	}
	private void CallBack(string wwwText,string errorText)
	{
		Dictionary<string,object> resultJsonDic = Json.Deserialize(wwwText) as Dictionary<string,object>;
		int flag = (int)resultJsonDic["flag"];
		if(flag == 1)
		{
			Debug.Log("ModifyPlayerPassword sucsess");
		}
		else if(flag == 0)
		{
			string error = resultJsonDic["err"].ToString();
			Debug.LogError("ModifyPlayerPassword fail:" + error);
			TipsManager.Instance.ShowTips(error);
		}
	}
}

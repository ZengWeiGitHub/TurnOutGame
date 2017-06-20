using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class FeedbackProblem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnClick()
	{
		string url = "/api/feedback";
		Dictionary<string,object> tempDic = new Dictionary<string, object>();
		Dictionary<string,object> jsonDic = new Dictionary<string, object>();
		tempDic.Add("type",1);
		tempDic.Add("content","怎样参与游戏");
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
			Debug.Log("FeedbackProblem sucsess");
		}
		else if(flag == 0)
		{
			string error = resultJsonDic["err"].ToString();
			Debug.LogError("FeedbackProblem fail:" + error);
			TipsManager.Instance.ShowTips(error);
		}
	}
}

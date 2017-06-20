using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class FeekBack : MonoBehaviour {

	public EasyFontTextMesh text;

	public void Commit()
	{
		if(string.IsNullOrEmpty(text.text))
		{
			TipsManager.Instance.ShowTips("反馈的问题为空");
			return;
		}
		string url = "/api/feedback";
		Dictionary<string,object> tempDic = new Dictionary<string, object>();
		Dictionary<string,object> jsonDic = new Dictionary<string, object>();
		tempDic.Add("type",1);
		tempDic.Add("content",text.text);
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
			TipsManager.Instance.ShowTips("反馈成功");
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

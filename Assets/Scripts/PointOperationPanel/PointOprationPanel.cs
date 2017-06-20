using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class PointOprationPanel : PanelBase {

	public static PointOprationPanel Instance;

	public EasyFontTextMesh userCodeText, pointText,pointTitleText;
	private int point;

	private PointOpration _curPointOpration;
	public PointOpration CurPointOpration
	{
		set
		{
			_curPointOpration = value;
			if(_curPointOpration == PointOpration.Up)
			{
				pointTitleText.text = "上分";
			}
			else if(_curPointOpration == PointOpration.Down)
			{
				pointTitleText.text = "下分";
			}
		}
	}

	private bool isCanOnClick = true;

	public override void InitPanel ()
	{
		base.InitPanel ();

		Instance = this;

		isCanOnClick = true;
	}


	public void SureOnClick()
	{
		if (!isCanOnClick)
			return;
		if (string.IsNullOrEmpty (userCodeText.text)) {
			TipsManager.Instance.ShowTips("用户识别码不能为空");
			return;
		}
		point = int.Parse(pointText.text);
		if(point < 0)
		{
			TipsManager.Instance.ShowTips("分数不能为负数");
			return;
		}
		if (_curPointOpration == PointOpration.Up) {
			point = Mathf.Abs(point);

		} else if (_curPointOpration == PointOpration.Down){
			point = -Mathf.Abs(point);
		}
		Post ();
	}
	public void CancleOnClick()
	{
		if (!isCanOnClick)
			return;
		UIManager.Instance.HidePanel (PanelType.PointOprationPanel);
	}

	public enum PointOpration
	{
		Up,
		Down
	}

	public void Post()
	{
		isCanOnClick = false;
		string url = "/api/changeuserpoint";
		Dictionary<string,object> tempDic = new Dictionary<string, object>();
		Dictionary<string,object> jsonDic = new Dictionary<string, object>();
		tempDic.Add("userCode",userCodeText.text);
		tempDic.Add("point",point);
		jsonDic.Add("data",tempDic);
		jsonDic.Add ("token", GameData.Instance.Token);
		
		string data = Json.Serialize(jsonDic);
		
		RestHttpAPI.HttpAPI.Instance.Post(url,data,CallBack);
	}
	private void CallBack(string wwwText,string errorText)
	{
		isCanOnClick = true;
		Dictionary<string,object> resultJsonDic = Json.Deserialize(wwwText) as Dictionary<string,object>;
		int flag = (int)resultJsonDic["flag"];
		if(flag == 1)
		{
			TipsManager.Instance.ShowTips(pointTitleText.text + "成功");
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
			int point = (int)temp["balance"];
			GameData.Instance.TheUesrDic[userCodeText.text]["balance"] = point.ToString();
			PointManager.Instance.usersDic[userCodeText.text].UpdateBalancePoint(point);
		}
		else if(flag == 0)
		{
			string error = resultJsonDic["err"].ToString();
			Debug.LogError("PointOperation fail:" + error);
			TipsManager.Instance.ShowTips(error);
		}
	}
}

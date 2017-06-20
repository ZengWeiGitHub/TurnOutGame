using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using DG.Tweening;

public class LoginPanel : PanelBase {

	public tk2dUITextInput passwordText;
	public EasyFontTextMesh inputUserNameText,tips;

	private bool isCanOnClick = true;

	public override void InitPanel ()
	{
		base.InitPanel ();

		inputUserNameText.text = GameData.Instance.GetUserName ();

		isCanOnClick = true;
	}


	public void OnClick()
	{
		if(!isCanOnClick)
		{
			return;
		}
		isCanOnClick = false;
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);

		string url = "/api/login";
		Dictionary<string,string> tempDic = new Dictionary<string, string>();
		Dictionary<string,object> loginInfoDic = new Dictionary<string, object>();
		tempDic.Add("username",inputUserNameText.text);
		tempDic.Add("password",passwordText.Text);
		loginInfoDic.Add("data",tempDic);
		
		string data = Json.Serialize(loginInfoDic);
		
		RestHttpAPI.HttpAPI.Instance.Post(url,data,CallBack);
	}
	private void CallBack(string wwwText,string errorText)
	{
		isCanOnClick = true;
		Dictionary<string,object> resultJsonDic = Json.Deserialize(wwwText) as Dictionary<string,object>;
		try
		{
			int flag = (int)resultJsonDic["flag"];
			if(flag == 1)
			{
				Debug.Log("login sucsess");
				GameData.Instance.bLoginSuccess = true;
				GameData.Instance.SaveUserName(inputUserNameText.text);
				Dictionary<string,object> temp = resultJsonDic["data"] as Dictionary<string,object>;
				GameData.Instance.Token = temp["token"].ToString();

				GameData.Instance.PassWord = passwordText.Text;
				
				List<object> rolesList = temp["roles"] as List<object>;
				
				
				string[] roles = new string[3];


				int expiredTime = (int)temp["expiredTime"];

				UIManager.Instance.HidePanel(PanelType.LoginPanel);
				MainInterfacePanel.Instance.MoveOutScree();
				UIManager.Instance.loadingPageScr.gameObject.SetActive(true);
			}
			else if(flag == 0)
			{
				string error = resultJsonDic["err"].ToString();
				Debug.LogError("login fail:" + error);
				TipsManager.Instance.ShowTips(error);
			}
		}
		catch
		{
			
		}
	}

	void TipsEffect(string errorText)
	{
		if (DOTween.IsTweening ("TipsEffect"))
			return;
		tips.transform.localPosition = new Vector3 (-112,-200,0);
		string text = "";
		switch(errorText)
		{
		case "1001":
			text = "用户名不能为空";
			break;
		case "1002":
			text = "密码不能为空";
			break;
		case "1004":
			text = "用户名与密码不匹配";
			break;
		}
		tips.text = text;
		tips.transform.gameObject.SetActive (true);
		tips.transform.DOLocalMoveY (-50,1f).SetId("TipsEffect").OnComplete(HideTips);
	}
	void HideTips()
	{
		tips.transform.gameObject.SetActive (false);
	}
}

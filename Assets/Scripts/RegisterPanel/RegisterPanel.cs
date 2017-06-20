using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using DG.Tweening;

public class RegisterPanel : PanelBase {

	public tk2dUITextInput passwordText;
	public EasyFontTextMesh inputUserNameText,inputNickNameText,tips;
	private bool isCanOnClick = true;

	public override void InitPanel ()
	{
		base.InitPanel ();
		isCanOnClick = true;
	}



	public void  OnClick()
	{
		if(!isCanOnClick)
		{
			return;
		}
		if(string.IsNullOrEmpty(inputNickNameText.text))
		{
			TipsManager.Instance.ShowTips("推荐人ID不能为空");
			return;
		}
		isCanOnClick = false;
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);
		string url = "/api/register";
		Dictionary<string,string> tempDic = new Dictionary<string, string>();
		Dictionary<string,Dictionary<string,string>> registerInfoDic = new Dictionary<string, Dictionary<string, string>>();
		tempDic.Add("username",inputUserNameText.text);
		tempDic.Add("password",passwordText.Text);
		tempDic.Add("nickName",inputUserNameText.text);
		tempDic.Add("referrerCode",inputNickNameText.text);
		registerInfoDic.Add("data",tempDic);
		
		string data = Json.Serialize(registerInfoDic);
		
		RestHttpAPI.HttpAPI.Instance.Post(url,data,CallBack);

	}

	private void CallBack(string wwwText,string wwwError)
	{
		isCanOnClick = true;
		Dictionary<string,object> resultJsonDic = Json.Deserialize(wwwText) as Dictionary<string,object>;
		try
		{
			int flag = (int)resultJsonDic["flag"];
			if(flag == 1)
			{
				Debug.Log("register sucsess");
				GameData.Instance.bLoginSuccess = true;
				GameData.Instance.SaveUserName(inputUserNameText.text);
				GameData.Instance.PassWord = passwordText.Text;
				UIManager.Instance.HidePanel(PanelType.RegisterPanel);
				MainInterfacePanel.Instance.MoveOutScree();
				UIManager.Instance.loadingPageScr.gameObject.SetActive(true);
			}
			else if(flag == 0)
			{
				string error = resultJsonDic["err"].ToString();
				Debug.LogError("register fail:" + error);
				TipsManager.Instance.ShowTips(error);
			}
		}
		catch
		{
			
		}
	}
	public void CloseBtn()
	{
		UIManager.Instance.HidePanel (PanelType.RegisterPanel);
	}void TipsEffect(string errorText)
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
		case "1003":
			text = "用户名已存在或不可用";
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

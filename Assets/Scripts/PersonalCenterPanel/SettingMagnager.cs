using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class SettingMagnager : MonoBehaviour {
	public GameObject OnOnAudioRightObj, OffOnAudioRightObj;

	public EasyFontTextMesh nickNameText, passwordText;

	private bool isCanOnClick;

	public void Init()
	{
		isCanOnClick = true;
		OnOnAudioRightObj.SetActive (true);
		OffOnAudioRightObj.SetActive (false);
		nickNameText.text = GameData.Instance.NickName;
		passwordText.text = GameData.Instance.PassWord;
		GameData.Instance.NickNameChangeEvent += UpdateNickName;
		GameData.Instance.PasswordChangeEvent += UpdatePassword;
		SetAudioState (GameData.Instance.GetAudioSetting() == 1?true:false);
	}

	void UpdateNickName(string name)
	{
		nickNameText.text = name;
	}
	void UpdatePassword(string name)
	{
		passwordText.text = name;
	}

	void SetAudioState(bool state)
	{
		if (state) {
			OnOnAudioRightObj.SetActive (true);
			OffOnAudioRightObj.SetActive (false);
		} else {
			OnOnAudioRightObj.SetActive (false);
			OffOnAudioRightObj.SetActive (true);
		}
	}

	public void OnAudioOnClick()
	{
		SetAudioState (true);
		GameData.Instance.SaveAudioSetting (1);
		AudioManager.Instance.UnPauseAll ();
		AudioManager.Instance.PlayMusic (MusicName.UIBackground);
		
	}
	public void OffnAudioOnClick()
	{
		SetAudioState (false);
		AudioManager.Instance.StopAll ();
		GameData.Instance.SaveAudioSetting (0);
	}
	void ModifyUserNamePost()
	{
		if(!isCanOnClick)
		{
			return;
		}
		isCanOnClick = false;
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);
		string url = "/api/changeuserinfo";
		Dictionary<string,string> tempDic = new Dictionary<string, string>();
		Dictionary<string,object> jsonDic = new Dictionary<string, object>();
		tempDic.Add("userCode",GameData.Instance.UserCode);
		tempDic.Add("userName",GameData.Instance.UserName);
		tempDic.Add("nickName",GameData.Instance.NickName);
		tempDic.Add("password",GameData.Instance.PassWord);
		jsonDic.Add("data",tempDic);
		jsonDic.Add ("token",GameData.Instance.Token);
		
		string data = Json.Serialize(jsonDic);
		
		RestHttpAPI.HttpAPI.Instance.Post(url,data,ModifyUserNameCallBack);
	}
	void ModifyUserNameCallBack(string wwwText,string errorText)
	{
		isCanOnClick = true;
		Dictionary<string,object> resultJsonDic = Json.Deserialize(wwwText) as Dictionary<string,object>;
		int flag = (int)resultJsonDic["flag"];
		if(flag == 1)
		{
			Debug.Log("ModifyPlayerInfo sucsess");
			GameData.Instance.NickName = nickNameText.text;
			//GameData.Instance.SaveUserName(nickNameText.text);
			TipsManager.Instance.ShowTips("修改用户昵称成功");
		}
		else if(flag == 0)
		{
			string error = resultJsonDic["err"].ToString();
			Debug.LogError("ModifyPlayerInfo fail:" + error);
			TipsManager.Instance.ShowTips(error);
		}
	}
	void ModifyPasswordPost()
	{
		if(!isCanOnClick)
		{
			return;
		}
		isCanOnClick = false;
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);
		string url = "/api/changepw";
		Dictionary<string,string> tempDic = new Dictionary<string, string>();
		Dictionary<string,object> jsonDic = new Dictionary<string, object>();
		tempDic.Add("username",GameData.Instance.UserName);
		tempDic.Add("oldPassword",GameData.Instance.PassWord);
		tempDic.Add("newPassword",passwordText.text);
		jsonDic.Add("data",tempDic);
		jsonDic.Add ("token",GameData.Instance.Token);
		
		string data = Json.Serialize(jsonDic);
		
		RestHttpAPI.HttpAPI.Instance.Post(url,data,ModifyPasswordCallBack);
	}
	void ModifyPasswordCallBack(string wwwText,string errorText)
	{
		isCanOnClick = true;
		Dictionary<string,object> resultJsonDic = Json.Deserialize(wwwText) as Dictionary<string,object>;
		int flag = (int)resultJsonDic["flag"];
		if(flag == 1)
		{
			Debug.Log("ModifyPlayerPassword sucsess");
			GameData.Instance.PassWord = passwordText.text;
			TipsManager.Instance.ShowTips("修改密码成功");
		}
		else if(flag == 0)
		{
			string error = resultJsonDic["err"].ToString();
			Debug.LogError("ModifyPlayerPassword fail:" + error);
			TipsManager.Instance.ShowTips(error);
		}
	}
}

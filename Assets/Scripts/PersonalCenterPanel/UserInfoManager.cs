using UnityEngine;
using System.Collections;

public class UserInfoManager : MonoBehaviour {

	public EasyFontTextMesh userNameText,userCodeText, nickNameText, pointText, agentCodeText, referrerCodeText;

	public void Init()
	{
		userNameText.text = GameData.Instance.UserName;
		userCodeText.text = GameData.Instance.UserCode;
		nickNameText.text = GameData.Instance.NickName;
		pointText.text = GameData.Instance.Point.ToString();
		agentCodeText.text = GameData.Instance.AgentCode;
		referrerCodeText.text = GameData.Instance.ReferrerCode;

		GameData.Instance.PointChangeEvent += PointChange;
		GameData.Instance.UserNameChangeEvent += UpdateUserName;
		GameData.Instance.NickNameChangeEvent += UpdateNickName;
	}
	void UpdateNickName(string name)
	{
		nickNameText.text = name;
	}
	void UpdateUserName(string name)
	{
		userNameText.text = name;
	}
	void PointChange(int point)
	{
		pointText.text = point.ToString();
	}
}

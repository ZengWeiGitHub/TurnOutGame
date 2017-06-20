using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TipsManager : SingletonMonoBehaviour<TipsManager> {
	
	public EasyFontTextMesh TipsText;
	
	public void ShowTips(string content)
	{
		if (DOTween.IsTweening ("TipsEffect"))
			return;
		TipsText.transform.position = Vector3.zero;
		TipsText.gameObject.SetActive (true);
		SetText (content);
		TipsText.transform.DOLocalMoveY (100,2f).SetId("TipsEffect").OnComplete(HideTips);
	}
	void HideTips()
	{
		TipsText.transform.gameObject.SetActive (false);
	}

	void SetText(string errorText)
	{
		if (DOTween.IsTweening ("TipsEffect"))
			return;
		string text = "";
		switch(errorText)
		{
		case "0001":
			text = "系统错误";
			break;
		case "0002":
			text = "参数错误";
			break;
		case "0003":
			text = "无效的功能";
			break;
		case "1001":
			text = "用户名不能为空";
			break;
		case "1002":
			text = "密码不能为空";
			break;
		case "1003":
			text = "用户名已存在或不可用";
			break;
		case "1004":
			text = "用户名与密码不匹配";
			break;
		case "1005":
			text = "介绍人识别码无效";
			break;
		case "1006":
			text = "所属代理识别码无效";
			break;
		case "1007":
			text = "Token已过期或无效";
			break;
		case "1008":
			text = "没有此用户";
			break;
		case "1009":
			text = "无操作权限";
			break;
		case "1010":
			text = "别处登陆";
			break;
		case "2001":
			text = "积分不够";
			break;
		case "3001":
			text = "该期不能下注";
			break;
		case "3002":
			text = "不能替该用户下注";
			break;
		case "3003":
			text = "下注命令无法识别";
			break;
		case "3004":
			text = "该期已经不能取消下注";
			break;
		default:
			text = errorText;
			break;
		}
		TipsText.text = text;
	}
}

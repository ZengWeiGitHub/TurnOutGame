using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class BalanceManager : MonoBehaviour {

	public GameObject DepositContentObj;
	public GameObject WithdrawContentObj;

	public EasyFontTextMesh acountPointText,balancePoint,inputPointText;


	private PointChangeType currentType = PointChangeType.Deposit;

	public void Init()
	{
		currentType = PointChangeType.Deposit;
		DepositContentObj.SetActive (true);
		WithdrawContentObj.SetActive (false);
		acountPointText.text = GameData.Instance.Point.ToString ();
		balancePoint.text = GameData.Instance.Balance.ToString ();
		GameData.Instance.PointChangeEvent += PointChange;
		GameData.Instance.BalancePointChangeEvent += BalancePointChange;
	}

	void PointChange(int point)
	{
		acountPointText.text = point.ToString ();
	}
	void BalancePointChange(int point)
	{
		balancePoint.text = point.ToString ();
	}

	//存款点击
	public void DepositOnClick()
	{
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);
		DepositContentObj.SetActive (true);
		WithdrawContentObj.SetActive (false);
		currentType = PointChangeType.Deposit;
		
	}
	//取款点击
	public void WithdrawOnClick()
	{
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);
		DepositContentObj.SetActive (false);
		WithdrawContentObj.SetActive (true);
		currentType = PointChangeType.Withdraw;
	}

	public void SureOnClick()
	{
		Post ();
	}

	public void Post()
	{
		string url = "/api/transferpoint";
		Dictionary<string,object> tempDic = new Dictionary<string, object>();
		Dictionary<string,object> jsonDic = new Dictionary<string, object>();
		tempDic.Add("point",inputPointText.text);
		tempDic.Add("password",GameData.Instance.PassWord);
		tempDic.Add("direction",(int)currentType);
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
			Debug.Log("transferpoint sucsess");
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
			GameData.Instance.Balance = (int)temp["balance"];
			GameData.Instance.Point = (int)temp["point"];
			

		}
		else if(flag == 0)
		{
			string error = resultJsonDic["err"].ToString();
			Debug.LogError("transferpoint:" + error);
			TipsManager.Instance.ShowTips(error);
		}
	}
}

public enum PointChangeType
{
	Deposit = 1,
	Withdraw
}

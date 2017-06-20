using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointOperationItem : MonoBehaviour {
	public EasyFontTextMesh userNameText, userCodeText, pointText, balancePointText;
	public void Init(Dictionary<string,object> dic)
	{
		userNameText.text = dic ["username"].ToString ();
		userCodeText.text = dic ["code"].ToString ();
		pointText.text = dic ["point"].ToString ();
		balancePointText.text = dic ["balance"].ToString ();
	}

	public void UpdateBalancePoint(int balance)
	{
		balancePointText.text = balance.ToString ();
	}
}

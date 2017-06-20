using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HistoryManager : MonoBehaviour {

	public Dictionary<string,List<string>> lotteryRecordDic = new Dictionary<string, List<string>>();

	public List<GameObject> historyItemObjList = new List<GameObject> ();


	public HistoryItem[] lastFourHistory;

	public Transform moreHistoryButtonTran;
	public Transform moreHistoryItemParent;
	public tk2dUIScrollableArea scrollbar;

	private Vector3 startPos = new Vector3 (-420,200,0);
	private float spanDis = 100;


	public void InitLotteryRecordDic(List<object> draws)
	{
		int count = draws.Count;
		lotteryRecordDic.Clear ();
		for(int i = 0; i < count; i++)
		{
			string temp = draws[i].ToString();
			string[] firstSplit = temp.Split(':');
			if(firstSplit.Length < 3)
				continue;
			string[] secondSplit = firstSplit[1].Split(',');
			List<string> tempList = new List<string>();
			for(int j = 0; j < secondSplit.Length; j++)
			{
				tempList.Add(secondSplit[j]);
			}
			tempList.Add(firstSplit[2]);
			if(!lotteryRecordDic.ContainsKey(firstSplit[0]))
			{
				lotteryRecordDic.Add(firstSplit[0],tempList);
			}
		}

	}
	public void InitPanel()
	{
		int count = lotteryRecordDic.Count;
		int i = 0;
		if (count <= 5)
			scrollbar.ContentLength = 1;
		else
			scrollbar.ContentLength = (count - 5) * 100;
		for(int j = 0; j < historyItemObjList.Count; j++)
		{
			Destroy(historyItemObjList[j]);
		}
		historyItemObjList.Clear ();
		foreach(string key in lotteryRecordDic.Keys)
		{
			GameObject obj = ResoureManager.Instance.LoadUIPrefab("HistoryItem");
			if(i < 4)
			{
				lastFourHistory[i].Init(key,lotteryRecordDic[key]);
				lastFourHistory[i].gameObject.SetActive(true);
			}
			obj.GetComponent<HistoryItem>().Init(key,lotteryRecordDic[key]);
			obj.transform.parent = moreHistoryItemParent;
			obj.transform.localPosition = startPos - i * Vector3.up * spanDis;
			i++;
			historyItemObjList.Add(obj);
		}
	}

	private ButtonState _curButtonState = ButtonState.Close;
	public ButtonState CurButtonState
	{
		set
		{
			_curButtonState = value;
			if(_curButtonState == ButtonState.Close)
			{
				BetPanel.Instance.isOpenHistory = false;
				moreHistoryButtonTran.localEulerAngles = new Vector3(0,0,90);
				scrollbar.gameObject.SetActive(false);
			}
			else if(_curButtonState == ButtonState.Open)
			{
				BetPanel.Instance.isOpenHistory = true;
				scrollbar.gameObject.SetActive(true);
				moreHistoryButtonTran.localEulerAngles = new Vector3(0,0,0);
			}
		}
		get
		{
			return _curButtonState;
		}
	}
	public void MoreHistoryOnClButtonStateick()
	{
		if (CurButtonState == ButtonState.Close)
			CurButtonState = ButtonState.Open;
		else if(CurButtonState == ButtonState.Open)
			CurButtonState = ButtonState.Close;
	}
	public enum ButtonState
	{
		Close,
		Open
	}
}

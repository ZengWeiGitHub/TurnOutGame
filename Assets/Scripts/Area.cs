using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Area : MonoBehaviour {
	public int Id;
	public EasyFontTextMesh betedChipCountText,chipCountText,rateText;
	public string name;
	public int chipCount = 0,betedChipCount;
	private List<GameObject> selectedChipObjList = new List<GameObject>();
	private ChipType curType = ChipType.None;

	public void Init(int id)
	{
		Id = id;
		chipCount = 0;
		betedChipCount = 0;
		selectedChipObjList.Clear ();
		betedChipCountText.gameObject.SetActive (false);
		curType = ChipType.None;
		rateText = transform.FindChild ("OddsText").GetComponent<EasyFontTextMesh>();
		name = GameData.Instance.areaNameDic[Id];
		InitRate ();
	}
	void InitRate()
	{
		switch(name)
		{
		case "1-2":
		case "1-3":
		case "1-4":
			rateText.text = "1:" + BetPanel.Instance.rateDic["1-2"];
			break;
		case "2-1":
		case "2-3":
		case "2-4":
			rateText.text = "1:" + BetPanel.Instance.rateDic["2-1"];
			break;
		case "3-1":
		case "3-2":
		case "3-4":
			rateText.text = "1:" + BetPanel.Instance.rateDic["3-1"];
			break;
		case "4-1":
		case "4-2":
		case "4-3":
			rateText.text = "1:" + BetPanel.Instance.rateDic["4-1"];
			break;
		default:
			rateText.text = "1:" + BetPanel.Instance.rateDic[name];
			break;

		}
	}
	public void OnClick()
	{
		if (BetPanel.Instance.isOpenHistory)
			return;

		if (GameData.Instance.currentGameState == GameState.StopBet) {
			TipsManager.Instance.ShowTips ("本期已封盘,请等待下期");
			return;
		} else if(GameData.Instance.currentGameState == GameState.Resulting){
			TipsManager.Instance.ShowTips ("开奖同步中……");
			return;
		}

		if (curType == ChipType.None) {
			curType = BetPanel.Instance.currentBetChipType;
		}
		else if(curType != BetPanel.Instance.currentBetChipType)
		{
			TipsManager.Instance.ShowTips("确认下注后在下注");
			return;
		}

		AudioManager.Instance.PlaySound (SoundName.AddChip);
		Debug.Log ("Id:" + Id);
		BetPanel.Instance.CurrentSelectArea = this;
		chipCount += (int)BetPanel.Instance.currentBetChipType;



		GameObject obj = ResoureManager.Instance.LoadUIPrefab ("Chip");
		obj.GetComponent<tk2dSprite> ().SetSprite (GetChipName());
		obj.transform.position = currentChip.transform.position;
		obj.transform.DOScale (0.5f,0.5f);
		obj.transform.DOLocalMove (this.transform.position,0.5f).OnComplete(AnimCallBack);
		selectedChipObjList.Add (obj);
		BetPanel.Instance.AddToSelectedAreaList (this);
	}

	void AnimCallBack()
	{
		if (!chipCountText.gameObject.activeInHierarchy) {
			chipCountText.gameObject.SetActive(true);
		}
		chipCountText.text = "X" + (chipCount / (int)BetPanel.Instance.currentBetChipType).ToString();
	}
	void ContinueAnimCallBack()
	{
		if (!chipCountText.gameObject.activeInHierarchy) {
			chipCountText.gameObject.SetActive(true);
		}
		chipCountText.text = "X" + chipCount / 10;
	}


	public void CancleBet()
	{
		for(int i = 0; i < selectedChipObjList.Count; i++)
		{
			Destroy(selectedChipObjList[i]);
		}
		chipCount = 0;
		selectedChipObjList.Clear ();
		curType = ChipType.None;
		chipCountText.gameObject.SetActive(false);

	}
	public void SureBet()
	{
		int count = selectedChipObjList.Count;
		if(count > 0)
		{
			for(int i = 0; i < count; i++)
			{
				Destroy(selectedChipObjList[i]);
			}
		}
		betedChipCount += chipCount;
		if (betedChipCount > 0) {
			betedChipCountText.gameObject.SetActive (true);
			betedChipCountText.text = betedChipCount.ToString ();
		} else {
			betedChipCountText.gameObject.SetActive (false);
		}
		chipCount = 0;
		selectedChipObjList.Clear ();
		curType = ChipType.None;
		chipCountText.gameObject.SetActive(false);
	}
	public void StopBet()
	{
		int count = selectedChipObjList.Count;
		if(count > 0)
		{
			for(int i = 0; i < count; i++)
			{
				Destroy(selectedChipObjList[i]);
			}
		}
		chipCount = 0;
		selectedChipObjList.Clear ();
		curType = ChipType.None;
		chipCountText.gameObject.SetActive(false);
	}
	public void Clear()
	{
		int count = selectedChipObjList.Count;
		if(count > 0)
		{
			for(int i = 0; i < count; i++)
			{
				Destroy(selectedChipObjList[i]);
			}
		}
		chipCount = 0;
		betedChipCount = 0;
		betedChipCountText.gameObject.SetActive (false);
		selectedChipObjList.Clear ();
		curType = ChipType.None;
		chipCountText.gameObject.SetActive(false);
	}
	public void ContinueBet(int count)
	{
//		if(curType != BetPanel.Instance.currentBetChipType)
//		{
//			TipsManager.Instance.ShowTips("确认下注后在下注");
//			return;
//		}
//		AudioManager.Instance.PlaySound (SoundName.AddChip);
//		chipCount += (int)BetPanel.Instance.currentBetChipType;
//		GameObject obj = ResoureManager.Instance.LoadUIPrefab ("Chip");
//		obj.GetComponent<tk2dSprite> ().SetSprite (GetChipName());
//		obj.transform.position = currentChip.transform.position;
//		obj.transform.DOScale (0.5f,0.5f);
//		obj.transform.DOLocalMove (this.transform.position,0.5f).OnComplete(AnimCallBack);
//		selectedChipObjList.Add (obj);
//		BetPanel.Instance.AddToSelectedAreaList (this);
		chipCount += count;


		GameObject obj = ResoureManager.Instance.LoadUIPrefab ("Chip");
		obj.GetComponent<tk2dSprite> ().SetSprite ("chip10");
		obj.transform.position = BetPanel.Instance.chipsArray[0].position;
		obj.transform.DOScale (0.5f,0.5f);
			obj.transform.DOLocalMove (this.transform.position,0.5f).OnComplete(ContinueAnimCallBack);
		selectedChipObjList.Add (obj);

		BetPanel.Instance.AddToSelectedAreaList (this);

	}
	private Transform currentChip;
	string GetChipName()
	{
		switch (BetPanel.Instance.currentBetChipType) {
		case ChipType.Ten:
			currentChip = BetPanel.Instance.chipsArray[0];
			return "chip10";
		case ChipType.Fifty:
			currentChip = BetPanel.Instance.chipsArray[1];
			return "chip50";
		case ChipType.OneHundred:
			currentChip = BetPanel.Instance.chipsArray[2];
			return "chip100";
		case ChipType.FiveHundred:
			currentChip = BetPanel.Instance.chipsArray[3];
			return "chip500";
		default:
			currentChip = BetPanel.Instance.chipsArray[0];
			return "chip10";
		}
	}
}

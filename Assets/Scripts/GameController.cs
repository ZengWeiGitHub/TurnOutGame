using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour {

	public static GameController Instance = null;
	public tk2dUITextInput chipUITextInput,LRUITextInput;
	public Transform startGameTran,LotteryResultTran,ChipTran;
	public EasyFontTextMesh countDownText;
	public EasyFontTextMesh inputText;
	public EasyFontTextMesh chipInputtext;
	public EasyFontTextMesh winningResultText;
	private int _lotteryId;
	public int LotteryId
	{
		set
		{ 
			_lotteryId = value;
		}
		get
		{ 
			return _lotteryId;
		}
	}
	private Area _currentSelectArea;
	public Area CurrentSelectArea
	{
		set
		{
			_currentSelectArea = value;
		}
		get
		{
			return _currentSelectArea;
		}
	}
	private Area _currentGiveUpArea;
	public Area CurrentGiveUpArea
	{
		set
		{ 
			_currentGiveUpArea = value;
			if (selectAreaList.Contains (_currentGiveUpArea))
				selectAreaList.Remove (_currentGiveUpArea);
		}
	}
	public List<Area> winningAreaList = new List<Area>();
	public List<Area> selectAreaList = new List<Area> ();
	private const float countDownTime = 30;
	public Transform areaTran;
	private List<Area> areaList = new List<Area> ();
	private int areaCount;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		startGameTran.DOScale (Vector3.one * 1.2f,0.5f).SetEase (Ease.Linear).SetLoops (-1,LoopType.Yoyo).SetDelay(1);
		areaCount = areaTran.childCount;
		for(int i = 0; i < areaCount; i++)
		{
			Area area = areaTran.GetChild (i).GetComponent<Area>();
			areaList.Add (area);

			area.Id = i + 1;
		}
		LotteryResultTran.gameObject.SetActive (false);
		ChipTran.localScale = Vector3.zero;
		winningAreaList.Clear ();
		winningResultText.transform.parent.gameObject.SetActive (false);
		selectAreaList.Clear ();
	}

	public void StartGame()
	{
		Debug.Log ("Start Game");
		startGameTran.gameObject.SetActive (false);
		StartCoroutine ("IECountDown");

	}
	IEnumerator IECountDown()
	{
		countDownText.transform.parent.gameObject.SetActive (true);
		float tempTime = countDownTime;
		while(tempTime > 0)
		{
			tempTime -= Time.deltaTime;
			countDownText.text = ((int)tempTime).ToString ();
			yield return null;
		}
		countDownText.transform.parent.gameObject.SetActive (false);

		LotteryResultTran.gameObject.SetActive (true);
	}

	public void ShowChipInputDialog()
	{
		ChipTran.DOScale (Vector3.one, 0.5f).SetEase (Ease.OutBack);
	}
	public void HideChipInputDialog()
	{
		ChipTran.DOScale (Vector3.zero, 0.5f).SetEase (Ease.OutBack);
	}
	public void ChipInputDialogSure()
	{
		CurrentSelectArea.chipCount = int.Parse (chipInputtext.text);
		selectAreaList.Add (CurrentSelectArea);
		HideChipInputDialog ();
	}
	public void Reset()
	{
		startGameTran.gameObject.SetActive (true);
		startGameTran.DOScale (Vector3.one * 1.2f,0.5f).SetEase (Ease.Linear).SetLoops (-1,LoopType.Yoyo).SetDelay(1);
		for(int i = 0; i < areaCount; i++)
		{
			Area area = areaTran.GetChild (i).GetComponent<Area>();
			//area.Reset ();
			areaList.Add (area);
		
			area.Id = i + 1;
		}
		LotteryResultTran.gameObject.SetActive (false);
		ChipTran.localScale = Vector3.zero;
		winningAreaList.Clear ();
		winningResultText.transform.parent.gameObject.SetActive (false);
		selectAreaList.Clear ();
	}

	void Update()
	{
		if(Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit ();
		}
	}
}

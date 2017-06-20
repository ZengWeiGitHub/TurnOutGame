using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HistoryItem : MonoBehaviour {
	public EasyFontTextMesh termNoText;
	public EasyFontTextMesh[] lotteryNumArrayText;
	public void Init(string key, List<string> list)
	{
		if(list.Count != lotteryNumArrayText.Length)
		{
			Debug.LogError("list.Count != lotteryNumArrayText.Length");
			return;
		}
		termNoText.text = key;
		for(int i = 0; i < list.Count; i++)
		{
			lotteryNumArrayText[i].text = list[i];
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointManager : MonoBehaviour {

	public static PointManager Instance;

	public tk2dUIScrollableArea contentLenght;
	public Transform ItemParent;
	public GameObject grayObj,normalObj;
	public Dictionary<string,PointOperationItem> usersDic = new Dictionary<string, PointOperationItem>();
	public List<GameObject> pointOperationItemObjList = new List<GameObject>();

	public void Init()
	{
		Instance = this;
		//int count = GameData.Instance.TheUsers.Count;
		int count = GameData.Instance.TheUesrDic.Count;
		usersDic.Clear ();
		for(int i = 0; i < pointOperationItemObjList.Count; i++)
		{
			Destroy(pointOperationItemObjList[i]);
		}
		pointOperationItemObjList.Clear ();
		int j = 0;
		foreach(string key in GameData.Instance.TheUesrDic.Keys)
		{
			GameObject obj = ResoureManager.Instance.LoadUIPrefab("AgentItem");
			obj.transform.parent = ItemParent;
			obj.transform.localPosition = new Vector3(0,-80 * j - 50,0);
			j++;
			PointOperationItem pointOperationItem = obj.GetComponent<PointOperationItem>();
			pointOperationItem.Init(GameData.Instance.TheUesrDic[key]);
			usersDic.Add(key,pointOperationItem);
			pointOperationItemObjList.Add(obj);
		}
//		for(int i = 0; i < count; i++)
//		{
//			GameObject obj = ResoureManager.Instance.LoadUIPrefab("AgentItem");
//			obj.transform.parent = ItemParent;
//			obj.transform.localPosition = new Vector3(0,-80 * i - 50,0);
//
//			Dictionary<string,object> tempDic = GameData.Instance.TheUsers[i] as Dictionary<string,object>; 
//			PointOperationItem pointOperationItem = obj.GetComponent<PointOperationItem>();
//			pointOperationItem.Init(tempDic);
//			usersDic.Add(tempDic ["code"].ToString (),pointOperationItem);
//			pointOperationItemObjList.Add(obj);
//		}
		if (count < 5) {
			contentLenght.ContentLength = 1;
		} else {
			contentLenght.ContentLength = (count - 6) * 80;
		}
		normalObj.SetActive (GameData.Instance.IsAgent);
		grayObj.SetActive (!GameData.Instance.IsAgent);

	}

	public void UpPointOnClick()
	{
		PointOprationPanel.Instance.CurPointOpration = PointOprationPanel.PointOpration.Up;
		UIManager.Instance.ShowPanel (PanelType.PointOprationPanel);
	}
	public void DownPointOnClick()
	{
		PointOprationPanel.Instance.CurPointOpration = PointOprationPanel.PointOpration.Down;
		UIManager.Instance.ShowPanel (PanelType.PointOprationPanel);
	}
}

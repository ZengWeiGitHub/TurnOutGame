using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResoureManager : MonoBehaviour {

	public static ResoureManager Instance = null;

	void Awake()
	{
		Instance = this;
	}

	//加载Resources/UI下的预设
	public GameObject LoadUIPrefabPanel(PanelType type)
	{
		string path = "UI/" + type.ToString();
		GameObject prefab = Resources.Load<GameObject>(path) as GameObject;
		if(prefab == null)
		{
			Debug.LogWarning(type.ToString() + " load fail");
			return null;
		}
		GameObject prefabClone = Instantiate(prefab);
		prefabClone.name = type.ToString();
		return prefabClone;
	}
	//加载Resources/UI下的预设
	public GameObject LoadUIPrefab(string name)
	{
		string path = "UI/" + name;
		GameObject prefab = Resources.Load<GameObject>(path) as GameObject;
		if(prefab == null)
		{
			Debug.LogWarning(name + " load fail");
			return null;
		}
		GameObject prefabClone = Instantiate(prefab);
		prefabClone.name = name;
		return prefabClone;
	}
		
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBackgroundMask : MonoBehaviour {

	private static UIBackgroundMask _instance = null;

	private Image translucentMaskImage;
	private Transform translucentMaskImageTran;

	private Color translucentZero = new Color(0,0,0,0);
	private Color translucentOne = new Color(0,0,0,150f/255f);

	public static UIBackgroundMask Instance
	{
		get
		{
			if(_instance == null)
			{
				GameObject prefab = Resources.Load<GameObject>("UI/UIBackgroundMask") as GameObject;
				GameObject prefabClone = Instantiate(prefab);
				prefabClone.name = "UIBackgroundMask";
				_instance = prefabClone.GetComponent<UIBackgroundMask>();
			}
			return _instance;
		}

	}

	public void Init()
	{
		translucentMaskImage = transform.GetComponent<Image>();
		translucentMaskImage.color = translucentZero;
		translucentMaskImageTran = translucentMaskImage.transform;
		this.transform.SetParent(UIManager.Instance.m_CanvasList[UIManager.CanvasList.FirstCanvas]);
		this.transform.localScale = Vector3.one;
		this.transform.localPosition = Vector3.zero;
		this.transform.localRotation = Quaternion.identity;
	}

	public void Show(PanelType type)
	{
		switch(type)
		{
		case PanelType.MainInterfacePanel:
			DOTween.To(() => translucentMaskImage.color,r => translucentMaskImage.color = r, translucentZero,0.5f);
			translucentMaskImageTran.SetParent(UIManager.Instance.m_CanvasList[UIManager.CanvasList.FirstCanvas]);
			break;
		}

		translucentMaskImageTran.SetAsFirstSibling();
	}
		
	public void Hide(PanelType type)
	{
		switch(type)
		{
		
		}
	
		translucentMaskImageTran.SetAsFirstSibling();
	}
}

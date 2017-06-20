using UnityEngine;
using System.Collections;

/// <summary>
/// 事件层管理类.
/// </summary>
[RequireComponent(typeof(tk2dUICamera))]
public class EventLayerController : MonoBehaviour {

	public static EventLayerController Instance;

	public EventLayer cueEventLayer;

	private tk2dUICamera UICamera;

	private GameObject CameraObj;

	void Awake()
	{
		Instance = this;
	}

	public void Init()
	{
		if (UICamera == null)
		{
			UICamera = transform.GetComponent<tk2dUICamera> ();
		}
	}

	public void SetEventLayer(PanelType boxType)
	{
//		print (boxType);
		switch (boxType) {
		case PanelType.MainInterfacePanel:
		case PanelType.BetPanel:
			SetEventLayer(EventLayer.FirstDialog);
			break;
		case PanelType.RegisterPanel:
		case PanelType.LoginPanel:
		case PanelType.PersonalCenterPanel:
		case PanelType.QuitPanel:
			SetEventLayer(EventLayer.SecondDialog);
			break;
		case PanelType.PointOprationPanel:
			SetEventLayer(EventLayer.ThirdDialog);
			break;
		}
	}
	
	public void SetEventLayer(EventLayer layer)
	{
		cueEventLayer = layer;
		switch(layer)
		{
		case EventLayer.Nothing:
			UICamera.AssignRaycastLayerMask(0);
			break;
		case EventLayer.EveryThing:
			UICamera.AssignRaycastLayerMask(-1);
			break;
		case EventLayer.UI:
			UICamera.AssignRaycastLayerMask(1 << 5);
			break;
		case EventLayer.FirstDialog:
			UICamera.AssignRaycastLayerMask(1 << 8);
			break;
		case EventLayer.SecondDialog:
			UICamera.AssignRaycastLayerMask(1 << 9);
			break;
		case EventLayer.ThirdDialog:
			UICamera.AssignRaycastLayerMask(1 << 10);
			break;
		}
	}
}

public enum EventLayer
{
	Nothing,
	EveryThing,
	UI,
	FirstDialog,
	SecondDialog,
	ThirdDialog,
	Guide
}
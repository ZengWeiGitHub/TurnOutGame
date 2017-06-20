using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理所有UI的管理类
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
	public LoadingPage loadingPageScr;
	public GameObject contectServerObj;
    void Awake()
    {
        Instance = this;

    }
	
    void Start()
    {
		contectServerObj.SetActive (false);
        Init();
		ShowPanel(PanelType.MainInterfacePanel);
		AudioManager.Instance.PlayMusic (MusicName.UIBackground);

    }	

    //面板项枚举
	public enum CanvasList{
        FirstCanvas,
        SecondCanvas,
    }

	public Transform[] canvasList;


    public Dictionary<PanelType, PanelBase> m_PanelDict = new Dictionary<PanelType, PanelBase>(); //存放界面
    public Dictionary<CanvasList, Transform> m_CanvasList = new Dictionary<CanvasList, Transform>(); //存放层级

    //面板的栈，只用于记录返回状态才需要进栈
    private Stack<PanelType> panelStack = new Stack<PanelType>();

    public void Init()
    {
        panelStack.Clear();
        //添加面板项到m_CanvasList里面
        m_CanvasList.Add(CanvasList.FirstCanvas, canvasList[0]);
        m_CanvasList.Add(CanvasList.SecondCanvas, canvasList[1]);

		EventLayerController.Instance.Init ();

		//UIBackgroundMask.Instance.Init();
        
        //加载不同面板的预设，并加入m_PanelDict里面
        //FirstCanvas
        LoadPanel(PanelType.MainInterfacePanel, CanvasList.FirstCanvas);
		LoadPanel(PanelType.BetPanel, CanvasList.FirstCanvas);
		LoadPanel(PanelType.LoginPanel, CanvasList.SecondCanvas);
		LoadPanel(PanelType.RegisterPanel, CanvasList.SecondCanvas);
		LoadPanel(PanelType.PersonalCenterPanel, CanvasList.SecondCanvas);
		LoadPanel(PanelType.QuitPanel, CanvasList.SecondCanvas);
		LoadPanel(PanelType.PointOprationPanel, CanvasList.SecondCanvas);


    }



	private void LoadPanel(PanelType paneltype, CanvasList canvas)
	{
		GameObject obj = ResoureManager.Instance.LoadUIPrefabPanel(paneltype);
		obj.transform.SetParent(m_CanvasList[canvas]);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.rotation = Quaternion.identity;
        PanelBase panelScript = obj.GetComponent<PanelBase>();
        panelScript.InitPanel();
        m_PanelDict.Add(paneltype, panelScript);
		obj.SetActive (false);
	}


    public void ShowPanel(PanelType paneltype)
    {
        m_PanelDict[paneltype].ShowPanel();

		EventLayerController.Instance.SetEventLayer (paneltype);
       // UIBackgroundMask.Instance.Show(paneltype);

        if (IsNeedToPushStack(paneltype))
        {
            //判断当前面板是否已经进栈
            if (panelStack.Count != 0 && (panelStack.Peek() == paneltype))
                return;

            panelStack.Push(paneltype);
            
        }
    }

    public void HidePanel(PanelType paneltype)
    {
        m_PanelDict[paneltype].HidePanel();

        if (panelStack.Peek() == paneltype)
        {
            panelStack.Pop();
        }

		//UIBackgroundMask.Instance.Hide(paneltype);
		ShowPanel(panelStack.Peek());
		
    }


    /// <summary>
    /// 需要进栈的面板类型
    /// </summary>
    /// <returns><c>true</c> if this instance is need to push stack the specified paneltype; otherwise, <c>false</c>.</returns>
    /// <param name="paneltype">Paneltype.</param>
    private bool IsNeedToPushStack(PanelType paneltype)
    {
        switch(paneltype)
        {
		case PanelType.MainInterfacePanel:
		case PanelType.LoginPanel:
		case PanelType.RegisterPanel:
		case PanelType.BetPanel:
		case PanelType.PersonalCenterPanel:
		case PanelType.QuitPanel:
		case PanelType.PointOprationPanel:
			return true;
        }

        return false;
    }

    /// <summary>
    /// Android的返回键处理
    /// </summary>
    public void Back()
    {
		PanelType type = panelStack.Peek ();
		if (type == PanelType.MainInterfacePanel || type == PanelType.BetPanel) {
			ShowPanel(PanelType.QuitPanel);
		} else {
			HidePanel (panelStack.Pop ());
		}
    }

    #if UNITY_ANDROID

    void Update()
    {
        ///*Android 返回键*/
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }

        ///*Android Home键*/
        if (Input.GetKeyDown(KeyCode.Home))
        {
        }
    }

    #endif
	private EventLayer curEventLayer;
	private string curTermNo;
	void OnApplicationPause(bool pauseStatus)
	{
		if(!GameData.Instance.bLoginSuccess)
		{
			return;
		}
		if (pauseStatus) {
			GameData.Instance.isGetCurrentInfoSuccess = false;
			GameData.Instance.isGetUserInfoSuccess = false;
			Application.targetFrameRate = 0;
			GameData.Instance.StopCountDown ();
			curTermNo = GameData.Instance.TermNo;
		} else {
			Application.targetFrameRate = 30;
			EventLayerController.Instance.SetEventLayer(EventLayer.Nothing);
			contectServerObj.SetActive(true);

			CurrentInfo.Instance.Post();
			PlayerInfoPanel.Instance.Post();
			LotteryRecord.Instance.Post ();
			StartCoroutine("ResetCallBack");
		}
	}

	IEnumerator ResetCallBack()
	{
		while(!GameData.Instance.isGetCurrentInfoSuccess || !GameData.Instance.isGetUserInfoSuccess)
		{
			yield return null;
		}
		if (curTermNo != GameData.Instance.TermNo) {
			BetPanel.Instance.Clear ();
		}
//		 else {
//			GameData.Instance.StartCountDown ();
//		}
		
		BetPanel.Instance.InitLotteryRecord (GameData.Instance.LotteryRecordList);
		GameData.Instance.StartCountDown ();
		ShowPanel(panelStack.Peek());
		contectServerObj.SetActive(false);

	}
}

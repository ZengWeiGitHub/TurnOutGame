using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour {
    /// <summary>
    /// 面板是否已经显示在界面上
    /// </summary>
    /// <value><c>true</c> if this instance is active in scene; otherwise, <c>false</c>.</value>
    public bool IsActiveInScene
    {
        get
        {
            return gameObject.activeSelf && gameObject.activeInHierarchy;
        }
    }

    public virtual void InitPanel()
    {
        
    }

    public virtual void ShowPanel()
    {
        gameObject.SetActive(true);
//        UIDotweenEffect[] dotweenList = gameObject.GetComponentsInChildren<UIDotweenEffect>();
//
//        for (int i = 0; i < dotweenList.Length; i++)
//        {
//            dotweenList[i].DotweenEffectPlay();
//        }
    }

    public virtual void HidePanel()
    {
        gameObject.SetActive(false);
//        UIDotweenEffect[] dotweenList = gameObject.GetComponentsInChildren<UIDotweenEffect>();
//
//        for (int i = 0; i < dotweenList.Length; i++)
//        {
//            dotweenList[i].DotweenEffectRewind();
//        }
    }

}

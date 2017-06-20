using UnityEngine;
using System.Collections;

public class QuitPanel : PanelBase {

	public void SureOnclick()
	{
		Application.Quit ();
	}
	public void CancelOnclick()
	{
		UIManager.Instance.HidePanel (PanelType.QuitPanel);
	}
}

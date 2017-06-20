using UnityEngine;
using System.Collections;

public class MainInterfacePanel : PanelBase {

	public static MainInterfacePanel Instance;

	public override void InitPanel ()
	{
		Instance = this;
		base.InitPanel ();
	}

	public void MoveOutScree()
	{
		transform.localPosition = transform.localPosition = Vector3.up * 2050;
	}

	public void MoveInScree()
	{
		transform.localPosition = Vector3.zero;
	}

	public override void ShowPanel ()
	{
		base.ShowPanel ();
	}

	void LoginOnclick()
	{
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);
		UIManager.Instance.ShowPanel(PanelType.LoginPanel);
	}

	void RegistOnClick()
	{
		AudioManager.Instance.PlaySound (SoundName.ButtonClick);
		UIManager.Instance.ShowPanel (PanelType.RegisterPanel);
	}
}

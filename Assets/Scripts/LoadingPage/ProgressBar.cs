using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	public float width = 136;  //进度条遮罩宽度
	public float Height = 20;   //进度条遮罩长度

	public tk2dUIMask ProgressMask;

	/// <summary>
	/// 传入进度比例（0到1）设置进度条长度
	/// </summary>
	/// <param name="proportion">Proportion.</param>
	public void SetProgress(float proportion)
	{
		float temp = width * Mathf.Clamp01(proportion);

		ProgressMask.size = new Vector2(width - temp, ProgressMask.size.y);
		ProgressMask.Build ();
	}
}

using UnityEngine;
using System.Collections;

public class ProgressBarNoMask : MonoBehaviour {
	public enum ChangeType
	{
		ChangeX,
		ChangeY
	}

	public tk2dSlicedSprite changeSprite;
	public tk2dTiledSprite tiledSprite;
	public ChangeType changeType = ChangeType.ChangeX;
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;

	public void SetProgress(float value)
	{
		if (changeSprite == null && tiledSprite == null)
			return;
		float temp = Mathf.Clamp01(value);
		if (changeSprite != null) {
			if (temp == 0) {
				changeSprite.gameObject.SetActive (false);
				return;
			}

			changeSprite.gameObject.SetActive (true);

			if (changeType == ChangeType.ChangeX) {
				changeSprite.dimensions = new Vector2 (minX + (maxX - minX) * temp, changeSprite.dimensions.y);
			} else {
				changeSprite.dimensions = new Vector2 (changeSprite.dimensions.x, minY + (maxY - minY) * temp);
			}
		}

		if (tiledSprite != null) {
			if (temp == 0) {
				tiledSprite.gameObject.SetActive (false);
				return;
			}

			tiledSprite.gameObject.SetActive (true);

			if (changeType == ChangeType.ChangeX) {
				tiledSprite.dimensions = new Vector2 (minX + (maxX - minX) * temp, tiledSprite.dimensions.y);
			} else {
				tiledSprite.dimensions = new Vector2 (tiledSprite.dimensions.x, minY + (maxY - minY) * temp);
			}
		
		}
	}
}



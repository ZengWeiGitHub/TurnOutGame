using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SignEffect : MonoBehaviour {

	public tk2dSprite sprite;

	// Use this for initialization
	void Start () {
		Effect ();
	}
	
	void Effect()
	{

		DOTween.To(() => sprite.color, x => sprite.color = x, new Color(sprite.color.r, sprite.color.g, sprite.color.b,0), 0.5f).SetLoops(1000000,LoopType.Yoyo);
	}
}

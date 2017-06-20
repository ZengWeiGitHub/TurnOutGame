using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using System;

public class PlayerData : IJsonData<PlayerData> {

	public PlayerData()
	{
		InitData("PlayerData");
	}
	 
	/// <summary>
	/// 获取音效状态，false为静音
	/// </summary>
	/// <returns><c>true</c>, if sound effect on was gotten, <c>false</c> otherwise.</returns>
	public bool GetSoundState()
	{
		return bool.Parse(GetProperty("Setting","SoundOn").ToString());
	}

	/// <summary>
	/// 设置音效状态，false为静音
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void SetSoundState(bool state)
	{
		SetProperty("Setting","SoundOn", state);
	}

	/// <summary>
	/// 获取背景音乐状态，false为静音
	/// </summary>
	/// <returns><c>true</c>, if music on was gotten, <c>false</c> otherwise.</returns>
	public bool GetMusicState()
	{
		return bool.Parse(GetProperty("Setting","MusicOn").ToString());
	}

	/// <summary>
	/// 设置背景音乐状态，false为静音
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void SetMusicState(bool state)
	{
		SetProperty("Setting","MusicOn", state);
	}
	/// <summary>
	/// 获取音效音量
	/// </summary>
	/// <returns><c>true</c>, if sound effect on was gotten, <c>false</c> otherwise.</returns>
	public float GetSoundVolume()
	{
		return float.Parse(GetProperty("Setting","SoundVolume").ToString());
	}

	/// <summary>
	/// 设置音效音量
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void SetSoundVolume(float volume)
	{
		SetProperty("Setting","SoundVolume", volume);
	}

	/// <summary>
	/// 获取音乐音量
	/// </summary>
	/// <returns><c>true</c>, if music on was gotten, <c>false</c> otherwise.</returns>
	public float GetMusicVolume()
	{
		return float.Parse(GetProperty("Setting","MusicVolume").ToString());
	}

	/// <summary>
	/// 设置音乐音量
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void SetMusicVolume(float volume)
	{
		SetProperty("Setting","MusicVolume", volume);
	}

}

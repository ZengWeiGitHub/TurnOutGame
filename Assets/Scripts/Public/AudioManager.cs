using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public static GameObject AudioPool;

	// 要播放的音阶编号
	private int coinSoundNum = 0;
	private float currentTime = 0f;
	private int powerSoundNum = 0;
	private float currentPowerTime = 0f;
	// 过了这个时间没有吃到金币就重新播放音阶
	private float repeatSoundTimeGap = 0.5f;
	// 音阶的总数量,每次更换音阶的时候总数量要改！
	private int coinSoundQuantity = 14;
	private int powerSoundQuantity = 6;

	//同一时间只播放一次
	static List<string> audioOnceSameTime = new List<string>();

	private static AudioManager _instance = null;
	public static AudioManager Instance
	{
		get
		{
			if (_instance == null)
			{
				AudioPool = (GameObject)Instantiate(Resources.Load("Audio/AudioContainer"));
				AudioPool.name = "AudioContainer";
				DontDestroyOnLoad(AudioPool);
				_instance = AudioPool.AddComponent<AudioManager>();
			}
			return _instance;
		}
	}

	#region  音量
	public float MusicVolume
	{
		get
		{
			return AudioController.GetCategory("Music").Volume;
		}

		set
		{
			AudioController.GetCategory("Music").Volume = value;

			PlayerData.Instance.SetMusicVolume(value);
		}
	}

	public float SoundVolume
	{
		get
		{
			return AudioController.GetCategory("Sound").Volume;
		}

		set
		{
			AudioController.GetCategory("Sound").Volume = value;

			PlayerData.Instance.SetSoundVolume(value);
		}
	}
	#endregion

	#region 音效控制（内部方法）
	private int MaxSoundNum = 3;//同一种音效，同时播放的最大次数

	/// <summary>
	/// 根据audioId，获取此音效正在被播放的次数
	/// </summary>
	/// <returns>The playing audio number.</returns>
	/// <param name="audioId">Audio identifier.</param>
	private int GetPlayingAudioNumber(string audioId)
	{
		return AudioController.GetPlayingAudioObjectsCount(audioId, false);
	}

	/// <summary>
	/// 根据audioId播放音效
	/// </summary>
	/// <param name="audioId">Audio identifier.</param>
	public void PlaySound(string audioId,float volumn=1,float delay=0)
	{
		if (GameData.Instance.GetAudioSetting () == 0)
			return;
		if (!PlayerData.Instance.GetSoundState())
			return;
		//同一种音效，同时只能播放 MaxSoundNum 次
		if (GetPlayingAudioNumber(audioId) >= MaxSoundNum && audioId.CompareTo("GetScore") != 0 && audioId.CompareTo("Get") != 0)
		{
			return;
		}
		if (audioOnceSameTime.Contains(audioId) && GetPlayingAudioNumber(audioId) >= 1)
		{
			return;
		}

		AudioObject ao = AudioController.Play(audioId, AudioPool.transform, volumn);

	}

	/// <summary>
	/// 根据audioId停止音效
	/// </summary>
	/// <param name="audioId">Audio identifier.</param>
	private void StopSound(string audioId)
	{
		if (!PlayerData.Instance.GetSoundState())
			return;
		AudioController.Stop(audioId);
	}
	#endregion

	#region  音乐控制（内部方法）
	/// <summary>
	/// 根据audioId播放音乐
	/// </summary>
	/// <param name="audioId">Audio identifier.</param>
	private void PlayMusic(string audioId)
	{
		if (GameData.Instance.GetAudioSetting () == 0)
			return;
		if (!PlayerData.Instance.GetMusicState())
			return;

		AudioController.PlayMusic(audioId, AudioPool.transform);
	}
	#endregion
	#region  音乐、音效的暂停、停止控制（外部方法）
	/// <summary>
	/// 暂停所有音效
	/// </summary>
	public void PauseSound()
	{
		if (!PlayerData.Instance.GetSoundState())
			return;
		AudioController.PauseCategory("Sound");
	}

	/// <summary>
	/// 恢复所有音效
	/// </summary>
	public void UnPauseSound()
	{
		if (!PlayerData.Instance.GetSoundState())
			return;

		AudioController.UnpauseCategory("Sound");
	}

	/// <summary>
	/// 暂停音乐
	/// </summary>
	/// <param name="audioId">Audio identifier.</param>
	public void StopMusic()
	{
		if (!PlayerData.Instance.GetMusicState())
			return;
		AudioController.StopMusic();
	}

	/// <summary>
	/// 暂停背景音乐
	/// </summary>
	public void PauseMusic()
	{
		if (GameData.Instance.GetAudioSetting () == 0)
			return;
		if (!PlayerData.Instance.GetMusicState())
			return;
		AudioController.PauseMusic();
	}

	/// <summary>
	/// 恢复背景音乐
	/// </summary>
	public void UnPauseMusic()
	{
		if (!PlayerData.Instance.GetMusicState())
			return;
		AudioController.UnpauseMusic();
	}

	/// <summary>
	/// 停止所有音效和音乐
	/// </summary>
	public void StopAll()
	{
		AudioController.StopAll();
	}

	/// <summary>
	/// 暂停所有音效和音乐
	/// </summary>
	public void PauseAll()
	{
		AudioController.PauseAll();
	}

	/// <summary>
	/// 恢复所有音效和音乐
	/// </summary>
	public void UnPauseAll()
	{
		AudioController.UnpauseAll();
	}
	#endregion

	#region  外部引用播放方法
	/// <summary>
	/// 根据音效名字播放音效.
	/// </summary>
	/// <param name="soundName">Sound name.</param>
	public void PlayMusicByName(string musicName)
	{
		if (GameData.Instance.GetAudioSetting () == 0)
			return;
		if (string.IsNullOrEmpty (musicName))
			return;
		PlayMusic ((MusicName)System.Enum.Parse (typeof(MusicName), musicName));
	}

	/// <summary>
	/// 根据音乐名字播放音乐
	/// </summary>
	/// <param name="musicName">Music name.</param>
	public void PlayMusic(MusicName musicName)
	{
		if (GameData.Instance.GetAudioSetting () == 0)
			return;
		PlayMusic (musicName.ToString ());
	}

	/// <summary>
	/// 根据音效名字播放音效.
	/// </summary>
	/// <param name="soundName">Sound name.</param>
	public void PlaySoundByName(string soundName)
	{
		if (GameData.Instance.GetAudioSetting () == 0)
			return;
		if (string.IsNullOrEmpty (soundName))
			return;
		PlaySound ((SoundName)System.Enum.Parse (typeof(SoundName), soundName));
	}

	/// <summary>
	/// 根据音效名字停止播放音效
	/// </summary>
	/// <param name="soundName">Sound name.</param>
	public void StopSoundByName(string soundName)
	{
		if (string.IsNullOrEmpty (soundName))
			return;
		StopSound (soundName);
	}

	/// <summary>
	/// 根据音效名字播放音效
	/// </summary>
	/// <param name="soundName">Sound name.</param>
	public void PlaySound(SoundName soundName)
	{
		if (GameData.Instance.GetAudioSetting () == 0)
			return;
		switch (soundName)
		{
		default:
			PlaySound(soundName.ToString());
			break;
		}
	}



	/// <summary>
	/// 根据音效名字停止播放音效
	/// </summary>
	/// <param name="soundName">Sound name.</param>
	public void StopSound(SoundName soundName)
	{
		StopSound(soundName.ToString());
	}
	#endregion
}
#region  音效名字 SoundName

public enum SoundName
{
	/// <summary>
	/// 按钮音效
	/// </summary>
	ButtonClick,
	/// <summary>
	/// 关闭按钮
	/// </summary>
	CloseBtClick,

	BetSuccess,

	BetStart,

	BetStop,

	AddChip,

}
#endregion

#region 背景音乐名字 MusicName
public enum MusicName
{
	/// <summary>
	/// UI场景背景音乐
	/// </summary>
	UIBackground,
}
#endregion

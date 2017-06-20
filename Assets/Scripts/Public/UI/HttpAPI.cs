using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Http Request SDK.
/// </summary>
namespace RestHttpAPI
{
	public class HttpAPI : MonoBehaviour {

		private static HttpAPI _instance = null;
		private HttpAPI()
		{
			
		}
		public static HttpAPI Instance
		{
			get
			{
				if(_instance == null)
					Debug.LogError("awake error");
				return _instance;
			}
		}
		void Awake()
		{
			_instance = gameObject.GetComponent<HttpAPI>();
			jsonDic.Add("Content-Type","application/json");
			AuthDic.Add("Content-Type","application/json");
			string NameAndPw = UserName + ":" + PassWord;
			AuthDic.Add("Authorization","Basic" + System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(NameAndPw)));
		}
		public string UserName = "";
		public String PassWord = "";
		public string ipAdr = "http://14.23.52.242:28261";
		public string ver = "";
		public Dictionary<string,string> AuthDic = new Dictionary<string, string>();
		public Dictionary<string,string> jsonDic = new Dictionary<string, string>();
		/// <summary>
		/// Get the Method With callBack.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="callBack">Call back.</param>
		public void Get(string url,Action<string,string> callBack)
		{
			StartCoroutine(IEHttpGet(ipAdr + url,callBack));
		}
		IEnumerator IEHttpGet(string url,Action<string,string> callBack)
		{
			WWW www = new WWW(url);
			yield return www;
			if(www.error != null)
				Debug.LogError(www.error);
			if(callBack != null)
				callBack(www.text,www.error);
		}
		/// <summary>
		/// Post the specified url and callBack.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="callBack">Call back.</param>
		public void Post(string url,Action<string,string> callBack)
		{
			StartCoroutine(IEPost(url,callBack));
		}
		IEnumerator IEPost(string url,Action<string,string> callBack)
		{
			string fullUrl = ipAdr + url;

			WWWForm form = new WWWForm();
			form.AddField("k","v");
			WWW www = new WWW(fullUrl,form);
			yield return www;
			if(www.error != null)
				Debug.LogError(www.error);
			if(callBack != null)
				callBack(www.text,www.error);
		}
		/// <summary>
		/// Post the specified url, data and callBack.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="data">Data.</param>
		/// <param name="callBack">Call back.</param>
		public void Post(string url,string data,Action<string,string> callBack)
		{
			StartCoroutine(IEPost(url,data,callBack));
		}
		IEnumerator IEPost(string url,string data,Action<string,string> callBack)
		{
			
			string fullUrl = ipAdr + url;

			byte[] post_data;
			post_data = System.Text.UTF8Encoding.UTF8.GetBytes(data);

//			WWWForm form = new WWWForm();
//			form.AddField("data",data);
			WWW www = new WWW(fullUrl,post_data,jsonDic);
			yield return www;
			if(www.error != null)
				Debug.LogError(www.error);
			if(callBack != null)
				callBack(www.text,www.error);
		}
		public void PostAuth(string url,string data,Action<string,string> callBack)
		{
			StartCoroutine(IEPostAuth(url,data,callBack));
		}
		IEnumerator IEPostAuth(string url,string data,Action<string,string> callBack)
		{
			string fullUrl = ipAdr + url;

			byte[] post_data;
			post_data = System.Text.UTF8Encoding.UTF8.GetBytes(data);
	
			WWW www = new WWW(fullUrl,post_data,AuthDic);
			yield return www;
			if(www.error != null)
				Debug.LogError(www.error);
			if(callBack != null)
				callBack(www.text,www.error);
		}
	}

}

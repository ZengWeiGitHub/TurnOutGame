using UnityEngine;
using System.Collections.Generic;
using MiniJSON;

public class JsonTool : SingletonBase<JsonTool> {

	public Dictionary<string,string> AnalyzeJsonToDic(string text)
	{
		Dictionary<string,string> temp = MiniJSON.Json.Deserialize (text) as Dictionary<string,string>;
		return temp;
	}
	public Dictionary<string,Dictionary<string,string>> AnalyzeJsonToNestDic(string text)
	{
		Dictionary<string,Dictionary<string,string>> temp = MiniJSON.Json.Deserialize (text) as Dictionary<string,Dictionary<string,string>>;
		return temp;
	}
}

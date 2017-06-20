using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

/// <summary>
/// Json数据的读写基类
/// </summary>
public class IJsonData<T>: SingletonBase<T> where T : new()
{

    private const string path = "Public/Data/JSON/";

	#region 文件操作.
	private string DataFileName;
	
    protected bool IsCreateAnotherData = false; //是否创建另外的数据文件进行读写
    protected bool IsEncryptData = false;  //是否加密

	protected static Dictionary<string, object> miniJsonData = new Dictionary<string, object>();			//当前的json数据.
	protected static Dictionary<string, object> miniOriginalFileJsonData = new Dictionary<string, object>(); //原始的json数据，防止某个字段缺失的错误.
	
    public void InitData(string fileName, bool isCreateAnotherData = false, bool isEncryptData = false)
	{
        this.DataFileName = fileName;
        this.IsCreateAnotherData = isCreateAnotherData;
        this.IsEncryptData = isEncryptData;

        string fileContent, jsonStr, jsonOriginalStr;

        //原始的数据文件
        TextAsset textAsset = Resources.Load (path + DataFileName) as TextAsset;
        jsonOriginalStr = textAsset.text;
        miniOriginalFileJsonData = Json.Deserialize(jsonOriginalStr) as Dictionary<string, object>;
		
        //检查是否有另外的数据文件
        if (IsCreateAnotherData && FileTool.IsFileExists(DataFileName))
        {
            fileContent = FileTool.ReadFile(DataFileName);
            jsonStr = IsEncryptData ? DesCode.DecryptDES(fileContent, DesCode.PassWord) : fileContent;

            miniJsonData = Json.Deserialize(jsonStr) as Dictionary<string, object>;

        }
        else
        {
            fileContent = jsonOriginalStr;
            jsonStr = fileContent;

            miniJsonData = miniOriginalFileJsonData;

            if (IsCreateAnotherData)
            {
                FileTool.createORwriteFile(DataFileName, IsEncryptData ? DesCode.EncryptDES(fileContent, DesCode.PassWord) : fileContent);
            }
        }
	}

    // <summary>
    /// 本地数据保存
    /// </summary>
    public void SaveData()
    {
        if (!IsCreateAnotherData)
            return;
        
        string jsonStr = Json.Serialize(miniJsonData);
        FileTool.createORwriteFile(DataFileName, IsEncryptData ? DesCode.EncryptDES(jsonStr, DesCode.PassWord) : jsonStr);
    }

	#endregion
	
	#region 原子操作.
	
	protected object GetProperty(string keyName)
	{
		object temp;
		try
		{
			temp = miniJsonData[keyName];
		}
		catch
		{
			temp = miniOriginalFileJsonData[keyName];
			miniJsonData[keyName] = temp;
		}
		return temp;
	}
	
	protected object GetProperty(string keyName, string secondKeyName)
	{
		object temp;
		try
		{
			Dictionary<string, object> itemJson = miniJsonData[keyName] as Dictionary<string, object>;
			temp = itemJson[secondKeyName];
		}
		catch
		{
			Dictionary<string, object> itemJson = miniOriginalFileJsonData[keyName] as Dictionary<string, object>;
			if(!itemJson.ContainsKey(secondKeyName))
			{
				itemJson.Add(secondKeyName, "");
			}
			
			temp = itemJson[secondKeyName];
			Dictionary<string, object> itemJson2 = miniJsonData[keyName] as Dictionary<string, object>;
			itemJson2[secondKeyName] = temp;
		}
		return temp;
	}
	
	protected void SetProperty(string keyName, object value)
	{
		miniJsonData[keyName] = value;
	}
	
	protected void SetProperty(string keyName, string secondKeyName, object value)
	{
		Dictionary<string, object> itemJson = miniJsonData[keyName] as Dictionary<string, object>;
		itemJson[secondKeyName] = value;
	}
	#endregion

}

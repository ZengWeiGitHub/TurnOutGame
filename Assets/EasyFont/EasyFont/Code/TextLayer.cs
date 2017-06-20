using UnityEngine;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Text layer.  Deal 3DText
/// </summary>
#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class TextLayer : MonoBehaviour {

	public int orderInLayer=0;
	public int lineWidth=10;
	public bool ApplyNow = false;
	public bool IsRichText=false;
	TextMesh tm;

  void Awake()
   {
		tm=GetComponent<TextMesh>();
	}

#if UNITY_EDITOR
	void OnEnable()
	{
		if(!Application.isPlaying)
			ApplyWordEffect();
	}


	void Update()
	{
		if(ApplyNow)
		{
			ApplyNow = false;
			ApplyWordEffect();
		}
	}
#endif

	public void ApplyWordEffect()
	{
		if(null == tm)
			tm=GetComponent<TextMesh>();
		
		if(null!=tm)
		{
			tm.GetComponent<Renderer>().sortingOrder=orderInLayer;

			string text=tm.text.Replace("\n","");

			if(false==IsRichText)
			      tm.text=WarpWord(text,lineWidth);
			else
				tm.text=WarpRichWord(text,lineWidth);
		}


	}

	public void ApplySortingOrder()
	{
		if(null!=tm)
		{
			tm.GetComponent<Renderer>().sortingOrder=orderInLayer;
		}
	}

	/// <summary>
	/// change one line t o multi line
	/// Warps the word.  
	/// </summary>
	/// <returns>The word.</returns>
	/// <param name="originalWord">Original word.</param>
	/// <param name="lineWidth">Line width.</param>
	public static string WarpWord(string originalWord,int lineWidth){
		if (lineWidth <= 0)
			return originalWord;

		StringBuilder sb = new StringBuilder();
		
		Regex punctuationRegex = new Regex(@"[，。；？~！：‘“”’【】（）]");
		int tempNum = 0;
		char[] c = originalWord.ToCharArray();
		for (int i = 0; i < c.Length;i++){
			if (c[i] >= 0x4e00 && c[i] <= 0x9fa5){
				tempNum += 2;
				if(tempNum>lineWidth){
					i--;
					sb.Append("\n");
					tempNum = 0;
				}else{
					sb.Append(c[i]);
				}
			}else{
				if (punctuationRegex.IsMatch(c[i].ToString()))
				{
					tempNum += 2;
					if(tempNum>lineWidth){
						i--;
						sb.Append("\n");
						tempNum = 0;
					}else{
						sb.Append(c[i]);
					}
				}else{
					tempNum++;
					if(tempNum>lineWidth){
						i--;
						sb.Append("\n");
						tempNum = 0;
					}else{
						sb.Append(c[i]);
					}
				}
			}
		}
		return sb.ToString();
	}

	/// <summary>
	/// Rich Text
	/// </summary>
	/// <returns>The rich word.</returns>
	/// <param name="originalWord">Original word.</param>
	/// <param name="lineWidth">Line width.</param>
	public static string WarpRichWord(string originalWord,int lineWidth){
		if (lineWidth <= 0)
			return originalWord;
		
		StringBuilder sb = new StringBuilder();
		
		Regex punctuationRegex = new Regex(@"[，。；？~！：‘“”’【】（）]");
		int tempNum = 0;
		char[] c = originalWord.ToCharArray();
		for (int i = 0; i < c.Length;i++){
			if (c[i] >= 0x4e00 && c[i] <= 0x9fa5){
				tempNum += 2;
				if(tempNum>lineWidth){
					i--;
					sb.Append("\n");
					tempNum = 0;
				}else{
					sb.Append(c[i]);
				}
			}else{

				if(c[i]=='<')  //*忽略<> 里的内容*/
				{ 
					for(int j=i;j<c.Length;++j)
					{
						sb.Append(c[j]);
						if(c[j]=='>')
						{
							i=j;
							break;
						}
					}
				}
				else if (punctuationRegex.IsMatch(c[i].ToString()))
				{
					tempNum += 2;
					if(tempNum>lineWidth){
						i--;
						sb.Append("\n");
						tempNum = 0;
					}else{
						sb.Append(c[i]);
					}
				}else{
					tempNum++;
					if(tempNum>lineWidth){
						i--;
						sb.Append("\n");
						tempNum = 0;
					}else{
						sb.Append(c[i]);
					}
				}
			}
		}
		return sb.ToString();
	}
}

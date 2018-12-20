using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;



public abstract class MarkdownUtility : MarkdownUtilityInterface
{
    public static MarkdownUtilityInterface Default = new MarkdownUtilityDefault();
    public MarkdownUtility() {
        Default = this;
    }

    public abstract string EmtyArrayAsText(int row, int col);
    public abstract string [] FindImages(string text);
    public abstract string [] FindLinks(string text);
    public abstract string GetFilePathExtension(string path);
    public abstract string Image(string description, string url);
    public abstract bool IsImageFile(string filePath);
    public abstract bool IsMarkdownFile(string filePath);
    public abstract bool IsPathAbsolute(string path);
    public abstract bool IsPathFromDevice(string urlOrPath);
    public abstract bool IsPathFromWeb(string urlOrPath);
    public abstract bool IsPathRelative(string path);
    public abstract string Italic(string text);
    public abstract string LineBreak();
    public abstract string LineReturn(string text);
    public abstract string LineTitleMain(string text);
    public abstract string LineTitleSecond(string text);
    public abstract string Link(string description, string url);
    public abstract string LinkedImage(string description, string urlImage, string urlRedirection);
    public abstract string MultiLineCode(string text, string language = "cs");
    public abstract string OneLineCode(string text);
    public abstract string Paragraph(string text);
    public abstract string Quote(string text);
    public abstract string SimpleCheckList(string[] elements, bool[] checkValue);
    public abstract string SimpleEnumeration(string[] elements);
    public abstract string SimpleList(string[] elements);
    public abstract string Strong(string text);
    public abstract string Subsection(string text);
    public abstract string Title(string text, int level);
    public abstract string Strikethrough(string text);
    public abstract bool IsPathFromRelativeFolder(string path, string folderName);
    public abstract bool IsPathDataUrl(string path);
    public abstract string GetDescriptionOfImage(string mdImageText);
    public abstract string GetLinkOfImage(string mdImageLink);
    public abstract string[] FindMarkdownLinks(string text);
  
    public abstract string[] GetMarkdownFilesInDirectory(string directoryPath, bool inChildrenToo);
    public abstract string GetYoutubeImageLink(string youtubeLink, YoutubeImageType youtubeImageType = YoutubeImageType._maxresdefault);
    public abstract string YoutubeImageAsText(string description, string youtubeLink, YoutubeImageType youtubeImageType, bool withLinkBelow);
    public abstract string LineBreak(string text);
    public abstract string Title(string text);
    public abstract string SimpleList(string text);
    public abstract string SimpleEnumeration(string text);
    public abstract string SimpleCheckList(string text);
    public abstract string GetDataUrlContent(string path, out byte[] bytes);
    public abstract IEnumerator IsLinkBroken(string url, BoolCallBack result);
    public abstract bool IsFilePathEmpty(string path);
}


public class MarkdownUtilityDefault : MarkdownUtility
{
    public override string EmtyArrayAsText(int row, int col)
    {
        string result = "";
        if (col < 1)
            col = 1;
        if (row < 1)
            row = 1;

        for (int i = 0; i < col; i++)
        {
            result += "T" + i + " | ";
        }
        result = result.Substring(0, result.Length - 3);
        result += "\n";

        for (int i = 0; i < col; i++)
        {
            if (i == 0) result += ":-";
            else if (i == col - 1) result += "-:";
            else result += ":-:";
            result += " | ";

        }
        result = result.Substring(0, result.Length - 3);
        result += "\n";


        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                result += " " + r + "" + c + " | ";
            }
            result = result.Substring(0, result.Length - 3);
            result += "\n";
        }



        return result;
    }
    public override bool IsMarkdownFile(string filePath)
    {
        string fileExtension = GetFilePathExtension(filePath);
        return fileExtension.ToLower() == "md";

    }
    public override string Image(string description, string url)
    {
        description= description.Trim();
        url =  url.Trim();
        if (string.IsNullOrEmpty(description)) description = " ";
        if (string.IsNullOrEmpty(url)) url = " ";
        return string.Format("![{0}]({1})",description, url);

    }
    public override bool IsImageFile(string filePath)
    {
        string fileExtension = GetFilePathExtension(filePath).ToLower();
        if (fileExtension == "jpeg") return true;
        if (fileExtension == "jpg") return true;
        if (fileExtension == "gif") return true;
        if (fileExtension == "png") return true;
        return false;
    }
    public override string GetDescriptionOfImage(string mdImageText)
    {
        if (string.IsNullOrEmpty(mdImageText)) return "";
        Match match = Regex.Match(mdImageText, m_imagelabelRegex);
        if (string.IsNullOrEmpty(mdImageText)) return "";
        return match.Value.Substring(2, match.Value.Length - 4);
    }
    public override string GetLinkOfImage(string mdImageLink)
    {
        if (string.IsNullOrEmpty(mdImageLink)) return "";
        Match match = Regex.Match(mdImageLink, m_imageLinkRegex);
        if (match == null || match.Value == null)
            return "";
        return match.Value.Substring(2, match.Value.Length - 3);
    }
    public override string [] FindImages(string text)
    {
        MatchCollection collection = Regex.Matches(text, m_imageMarkDownRegex);
        List<string> result = new List<string>();
        foreach (Match match in collection)
        {
            result .Add(match.Value);
        }
        return result.ToArray();
    }
    public override string GetFilePathExtension(string path)
    {
        path.Trim();
        if (IsPathDataUrl(path))
        {
            if (path.IndexOf("data:image/png") == 0) return "png";
            if (path.IndexOf("data:image/jpg") == 0) return "jpg";
            if (path.IndexOf("data:image/jpeg") == 0) return "jpeg";
            return "";
        }
        else if (IsPathFromWeb(path))
        {
            if (path.IndexOf('?') > -1)
                path = path.Split('?')[0];
            return GetExtAndRemoveDot(path); 

        }
        else
        {
            return GetExtAndRemoveDot(path);

        }
    }

    private static string GetExtAndRemoveDot(string path)
    {
        string ext = Path.GetExtension(path);
        if (!string.IsNullOrEmpty(ext)) 
            ext = ext.Replace('.', ' ').Trim() ;
        else ext = "";
        return ext;
    }

    public override bool IsPathAbsolute(string path)
    {
        if (IsPathFromWeb(path))
            return true;
#if UNITY_STANDALONE_WIN
        try
        {
            return Path.GetFullPath(path) == path;
        }
        catch
        {
            return false;
        }
#endif
    }
    public override bool IsPathFromDevice(string urlOrPath)
    {
        if (IsPathFromWeb(urlOrPath)) return false;
        if (IsPathDataUrl(urlOrPath)) return false;
        return true;
    }
    public override bool IsPathFromWeb(string urlOrPath)
    {
        return urlOrPath.ToLower().IndexOf("http://") == 0 
            || urlOrPath.ToLower().IndexOf("https://") == 0;
    }
    public override bool IsPathRelative(string path)
    {
        return !IsPathAbsolute(path);
    }
    public override string[] GetMarkdownFilesInDirectory(string directoryPath, bool inChildrenToo)
    {
        if (!Directory.Exists(directoryPath))
        {
            return new string [0];
        }
        return Directory.GetFiles(@directoryPath, "*.md",inChildrenToo? SearchOption.AllDirectories: SearchOption.TopDirectoryOnly);

    }
    public override string GetYoutubeImageLink(string youtubeLink, YoutubeImageType youtubeImageType = YoutubeImageType._maxresdefault)
    {
        string urlFormat = "https://img.youtube.com/vi/{0}/{1}.jpg";
        string imageType = youtubeImageType.ToString();
        imageType=  imageType.Substring(1, imageType.Length-1);
        return string.Format(urlFormat, GetYoutubeId(youtubeLink), youtubeImageType);
    }
    public static string GetYoutubeId(string url)
    {
        Regex regexExtractId = new Regex(m_youtubeLinkRegex, RegexOptions.Compiled);
        var regRes = regexExtractId.Match(url);
        if (regRes.Success)
        {
            return regRes.Groups[1].Value;
        }
        return "";
    }
    public override string YoutubeImageAsText(string description, string youtubeLink, YoutubeImageType youtubeImageType, bool withLinkBelow)
    {
        return string.Format("[![{0}]({2})]({1})  \n",
            description,
            youtubeLink,
            GetYoutubeImageLink(youtubeLink, youtubeImageType)
            )+(withLinkBelow? youtubeLink:"");
    }





    public override string Italic(string text) {
        return text = "*" + text.Trim() + "*";
    }
    public override string LineBreak()
    {
        return "  \n  \n---------  \n  \n";
    }
    public override string LineReturn(string text) {
        return text = "" + text + "  \n";
    }
    public override string Link(string description, string url)
    {
        return string.Format("[{0}]({1})",TrimButNotEmpty(description), TrimButNotEmpty(url));
    }
    public static   string TrimButNotEmpty(string text)
    {
        if (text == null)
            return " ";
        text= text.Trim();
        if (text == "")
            text = " ";
        return text;
    }
    public override string LinkedImage(string description, string urlImage, string urlRedirection)
    {
       return  Link( Image(description, urlImage), urlRedirection);
    }
    public override string MultiLineCode(string text, string language ="cs")
    {
        return text = "  \n``` "+language+"  \n" +text.Trim() + "  \n```  \n";
    }
    public override string OneLineCode(string text)
    {
       return text = "`" + text.Trim() + "`";
    }
    public override string Quote(string text)
    {
        return text = "> " +text + " \n  \n";
    }
    public override string Strong(string text)
    {
       return text = "**" + text.Trim() + "**";
    }
    public override string Strikethrough(string text) {
       return  text = "~~" +text.Trim() + "~~";
     }
    public override string Title(string text)
    {
        string t = "#";
        if (text.Substring(0, 1) != "#")
            t = "# ";
        text = t + text + "  \n";
        return text;
    }
    public override string Title(string text, int level)
    {
       text = text.TrimStart(new char[] { '#', ' ' });
        for (int i = 0; i < level; i++)
        {
            Title(text);
        }
        return text;
      
    }
    public override string LineBreak(string text)
    {
        return text + LineBreak() ;
    }
    public override string LineTitleMain(string text)
    {
        return text + "  \n=====  \n  \n";
    }
    public override string LineTitleSecond(string text)
    {
        return text + "  \n-----  \n  \n";
    }
    public override string Paragraph(string text)
    {
        return text + "  \n  \n";
    }
    public override string Subsection(string text)
    {
        string newText="";
        string[] tokkens = text.Split('\n');
        for (int i = 0; i < tokkens.Length; i++)
        {
            newText = "    " + tokkens[i];
        }
        return newText;
    }

    public override string SimpleList(string text)
    {
        string[] lineTokkens = text.Split('\n');
        return SimpleList(lineTokkens);
    }
    public override string SimpleEnumeration(string text)
    {
        string[] lineTokkens = text.Split('\n');
        return SimpleEnumeration(lineTokkens);
    }
    public override string SimpleCheckList(string text)
    {
        string[] lineTokkens = text.Split('\n');
        return SimpleCheckList(lineTokkens, new bool[lineTokkens.Length] );
    }
    public override string SimpleCheckList(string[] elements, bool[] checkValue)
    {
        if (checkValue.Length != elements.Length) {
            Debug.LogWarning("Should not be different size !"); 
        }

        string text = "";
        for (int i = 0; i < elements.Length; i++)
        {
            text += "- ["+(checkValue[i]?"x":" ") +"] " + elements[i].Trim() + "  \n";
        }
        return text;
    }
    public override string SimpleEnumeration(string[] elements)
    {
        string text = "";
        for (int i = 0; i < elements.Length; i++)
        {
            text += i+". " + elements[i].Trim() + "  \n";
        }
        return text;
    }
    public override string SimpleList(string[] elements)
    {
        string text = "";
        for (int i = 0; i < elements.Length; i++)
        {
            text += "- " + elements[i].Trim() + "  \n";
        }
        return text;
    }














    public override bool IsPathFromRelativeFolder(string path, string folderName)
    {
        return path.ToLower().IndexOf(folderName+ "/") == 0;
    }

    public override bool IsPathDataUrl(string path)
    {

        byte[] b=null;
        return !string.IsNullOrEmpty(GetDataUrlContent(path,out b));

    }
    public override string GetDataUrlContent(string path, out byte[] bytes) {
        string data = Regex.Match(path, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
        bytes = Convert.FromBase64String(data);
        return data;
    }
    
    public override string [] FindLinks(string text)
    {
        throw new System.NotImplementedException();
    }

    public override string[] FindMarkdownLinks(string text)
    {
        throw new System.NotImplementedException();
    }

   

    public override bool IsFilePathEmpty(string path)
    {
        return File.Exists(path) || Directory.Exists(path);
    }
    


    public override IEnumerator IsLinkBroken(string url, BoolCallBack result)
    {
        WWW www = new WWW(url);
        yield return www;
        result = new BoolCallBack();
        result.m_value = !string.IsNullOrEmpty(www.error);
        yield break;
    }

    public class CallbackResult <T>{
        public T m_value;
    }




    //https://stackoverflow.com/questions/39777659/extract-the-video-id-from-youtube-url-in-net
    public static string m_youtubeLinkRegex = "(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+";
    // Source: https://stackoverflow.com/questions/36391979/find-markdown-image-syntax-in-string-in-java
    public static string m_imageMarkDownRegex = "!\\[[^\\]]*?\\]\\([^)]+\\)";
    public static string m_imageLinkRegex = "\\]\\([^)]+\\)";
    public static string m_imagelabelRegex = "!\\[[^\\]]*?\\]\\(";
}



public interface MarkdownUtilityInterface
{
    #region SIMPLE

    string Italic(string text);// *Italic Text* || _Italic Text_
    string Strong(string text); // **Strong text**
    string Strikethrough(string text);// __Under Line the text__
    string OneLineCode(string text);//``Line of code`
    string MultiLineCode(string text, string language="cs"); //``` cs\n  Multi line of code```
    string Subsection(string text); //    Four Space    
    string Paragraph(string text);//Space between text
    string Quote(string text);// > Quote
    string Title(string text);// ## Title
    string Title(string text, int level);// ## Title
    string LineTitleMain(string text); // Title ==============
    string LineTitleSecond(string text); // Title -----------
    #endregion

    #region EASY
    string Image(string description, string url); //![alt](url)
    string GetDescriptionOfImage(string mdImageText);
    string GetLinkOfImage(string mdImageLink);

    string LinkedImage(string description, string urlImage, string urlRedirection); //[![alt](imageUrl)](Redirection)
    string Link(string description, string url); //[link](url)

    string LineBreak();//----------
    string LineBreak(string text);//----------
    string LineReturn(string text);// Return with two space at end  


    string SimpleList(string text);
    string SimpleEnumeration(string text);
    string SimpleCheckList(string text);

    string SimpleList(string[] elements);
    string SimpleEnumeration(string[] elements);
    string SimpleCheckList(string[] elements, bool[] checkValue);

    #endregion
    #region COMPLEX
    string EmtyArrayAsText(int row, int col); // See documentation
    string YoutubeImageAsText(string description, string youtubeLink, YoutubeImageType youtubeImageType,bool withLinkBelow);
    string GetYoutubeImageLink(string youtubeLink, YoutubeImageType youtubeImageType = YoutubeImageType._maxresdefault);
  

    #endregion


    #region FIND
    string[] FindImages(string text);
    string[] FindLinks(string text);
    string[] FindMarkdownLinks(string text);
    //...
    #endregion

    #region COMPLEMENTARY
    bool IsPathFromWeb(string urlOrPath);
    bool IsPathFromDevice(string urlOrPath);
    bool IsPathFromRelativeFolder(string path, string folderName);
    bool IsPathRelative(string path);
    bool IsPathAbsolute(string path);
    bool IsPathDataUrl(string path);
    string GetFilePathExtension(string path);

  


    bool IsImageFile(string filePath);
    bool IsMarkdownFile(string filePath);
    
    string GetDataUrlContent(string path, out byte[] bytes);

    string[] GetMarkdownFilesInDirectory(string directoryPath, bool inChildrenToo);

    IEnumerator IsLinkBroken(string url, BoolCallBack result);
    bool IsFilePathEmpty(string path);


   


    #endregion

}
public class BoolCallBack
{
    public bool m_value;
}


public enum YoutubeImageType : int
{
    _0 = 0,
    _1 = 1,
    _2 = 2,
    _3 = 3,
    _hqdefault = 4,
    _mqdefault = 5,
    _maxresdefault = 6
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface WikiGitUtilityInterface {

    void IsFolderGitRoot(string gitRootPath);
    List<string> GetNameListOfMarkdownFiles(string wikiRootPath);
    List<string> GetPathListOfMarkdownFiles(string wikiRootPath);
    List<string> GetNameListOfImageFiles(string wikiRootPath);
    List<string> GetPathListOfImageFiles(string wikiRootPath);

    List<string> GetLinksCodeInFollowingText(string text);
    List<string> GetImagesCodeInFollowingText(string text);

    List<string> GetTitlesCodeInFollowingText(string text);
    List<string> GetTitlesCodeInFollowingText(string text,int level);

    void SaveImage(Texture2D image, string name, string extention);
    Texture2D LoadImage(string name);

    void SaveMarkdownFile(string name, string text);
    string LoadMarkdownFile(string name);


    void DownloadImageFromWeb(string url, string newFileName);
    void DownloadImageFromDevice(string path, string newFileName);

}

public interface WikiGitCurrentProjectUtilityInterface {

    bool IsWikiRootPathDefine();
    bool IsWikiImagePathDefine();
    bool IsGitRootPathDefine();

    string GetCurrentWikiRootPath();
    string GetCurrentGitRootPath();
    string GetCurrentWikiImageRootPath();

    void OpenCurrentWikiRootDirectory();
    void OpenCurrentGitRootDirectory();
    void OpenCurrentWikiImageRootDirectory();
}


public interface MarkdownUtilityInterface {
    #region SIMPLE

    string Italic(string text);// *Italic Text* || _Italic Text_
    string Strong(string text); // **Strong text**
    string Underline(string text);// __Under Line the text__
    string OneLineCode(string text);//``Line of code`
    string MultiLineCode(string text); //``` Multi line of code```
    string Subsection(string text); //    Four Space    
    string Paragraph(string text);//Space between text
    string Quote(string text);// > Quote
    string Title(string text, int level);// ## Title
    string LineTitleMain(string text); // Title ==============
    string LineTitleSecond(string text); // Title -----------
    #endregion

    #region EASY
    string Image(string description, string url); //![alt](url)
    string Link(string description, string url); //[link](url)

    string LineBreak();//----------
    string LineReturn(string text);// Return with two space at end  

    string SimpleList(string[] elements);
    string SimpleEnumeration(string[] elements);
    string SimpleCheckList(string[] elements, bool [] checkValue);

    #endregion
    #region COMPLEX
    string EmtyArray(int row, int col); // See documentation

    #endregion


    #region FIND
    string FindImages(string text);
    string FindLinks(string text);
    //...
    #endregion

    #region COMPLEMENTARY
    bool IsPathFromWeb(string urlOrPath);
    bool IsPathFromDevice(string urlOrPath);
    bool IsPathRelative(string path);
    bool IsPathAbsolute(string path);
    string GetFilePathExtension(string path);

    bool IsImageFile(string filePath);
    bool IsMarkdownFile(string filePath);

    #endregion
}

public class WikiGitUtilityMono : MonoBehaviour
{

}

//public class WikiGitUtility : WikiGitUtilityInterface {


//}

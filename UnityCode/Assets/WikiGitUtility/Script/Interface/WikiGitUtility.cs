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



public class WikiGitUtilityMono : MonoBehaviour
{

}

//public class WikiGitUtility : WikiGitUtilityInterface {


//}

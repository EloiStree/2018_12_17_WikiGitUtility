using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadAllImagesInProjectLocaly : MonoBehaviour
{
    [SerializeField]
    string m_gitProjetPath;
    [SerializeField] FindMarkdownFiles m_filesFinder;
    [SerializeField] FindImagesInMarkdownText m_imagesFinder;
    [SerializeField]
    string m_backupFolderName="WebImageBackup";



    public bool m_loadAllFilesOnWikiAtStart;
    public void SetProjectPath(string path)
    {
        m_gitProjetPath = path;
    }

    private static LoadAllImagesInProjectLocaly m_instance;

    IEnumerator Start()
    {
        m_instance=this; 
        if(m_loadAllFilesOnWikiAtStart)
        yield return LoadNonLocalImagesAsBackup();

    }

    private IEnumerator CheckForMissingLocalImage() {
        throw new System.NotImplementedException("");
        //Check and return a list of MardownImage that should be on the /Image/ folder but are not.
    }


    public static string random = "0123456789abcdeflghijklmnopkrstuvxyz";
    public static string RandomName()
    {
        string randomName = "";
        for (int i = 0; i < 32; i++)
        {
            randomName += random[UnityEngine.Random.Range(0, random.Length)];
        }
        return randomName;
    }

    internal List<MarkdownImageInFile> LoadAllWebImagesReference(string gitProjectPath)
    {
        List<MarkdownImageInFile> result = new List<MarkdownImageInFile>();
        m_gitProjetPath = gitProjectPath;
        m_filesFinder.SetProjectPath(m_gitProjetPath);
        List<MarkdownFileWithText> files = m_filesFinder.CheckForMarkdownFilesInProject();
  //      Debug.Log("> Go in each files:" + files.Count);
        int i = 0;
        foreach (var file in files)
        {
            file.Load();
//            Debug.Log(">> Go in each Image:" + files.Count);
            if (file.TextLenght > 0)
            {
                List<MarkdownImageAsText> images = m_imagesFinder.LookForImageInMarkdownText(file.GetValue());
                foreach (var image in images)
                {
                    result.Add(new MarkdownImageInFile(file, image));
                }

            }
            file.Unload();

        }
        return result;
    }

    public void LaunchLoadNonLocalImageAsBackup() {
        StartCoroutine(LoadNonLocalImagesAsBackup());
    }
    private IEnumerator LoadNonLocalImagesAsBackup()
    {
        m_filesFinder.SetProjectPath(m_gitProjetPath);
        List<MarkdownFileWithText> files = m_filesFinder.CheckForMarkdownFilesInProject();
        Debug.Log("> Go in each files:" + files.Count);
        int i = 0;
        foreach (var file in files)
        {
            file.Load();
            Debug.Log(">> Go in each Image:" + files.Count);
            if (file.TextLenght > 0)
            {
                List<MarkdownImageAsText> images = m_imagesFinder.LookForImageInMarkdownText(file.GetValue());
                foreach (var image in images)
                {
                    yield return StartDownloadingImage(image,""+i,true);
                    i++;
                }
            }
            file.Unload();
        }
    }


    public static IEnumerator StartDownloadingImage(MarkdownImageAsText image, string name, bool asBackup=false)
    {
        //if (image.IsWebLink())
        {
            Debug.Log(">>>" + name + ": Download: " + image.GetImageLink().Trim());
            string path = image.GetImageLink();
            if (image.IsImagesFolder())
                path = "file:///"+m_instance.m_gitProjetPath + "/" + path;
            Debug.Log("lll:" + path);


            byte[] b;
            string data = MarkdownUtility.Default.GetDataUrlContent(path, out b);
            string ext = MarkdownUtility.Default.GetFilePathExtension(path);
            if (!string.IsNullOrEmpty(data))
            {
                path = Application.temporaryCachePath + "/data." + ext;
                Debug.Log("U:" + Application.temporaryCachePath);
                File.WriteAllBytes(path, b);
            }




            WWW download = new WWW(path);
            yield return download;
            if (download != null || string.IsNullOrEmpty(download.error))
            {
                Debug.Log(">>>> Download fail: " + download.error);

            }

            {
                string pathToStore = 
                    asBackup ? m_instance.m_gitProjetPath + "/" + m_instance.m_backupFolderName + "/":
                     m_instance.m_gitProjetPath + "/Image/";
                if (!Directory.Exists(pathToStore))
                    Directory.CreateDirectory(pathToStore);
                string foundExention = "";
                foundExention = image.GetFileExtension();
                if (foundExention != "")
                {

                    File.WriteAllBytes(pathToStore + name + "." + foundExention, download.bytes);
                }
                else
                {
                    File.WriteAllBytes(pathToStore + name + ".png", download.texture.EncodeToPNG());
                }

                Debug.Log(">>>> Downloaded: " + image.GetImageLink());
            }
        }

    }
    public void OpenProjectImageDirectory()
    {
        Application.OpenURL(m_gitProjetPath + "/Image/");
    }
    public void OpenProjectDirectory()
    {
        Application.OpenURL(m_gitProjetPath );
    }

    private string CheckForExtension(string link)
    {
       link = link.ToLower();
        if (link.IndexOf(".jpeg") >= 0)
            return "jpeg";
        if (link.IndexOf(".jpg") >= 0)
            return "jpg";
        if (link.IndexOf(".gif") >= 0)
            return "gif";



        return "";
    }

    private void OnDestroy()
    {
      //  PlayerPrefs.SetString("LoadAllImagesInProjectLocaly", m_gitProjetPath);
    }

    private void OnValidate()
    {
       // PlayerPrefs.SetString("LoadAllImagesInProjectLocaly", m_gitProjetPath);
    }
}

[System.Serializable]
public class MarkdownImageInFileDetectedEvent : UnityEvent<MarkdownImageInFile> { }

[System.Serializable]
public class MarkdownImageInFile
{
    public readonly MarkdownFileWithText markdownFile;
    public readonly MarkdownImageAsText markdownImage;

    public MarkdownImageInFile(MarkdownFileWithText file, MarkdownImageAsText image)
    {
        this.markdownFile = file;
        this.markdownImage = image;
    }

    internal string GetImagePath()
    {
        if (markdownImage.IsWebLink())
            return markdownImage.GetImageLink();
        else
            return "file:///" + markdownFile.GetProjectPath() + "/" + markdownImage.GetImageLink();
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UI_MarkDownImageEdit : MonoBehaviour
{
    public Text m_label;
    public Text m_urlPath;
    public Text m_filePath;
    public Text m_extension;
    public InputField m_imageNewName;
    public AspectRatioFitter m_ratioFitter;
    public RawImage m_image;
    public MarkdownImageInFile m_givenData;
    public GameObject m_lockPanel;
    internal void SetWith(MarkdownImageInFile imageToFile)
    {
        m_givenData = imageToFile;
        m_label.text = imageToFile.markdownImage.GetImageLabel();
        m_urlPath.text = imageToFile.markdownImage.GetImageLink();
        m_filePath.text = imageToFile.markdownFile.GetPath();
        m_imageNewName.text = GetPropositionOfFileName();
        m_extension.text = m_givenData.markdownImage.GetFileExtension();
        StartCoroutine(LoadImage());

        m_imageNewName.onEndEdit.AddListener(CheckFieldValidity);
        Unlock();
    }

    private void CheckFieldValidity(string arg0)
    {
        m_imageNewName.text = CheckForNameValidity(m_imageNewName.text);
    }

    private string CheckForNameValidity(string text)
    {
        text = text.Substring(0, text.Length>32 ? 32 : text.Length);
        Regex rgx = new Regex("[^a-zA-Z0-9]");
        text = rgx.Replace(text, "");
        text = text.Replace(' ', '_');
        return text;
    }

    public void OpenImageUrl()
    {
            Application.OpenURL(m_givenData.GetImagePath());
        
    }
    public void OpenMarkdownFile()
    {
        Application.OpenURL("file:///"+ m_givenData.markdownFile.GetPath());
        //Application.OpenURL("file:///C:/Users/stree/Desktop/Wiki/2018_03_01_ToDoToday.md");

    }

    private IEnumerator LoadImage()
    {
        
            string path = m_givenData.markdownImage.IsWebLink() ?
                m_givenData.markdownImage.GetImageLink() :
                "file://" + m_givenData.markdownFile.GetProjectPath() + "/" + m_givenData.markdownImage.GetImageLink()
    ;
            path = path.Replace('\\', '/');
            WWW image = new WWW(path);
            Debug.Log("Path Image:" + image.url + " <> " + path);
            yield return image;
            m_image.texture = image.texture;
            yield return new WaitForSeconds(0.1f);
            if (m_image.texture != null)
                m_ratioFitter.aspectRatio = ((float)m_image.texture.width) / ((float)m_image.texture.height);
        
    }

    private string GetPropositionOfFileName()
    {
        string str = m_givenData.markdownImage.m_label;
        return CheckForNameValidity(str);
    }
    private string GetNewNameWithExtention()
    {
        return m_imageNewName.text + "."
                + m_givenData.markdownImage.GetFileExtension();
    }
    private string GetNewNameWithoutExtention()
    {
        return m_imageNewName.text;
    }

    public void DownloadLocalyTheImage() {
        StartCoroutine( LoadAllImagesInProjectLocaly.StartDownloadingImage(m_givenData.markdownImage, m_imageNewName.text));
    }
    public void FindAndReplace() {
        string newLocalUrl= "Image/" + GetNewNameWithExtention();

        if (m_givenData.markdownImage.GetFileExtension() == "")
            newLocalUrl = "Image/" + GetNewNameWithoutExtention()+".png";
        FindAndReplaceImageGlobalToLocal.FindReplace(m_givenData, newLocalUrl); 
    }

    public void AutoDestroy(float time =0)
    {
        Destroy(this.gameObject,time);
    }

    public static string random = "0123456789abcdeflghijklmnopkrstuvxyz";
    public void RandomName() {
        string randomName = "";
        for (int i = 0; i < 32; i++)
        {
            randomName += random[UnityEngine.Random.Range(0, random.Length)];
        }
        m_imageNewName.text = randomName;
    }
    public void DoTheThing() {

        Lock();
        RandomName();
        DownloadLocalyTheImage();
        FindAndReplace();
        AutoDestroy(3);

    }

    public void Lock()
    {
        m_lockPanel.SetActive(true);
    }
    public void Unlock()
    {
        m_lockPanel.SetActive(false);
    }

}

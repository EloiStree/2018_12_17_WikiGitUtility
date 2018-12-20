using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using System;

public class FindImagesInMarkdownText : MonoBehaviour
{

    [SerializeField]
    [TextArea(0,10)]
    string m_textInFile;
   

    [SerializeField]
    MarkdownImageDetectedEvent  m_imageDetected;

    [Header("Debug")]
    [SerializeField]
    bool m_useTextInInspectorAtstart;
    [SerializeField]
    List<string> m_imagesAsText;
    [SerializeField]
    List<MarkdownImageAsText> m_imagesMarkdown;


    void Start()
    {
        if(m_useTextInInspectorAtstart)
            CheckForMarkDownImage(m_textInFile);
    }

    private void CheckForMarkDownImage(string text)
    {
        m_imagesMarkdown = new List<MarkdownImageAsText>();
        m_imagesAsText = new List<string>();

        string[] images = MarkdownUtility.Default.FindImages(text);
        foreach (string image in images)
        {
            m_imagesAsText.Add(image);
            try
            {
                MarkdownImageAsText md = new MarkdownImageAsText(image);
                m_imagesMarkdown.Add(md);
                m_imageDetected.Invoke(md);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Impossible to convert as image:" + image);
            }
        }
    }

    public List<MarkdownImageAsText> LookForImageInMarkdownText(string text) {
        m_textInFile = text;
        CheckForMarkDownImage(text);
        return m_imagesMarkdown;
    }
}

[System.Serializable]
public class MarkdownImageDetectedEvent : UnityEvent<MarkdownImageAsText> { }

[System.Serializable]
public class MarkdownImageAsText {
    public string m_text;
    public string m_label;
    public string m_link;

    public MarkdownImageAsText(string text) {
        m_text = text;
        m_label = GetImageLabel();
        m_link = GetImageLink();
    }

    internal MarkdownImageAsText Copy()
    {
        return new MarkdownImageAsText(m_text);
    }

    public string GetImageLink() {
        return MarkdownUtility.Default.GetLinkOfImage(m_text);
    
    }
    public string GetImageLabel() {

        return MarkdownUtility.Default.GetDescriptionOfImage(m_text);

    }
    public bool IsImagesFolder() {
        return MarkdownUtility.Default.IsPathFromRelativeFolder(GetImageLink(), "image");
        
    }
    public bool IsWebLink() {
        return MarkdownUtility.Default.IsPathFromWeb(GetImageLink());
  }

    internal string GetFileExtension()
    {
        return MarkdownUtility.Default.GetFilePathExtension(GetImageLink());
        
    }

    internal void SetUrlTo(string newUrl)
    {
        m_text = MarkdownUtility.Default.Image(GetImageLabel(), newUrl);   }
    internal void SetLabelTo(string newLabel)
    {
        m_text = MarkdownUtility.Default.Image(newLabel,GetImageLink());
    }

    internal string GetText()
    {
        return m_text;
    }

    internal static MarkdownImageAsText CreateMarkdownImage(string alt, string url)
    {
        string md = MarkdownUtility.Default.Image(alt, url);
        return new MarkdownImageAsText(md);
    }
}

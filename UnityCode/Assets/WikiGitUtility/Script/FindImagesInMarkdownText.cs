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
    // Source: https://stackoverflow.com/questions/36391979/find-markdown-image-syntax-in-string-in-java
    //string m_imageMarkDownFormat = "!\\[[^\\]]+\\]\\([^)]+\\)";
    string m_imageMarkDownFormat = "!\\[[^\\]]*?\\]\\([^)]+\\)";
    

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

        MatchCollection collection = Regex.Matches(m_textInFile, m_imageMarkDownFormat);
        foreach (Match match in collection)
        {
            m_imagesAsText.Add(match.Value);
            try
            {
                MarkdownImageAsText md = new MarkdownImageAsText(match.Value);
                m_imagesMarkdown.Add(md);
                m_imageDetected.Invoke(md);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Impossible to convert as image:" + match.Value);
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
    public static string m_linkPattern = "\\]\\([^)]+\\)";
    public static string m_labelPattern = "!\\[[^\\]]*?\\]\\(";
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
     Match match =  Regex.Match(m_text, m_linkPattern);
        return match.Value.Substring(2, match.Value.Length - 3);
    }
    public string GetImageLabel() {
        Match match = Regex.Match(m_text, m_labelPattern);
        return match.Value.Substring(2, match.Value.Length-4) ;

    }
    public bool IsImagesFolder() {
        string link = GetImageLink();
        return link.ToLower().IndexOf("image/")==0 ;
    }
    public bool IsWebLink() {
        string link = GetImageLink();
        return link.ToLower().IndexOf("http://") ==0 || link.ToLower().IndexOf("https://")==0;
    }

    internal string GetFileExtension()
    {
            string link = GetImageLink().ToLower();
            if (link.IndexOf(".jpeg") >= 0)
                return "jpeg";
            if (link.IndexOf(".jpg") >= 0)
                return "jpg";
            if (link.IndexOf(".gif") >= 0)
                return "gif";
            if (link.IndexOf(".png") >= 0)
                return "png";



        return "";
        
    }

    internal void SetUrlTo(string newUrl)
    {
        m_text = string.Format("![{0}]({1})", GetImageLabel(), newUrl);
    }
    internal void SetLabelTo(string newLabel)
    {
        m_text = string.Format("![{0}]({1})", newLabel,GetImageLink());
    }

    internal string GetText()
    {
        return m_text;
    }
    
}

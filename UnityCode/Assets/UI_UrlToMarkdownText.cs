using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UrlToMarkdownText : MonoBehaviour
{

    [Header("Input")]
    public InputField m_altText;
    public InputField m_image;
    public InputField m_youtube;
    public Dropdown m_dropdown;
    public LoadAllImagesInProjectLocaly m_localDownloader;

    [Header("Preview")]
    public AspectRatioFitter m_ratioFilter;
    public RawImage m_imagePreview;
    public InputField m_markdownText;


    public void DownloadImageFromYoutube()
    {

    }


    void Start()
    {
        m_image.onEndEdit.AddListener(DownloadImage);
        m_youtube.onEndEdit.AddListener(DownloadYoutube);
        m_dropdown.onValueChanged.AddListener(DownloadYoutube);
    }

    private void DownloadYoutube(int value)
    {
        DownloadYoutube(m_youtube.text);
    }

    private void DownloadYoutube(string value)
    {
        string youtubeUrl = YoutubeThumbnail.GetImageUrlFromUrl(value,(YoutubeThumbnail.YoutubeImageType) m_dropdown.value);
        SetText( string.Format("[![{0}]({2})]({1})  \n{1}", m_altText.text, value, youtubeUrl));
        StartCoroutine(StartDownloadPreview(youtubeUrl));    
    }
    public string urlYoutube;
    public void DownloadYoutubeLocaly() {
        string name = LoadAllImagesInProjectLocaly.RandomName();
        string urlYoutube = GetYoutubeImageUrl();
        if (urlYoutube != "") {
            MarkdownImageAsText t = MarkdownImageAsText.CreateMarkdownImage(m_altText.text, urlYoutube);
            StartCoroutine(LoadAllImagesInProjectLocaly.StartDownloadingImage(
              t, name));
            t.SetUrlTo("Image/" + name + "." + t.GetFileExtension());
            SetText(t.GetText());
            StartCoroutine(StartDownloadPreview(urlYoutube));
        }
    }

    

    public void DownloadImageLocaly() {

        string name =  LoadAllImagesInProjectLocaly.RandomName();
        MarkdownImageAsText t = MarkdownImageAsText.CreateMarkdownImage(m_altText.text, m_image.text);
        StartCoroutine(LoadAllImagesInProjectLocaly.StartDownloadingImage(
          t,name));
        t.SetUrlTo("Image/" + name +"."+ t.GetFileExtension());
        SetText(t.GetText());
        StartCoroutine(StartDownloadPreview(m_image.text));
    }
    private string GetYoutubeImageUrl()
    {
        return YoutubeThumbnail.GetImageUrlFromUrl(m_youtube.text, (YoutubeThumbnail.YoutubeImageType)m_dropdown.value);
    }
    private void SetText(string textMardown)
    {
        m_markdownText.text = textMardown;
        GUIUtility.systemCopyBuffer = textMardown;
    }
    

    public IEnumerator StartDownloadPreview( string url) {

        WWW www = new WWW(url);
        yield return www;
        m_imagePreview.texture = www.texture;
        if( www.texture!=null)
            m_ratioFilter.aspectRatio = (float)www.texture.width/ (float)www.texture.height;
    }



    private void DownloadImage(string value)
    {
        SetText( string.Format("![{0}]({1})  ", m_altText.text, value));
        StartCoroutine(StartDownloadPreview(value));
    }
    
}

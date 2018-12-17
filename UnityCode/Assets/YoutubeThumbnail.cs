using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class YoutubeThumbnail : MonoBehaviour
{
    
    #region From WEB
    //https://stackoverflow.com/questions/39777659/extract-the-video-id-from-youtube-url-in-net
    private const string YoutubeLinkRegex = "(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+";
    private static Regex regexExtractId = new Regex(YoutubeLinkRegex, RegexOptions.Compiled);


    internal static string GetImageUrlFromUrl(string url, YoutubeImageType type)
    {
        Debug.Log("ulr" + url + "  --  " + type);
        if (url == "")
            return "";
        string id = GetYoutubeId(url);

        if (id == "")
            return "";

        Debug.Log("id" + id + "  --  " + type);
        return GetImageUrlFromId(type, id);
    }
    public static string GetYoutubeId(string url)
    {
        //extract the id
        var regRes = regexExtractId.Match(url);
        if (regRes.Success)
        {
            return regRes.Groups[1].Value;
        }
        return "";
    }
    #endregion

    public static string GetImageUrlFromId(YoutubeImageType type, string youtubeId) {
        if (youtubeId == "")
            return "";
        string urlFormat = "https://img.youtube.com/vi/{0}/{1}.jpg";

        switch (type)
        {
            case YoutubeImageType._0:
                return string.Format(urlFormat, youtubeId, "0");
               
            case YoutubeImageType._1:
                return string.Format(urlFormat, youtubeId, "1");
               
            case YoutubeImageType._2:
                return string.Format(urlFormat, youtubeId, "2");
                
            case YoutubeImageType._3:
                return string.Format(urlFormat, youtubeId, "3");
                
            case YoutubeImageType._hqdefault:
                return string.Format(urlFormat, youtubeId, "hqdefault");
                
            case YoutubeImageType._mqdefault:
                return string.Format(urlFormat, youtubeId, "mqdefault");
                
            case YoutubeImageType._maxresdefault:
                return string.Format(urlFormat, youtubeId, "maxresdefault");
                
            default:
                return "";
        }
    }


    public static  IEnumerator DownloadImage(string youtubeId,YoutubeImageType type, TextureResult texture) {

        WWW www = new WWW(GetImageUrlFromId(type, youtubeId) );
        yield return www;
        texture.m_texture = www.texture;
        texture.m_hasBeenLoaded = true;
       }

    public enum YoutubeImageType :int {
        _0=0,
        _1 = 1,
        _2 = 2,
        _3 = 3,
        _hqdefault = 4,
        _mqdefault = 5,
        _maxresdefault = 6
    }

    public class TextureResult
    {
        public bool m_hasBeenLoaded;
        public Texture2D m_texture;

    }
    private IEnumerator Start()
    {
        if (m_useDebugAtStart) {

            string id = "";
            TextureResult text= new TextureResult();
            foreach (string url in m_tdd)
            {
                id = GetYoutubeId(url);
                yield return DownloadImage(id, YoutubeImageType._maxresdefault, text);
                m_image.texture = text.m_texture;
                m_ratio.aspectRatio = (float)text.m_texture.width / (float)text.m_texture.height;

                Debug.Log("texture ("+id+"):" + text);
                yield return new WaitForSeconds(2);
            }
        }
    }

    [Header("Debug")]
    public bool m_useDebugAtStart;
    public AspectRatioFitter m_ratio;
    public RawImage m_image;
    public string[] m_tdd = new string[] {
        "http://youtu.be/AAAAAAAAA01"
,"http://www.youtube.com/embed/watch?feature=player_embedded&v=AAAAAAAAA02"
,"http://www.youtube.com/embed/watch?v=AAAAAAAAA03"
,"http://www.youtube.com/embed/v=AAAAAAAAA04"
,"http://www.youtube.com/watch?feature=player_embedded&v=AAAAAAAAA05"
,"http://www.youtube.com/watch?v=AAAAAAAAA06"
,"http://www.youtube.com/v/AAAAAAAAA07"
,"www.youtu.be/AAAAAAAAA08"
,"youtu.be/AAAAAAAAA09"
,"http://www.youtube.com/watch?v=i-AAAAAAA14&feature=related"
,"http://www.youtube.com/attribution_link?u=/watch?v=AAAAAAAAA15&feature=share&a=9QlmP1yvjcllp0h3l0NwuA"
,"http://www.youtube.com/attribution_link?a=fF1CWYwxCQ4&u=/watch?v=AAAAAAAAA16&feature=em-uploademail"
,"http://www.youtube.com/attribution_link?a=fF1CWYwxCQ4&feature=em-uploademail&u=/watch?v=AAAAAAAAA17"
,"http://www.youtube.com/v/A-AAAAAAA18?fs=1&rel=0"
,"http://www.youtube.com/watch/AAAAAAAAA11"
    };
}

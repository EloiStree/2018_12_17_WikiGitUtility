using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class DataUrlToPNG : MonoBehaviour
{
    public Texture2D m_image;
    [TextArea(1, 60)]
    public string m_dataUrl;
    [TextArea(1,60)]
    public string m_data;
    // Start is called before the first frame update
    IEnumerator DD()
    {


        m_data = Regex.Match(m_dataUrl, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
        
        byte [] binData = Convert.FromBase64String(m_data);
        m_dataUrl = "";
        File.WriteAllBytes(Application.dataPath + "/../dd.png", binData);

        WWW www = new WWW(Application.dataPath + "/../dd.png");
        yield return www;
        m_image = www.texture;

        //m_image =new Texture2D(16, 16, TextureFormat.PVRTC_RGBA4, false);
        //m_image.LoadRawTextureData(binData);
        //m_image.Apply();
    }


    private void OnValidate()
    {
        if(m_dataUrl!="" )
            StartCoroutine(DD());
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FindAndReplaceImageGlobalToLocal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal static void FindReplace(MarkdownImageInFile m_givenData, string newLocalUrl)
    {
        MarkdownImageAsText copy = m_givenData.markdownImage.Copy();
        copy.SetUrlTo(newLocalUrl);
        m_givenData.markdownFile.Load();
        string textOfFile = m_givenData.markdownFile.GetValue();
        textOfFile = textOfFile.Replace(m_givenData.markdownImage.GetText(), copy.GetText());
        File.WriteAllText(m_givenData.markdownFile.GetPath(), textOfFile);
        m_givenData.markdownFile.Unload();
    }
}

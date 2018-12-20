using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenURL : MonoBehaviour
{

    public InputField m_fieldToOpen;

    public void OpenUrl(string url) {
        Application.OpenURL(url);
    }

    public void OpenUrlFromInputField()
    {
        if(m_fieldToOpen!=null)
        Application.OpenURL(m_fieldToOpen.text);
    }
}

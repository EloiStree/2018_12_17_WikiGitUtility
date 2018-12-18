using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PageManager : MonoBehaviour
{
    [SerializeField]
    public Page[] pages;
    public GameObject[] m_deactivateOnSwitch;

   
    public void EnablePage(string pageName)
    {
        Reset();
        foreach (Page p in pages)
        {
            if (p.m_pageName == pageName)
            {
                p.Enable();
            }
        }
    }

    public void DisablePage(string pageName)
    {
        Reset();
        foreach (Page p in pages)
        {
            if (p.m_pageName == pageName)
            {
                p.Disable();
            }
        }
    }
    private void Reset()
    {
        foreach (var item in m_deactivateOnSwitch)
        {
            item.SetActive(false);

        }
    }

}
[System.Serializable]
public class Page {

    [SerializeField] public string m_pageName;
    [SerializeField] GameObject[] m_toEnable;
    [SerializeField] GameObject[] m_toDisable;

    public void Enable()
    {
        foreach (GameObject go in m_toEnable)
        {
            go.SetActive(true);
        }
    }
    public void Disable()
    {
        foreach (GameObject go in m_toDisable)
        {
            go.SetActive(false);
        }
    }
}

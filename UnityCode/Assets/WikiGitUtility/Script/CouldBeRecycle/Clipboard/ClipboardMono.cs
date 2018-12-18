using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClipboardMono : MonoBehaviour
{
    public string m_value;
    public OnChangeEvent m_onchange;


    private void OnEnable()
    {
        Clipboard.onChange += NotifyChange;
    }

   
    private void OnDisable()
    {
        
        Clipboard.onChange -= NotifyChange;
    }
    private void NotifyChange(string text)
    {
        m_onchange.Invoke(text);
    }


    private void Update()
    {
        m_value = Clipboard.Value;
    }
    [System.Serializable]
    public class OnChangeEvent : UnityEvent<string> { }
}
public static class Clipboard
{
    static string m_previousValue="";
    public static string Value {
        get {
            string current = GUIUtility.systemCopyBuffer;
            CheckForInstaneInScene();
            if (m_previousValue != current)
            {
                m_previousValue = current;
                if(onChange!=null)
                    onChange(current);
            }
            return current; }
        set {

            GUIUtility.systemCopyBuffer = value;
        }
    }
    static ClipboardMono m_instance;
    private static void CheckForInstaneInScene()
    {
        if (m_instance != null) return;
        m_instance = GameObject.FindObjectOfType<ClipboardMono>();

        if (m_instance != null) return;
        GameObject obj = new GameObject("#Clipboard");
        obj.AddComponent<ClipboardMono>();
        
    }

    public static OnChange onChange;
    public delegate void OnChange(string text);
}

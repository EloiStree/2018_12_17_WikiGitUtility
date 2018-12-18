using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationUI : MonoBehaviour
{
    public void NotifyMessage(string message) {
        NotificationUI.Notify(message);
    }
    public static void Notify(string message) {

        NotificationUI ui = GameObject.FindObjectOfType<NotificationUI>();
        if(ui!=null && ui.m_text!=null)
            ui.m_text.text = message;
        if (ui != null && ui.m_input != null)
            ui.m_input.text = message;
        ui.Display();
        ui.HideWithDefaultTime();
    }

    public Transform m_root;
    public Text m_text;
    public InputField m_input;
    public float m_defaultDisplayTime = 2f;

    private void Awake()
    {
        Hide();
    }

    private void HideWithDefaultTime()
    {
        Hide(m_defaultDisplayTime);
    }


    public void Display() {
        m_root.gameObject.SetActive(true);
    }

    public void Hide(float time) {
        m_hideCountDown = time;
    }
    public void Hide()
    {
        m_root.gameObject.SetActive(false);

    }
    public float m_hideCountDown;
    public void Update()
    {
        if (m_hideCountDown > 0f) {
            m_hideCountDown -= Time.deltaTime;
            if (m_hideCountDown < 0f)
            {
                m_hideCountDown = 0;
                Hide();
            }

        }
        
        if(Input.GetKeyDown(KeyCode.Space)){
            NotificationUI.Notify("" + UnityEngine.Random.Range(0, 500));
        }
    }
}

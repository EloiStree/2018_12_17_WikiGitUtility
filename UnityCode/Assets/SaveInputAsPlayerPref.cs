using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveInputAsPlayerPref : MonoBehaviour
{
    public InputField m_field;
    public string m_id;

    public void OnEnable()
    {

       m_field.text = PlayerPrefs.GetString(m_id);

    }

    private void OnDisable()
    {
        PlayerPrefs.SetString(m_id, m_field.text);
    }
    void Reset() {
        m_field = GetComponent<InputField>();
        m_id = ""+Random.Range(0, long.MaxValue);
    }
}

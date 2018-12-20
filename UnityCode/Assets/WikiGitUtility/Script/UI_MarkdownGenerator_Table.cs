using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MarkdownGenerator_Table : MonoBehaviour
{

    public InputField m_row;
    public InputField m_column;
    public InputField m_result;

    // Start is called before the first frame update
    void OnEnable()
    {
        m_result.onEndEdit.AddListener(SendToClipboard);
        m_row.onValueChanged.AddListener(SendToClipboard);
        m_column.onValueChanged.AddListener(SendToClipboard);
    }
    void OnDisable()
    {
        m_result.onEndEdit.RemoveListener(SendToClipboard);
        m_row.onValueChanged.RemoveListener(SendToClipboard);
        m_column.onValueChanged.RemoveListener(SendToClipboard);
    }

    private void SendToClipboard(string text)
    {
        m_result.text = GenerateTable(m_column.text, m_row.text);
        Clipboard.Value = m_result.text;
    }
    

    private string GenerateTable(string col, string row)
    {
        try
        {
            int c, r;
            int.TryParse(col, out  c);
            int.TryParse(row, out  r);
            return GenerateTable(c ,r);
        }
        finally { }
    }
    private string GenerateTable(int col, int row)
    {
        return MarkdownUtility.Default.EmtyArrayAsText(row, col);
    }
}

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
            return GenerateTable(int.Parse(col), int.Parse(row));
        }
        finally { }
    }
    private string GenerateTable(int col, int row)
    {
        if (col < 1)
            col = 1;
        if (row < 1)
            row = 1;
        string result="";

        for (int i = 0; i < col; i++)
        {
            result += "T" + i + " | ";
        }
        result = result.Substring(0, result.Length - 3);
        result += "\n";

        for (int i = 0; i < col; i++)
        {
            if (i == 0) result += ":-";
            else if (i == col-1) result += "-:";
            else result += ":-:";
            result += " | ";
            
        }
        result = result.Substring(0, result.Length - 3);
        result += "\n";


        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                result += " "+r+""+c+" | ";
            }
            result = result.Substring(0, result.Length - 3);
            result += "\n";
        }
        


        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Input_MarkdownWrapper : MonoBehaviour
{
    public InputField m_textInput;

    public void Italic() { m_textInput.text = "*"+m_textInput.text.Trim() + "*"; Clipboard.Value = m_textInput.text.Trim(); }
    public void Strong() { m_textInput.text = "**" + m_textInput.text.Trim() + "**"; Clipboard.Value = m_textInput.text.Trim(); }
    public void LineCode() { m_textInput.text = "`" + m_textInput.text.Trim() + "`"; Clipboard.Value = m_textInput.text.Trim(); }
    public void MutilineCode() { m_textInput.text = "  \n``` cs  \n" + m_textInput.text.Trim() + "  \n```  \n"; Clipboard.Value = m_textInput.text.Trim(); }
    public void Link() { m_textInput.text = "[" + m_textInput.text.Trim() + "](#)"; Clipboard.Value = m_textInput.text.Trim(); }
    public void Image() { m_textInput.text = "![" + m_textInput.text.Trim() + "](#)"; Clipboard.Value = m_textInput.text.Trim(); }

    public void Strikethrough() { m_textInput.text = "~~" + m_textInput.text + "~~"; Clipboard.Value = m_textInput.text.Trim(); }

    
    public void Quote() { m_textInput.text = "> " + m_textInput.text + " \n  \n"; Clipboard.Value = m_textInput.text; }
    public void Title() {
        string t = "#";
        if (m_textInput.text.Substring(0, 1) != "#")
            t = "# ";
        m_textInput.text = t + m_textInput.text + "  \n"; Clipboard.Value = m_textInput.text; }
    public void LineBreak() { m_textInput.text = "" + m_textInput.text + "  \n  \n---------  \n  \n"; Clipboard.Value = m_textInput.text; }
    public void LineReturn() { m_textInput.text = "" + m_textInput.text + "  \n"; Clipboard.Value = m_textInput.text; }

    public void Clear() {
        m_textInput.text = "";
    }



}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Input_MarkdownWrapper : MonoBehaviour
{
    public InputField m_textInput;

    private void SetWith(string text)
    {
        Clipboard.Value = m_textInput.text = text;
    }

    private string GetCurrent()
    {
        return m_textInput.text;
    }

    public void Italic() { SetWith(MarkdownUtility.Default.Italic(GetCurrent())); }
    public void Strong() { SetWith(MarkdownUtility.Default.Strong(GetCurrent())); }
    public void LineCode() { SetWith(MarkdownUtility.Default.OneLineCode(GetCurrent())); }
    public void MutilineCode() { SetWith(MarkdownUtility.Default.MultiLineCode(GetCurrent())); }
    public void Link() { SetWith(MarkdownUtility.Default.Link(GetCurrent()," ")); }
    public void Image() { SetWith(MarkdownUtility.Default.Image(GetCurrent()," ")); }
    public void Strikethrough() { SetWith(MarkdownUtility.Default.Strikethrough(GetCurrent())); }
    public void Title() { SetWith(MarkdownUtility.Default.Title(GetCurrent())); }
    public void LineBreak() { SetWith(MarkdownUtility.Default.LineBreak(GetCurrent())); }
    public void Quote() { SetWith(MarkdownUtility.Default.Quote(GetCurrent())); }
    public void LineReturn() { SetWith(MarkdownUtility.Default.LineReturn(GetCurrent())); }
    public void Subsection() { SetWith(MarkdownUtility.Default.Subsection(GetCurrent())); }


    public void EnumerationList() { SetWith(MarkdownUtility.Default.SimpleEnumeration(GetCurrent())); }
    public void ClassicList() { SetWith(MarkdownUtility.Default.SimpleList(GetCurrent())); }
    public void CheckList() { SetWith(MarkdownUtility.Default.SimpleCheckList(GetCurrent())); }

    public void Clear() {
        m_textInput.text = "";
    }



}

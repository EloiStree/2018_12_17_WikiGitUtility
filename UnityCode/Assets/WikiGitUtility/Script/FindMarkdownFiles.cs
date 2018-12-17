using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class FindMarkdownFiles : MonoBehaviour
{

    [SerializeField]
    string m_gitProjetPath;

    [SerializeField]
    string[] m_mdFiles;
    [SerializeField]
    List<MarkdownFileWithText> m_markdownFiles;

    [SerializeField]
    MarkdownFileDetectedEvent m_markdownFileDetected;



    public void SetProjectPath(string path) {
        m_gitProjetPath = path;
    }

    void Start()
    {
        SetProjectPath(PlayerPrefs.GetString("FindMarkdownFiles"));
        CheckForMarkdownFilesInProject();
    }
    private void OnDestroy()
    {
        PlayerPrefs.SetString("FindMarkdownFiles", m_gitProjetPath);
    }

    public List<MarkdownFileWithText> CheckForMarkdownFilesInProject() {
        ResetValues();
        if (!Directory.Exists(m_gitProjetPath)) {
            return m_markdownFiles ;
        }
        m_mdFiles = Directory.GetFiles(@m_gitProjetPath, "*.md", SearchOption.AllDirectories);
        for (int i = 0; i < m_mdFiles.Length; i++)
        {
            MarkdownFileWithText file = new MarkdownFileWithText(m_mdFiles[i],m_gitProjetPath, false);
            m_markdownFiles.Add(file);
            m_markdownFileDetected.Invoke(file);
        }
        return m_markdownFiles;
    }

    private void ResetValues()
    {
        m_markdownFiles = new List<MarkdownFileWithText>();
        m_mdFiles = new string[0];
    }

    private string previousPathInInspector;
    private void OnValidate()
    {
        if (previousPathInInspector != m_gitProjetPath)
        {
            SetProjectPath(m_gitProjetPath);
            CheckForMarkdownFilesInProject();

        }
    }
}

[System.Serializable]
public class MarkdownFileDetectedEvent : UnityEvent<MarkdownFileWithText> { }

[System.Serializable]
public class MarkdownFileWithText
{
    public MarkdownFileWithText(string absolutePath, string projectPath, bool autoLoad) {
        m_absolutePath = absolutePath;
        m_projectPath = projectPath;
        if (autoLoad)
            Load();
          
    }
    [SerializeField] string m_absolutePath;
    [SerializeField] string m_projectPath;
    [SerializeField] string m_text;
    [SerializeField] bool m_hasBeenLoad;
    [SerializeField] bool m_isFileExisting;

    public int TextLenght { get { return m_text.Length; } }

    public string GetValue() { return m_text; }
    public bool IsFileExisting() {
        return m_isFileExisting = File.Exists(m_absolutePath); 
    }
    public void Load() {
        m_hasBeenLoad = true;
        if (IsFileExisting())
        {
            m_text = File.ReadAllText(m_absolutePath);
        }
    }
    public void Unload() {
        m_text = "";
        m_hasBeenLoad = false;
    }

    internal string GetPath()
    {
        return m_absolutePath;
    }

    internal string GetProjectPath()
    {
        return m_projectPath;
    }
}
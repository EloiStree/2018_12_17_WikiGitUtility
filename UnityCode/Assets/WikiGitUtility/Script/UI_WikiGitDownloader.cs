using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_WikiGitDownloader : MonoBehaviour
{
    [SerializeField]
    public InputField m_projectPath;

    [SerializeField]
    LoadAllImagesInProjectLocaly m_loadImages;

    public string[] m_folders;

    public float m_prefabHeight = 200;
    public GameObject m_uiImageEditPrefab;
    public RectTransform m_rootOfImageEditors;
    public List<MarkdownImageInFile> m_imageinFileInProject;

    public void Start()
    {
        FlushImagesEditor();
       
        m_projectPath.text = PlayerPrefs.GetString("UI_WikiGitDownloader");
        if (m_projectPath.text == "")
            m_projectPath.text = Application.dataPath;

    }

    public void OnDestroy()
    {
        PlayerPrefs.SetString("UI_WikiGitDownloader", m_projectPath.text);

    }
    public void DownloadFromWebBackupImages() {
        m_loadImages.LaunchLoadNonLocalImageAsBackup();
    }


    public void LoadImageLinkedToWeb() {
        FlushImagesEditor();
        m_imageinFileInProject = m_loadImages. LoadAllWebImagesReference(m_projectPath.text);
       // m_rootOfImageEditors.sizeDelta. = new Vector2(1, m_imageinFileInProject.Count);
        foreach (MarkdownImageInFile imageToFile in m_imageinFileInProject)
        {
            GameObject gamo = GameObject.Instantiate(m_uiImageEditPrefab);
            gamo.transform.SetParent( m_rootOfImageEditors,false);
            UI_MarkDownImageEdit edit = gamo.GetComponent<UI_MarkDownImageEdit>();
            edit.SetWith(imageToFile);
        }

        Vector2 r = m_rootOfImageEditors.sizeDelta;
        r.y = m_prefabHeight * m_imageinFileInProject.Count;
        m_rootOfImageEditors.sizeDelta =r;
    }

    public void FlushImagesEditor() {
        Transform[] children = new Transform[m_rootOfImageEditors.childCount];
        for (int i = 0; i < m_rootOfImageEditors.childCount; i++)
        {
            children[i] = m_rootOfImageEditors.GetChild(i);
        }
        m_rootOfImageEditors.DetachChildren();
        for (int i = 0; i < children.Length; i++)
        {
            //foreach (var compo in children[i].GetComponents<LayoutElement>())
            //    Destroy(compo);
            //foreach (var compo in children[i].GetComponents<Image>())
            //    Destroy(compo);
            
            Destroy(children[i].gameObject);
        }
    }

    public bool m_gitRoot;
    public void CheckIfPathIsGitRootFolder() {

        if (Directory.Exists(m_projectPath.text))
        {
            m_folders = Directory.GetDirectories(m_projectPath.text);
            foreach (string path in m_folders)
            {
                m_gitRoot = path.EndsWith("\\.git");
                if (m_gitRoot)
                    break;
            }
            if (m_gitRoot == false)
            {
                SetAsNoProjetPath();
            }

        }
        else {
            SetAsNoProjetPath();
        }

    }

    private void SetAsNoProjetPath()
    {

        m_projectPath.text = "";
        m_folders = new string[0];
        m_gitRoot = false;
    }
}

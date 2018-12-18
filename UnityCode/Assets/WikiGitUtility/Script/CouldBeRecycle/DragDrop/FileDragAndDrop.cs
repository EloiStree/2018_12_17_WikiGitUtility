using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using B83.Win32;

#endif
public class FileDragAndDrop : MonoBehaviour
{
    [System.Serializable]
    public class FileDroppedEvent : UnityEvent<string> {}
    public FileDroppedEvent m_onFileDropped;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN  
    // important to keep the instance alive while the hook is active.
    UnityDragAndDropHook hook;
    void OnEnable()
    {
        // must be created on the main thread to get the right thread id.
        hook = new UnityDragAndDropHook();
        hook.InstallHook();
        hook.OnDroppedFiles += OnFiles;
    }
    void OnDisable()
    {
        hook.UninstallHook();
    }

    void OnFiles(List<string> aFiles, POINT aPos)
    {
        // do something with the dropped file names. aPos will contain the 
        // mouse position within the window where the files has been dropped.
        Debug.Log("Dropped " + aFiles.Count + " files at: " + aPos + "\n" +
            aFiles.Aggregate((a, b) => a + "\n" + b));
        for (int i = 0; i < aFiles.Count; i++)
        {
            m_onFileDropped.Invoke(aFiles[i].Trim());
        }
    }
#endif
}

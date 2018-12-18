using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OnFileDropped : MonoBehaviour
{
    
    public void FileDropped(string path)
    {
        NotificationUI.Notify("Dropped: " + path);    
    }
}

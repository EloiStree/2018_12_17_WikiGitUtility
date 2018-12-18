using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataUrlToPNG : MonoBehaviour
{
    [TextArea(1,60)]
    public string m_data;
    // Start is called before the first frame update
    void Start()
    {

        byte [] binData = Convert.FromBase64String(m_data);
        File.WriteAllBytes(Application.dataPath + "/dd.png", binData);

    }
    
}

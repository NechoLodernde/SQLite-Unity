using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager PlayerDataInstance { get; private set; }


    private void Awake()
    {
        PlayerDataInstance = this;

        DontDestroyOnLoad(gameObject);
    }
}

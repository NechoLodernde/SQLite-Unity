using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageClipsManager : MonoBehaviour
{
    public static ImageClipsManager Instance { get; private set; }
    public ImageClipsStruct Struct;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string imgCCode, string imgCName,
        string imgCPath)
    {
        ImageClipsEntry newEntry = new();
        newEntry.imageClipsCode = imgCCode;
        newEntry.imageClipsName = imgCName;
        newEntry.imageClipsPath = imgCPath;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevICCode)
    {
        foreach (ImageClipsEntry entry in Struct.list)
        {
            if (entry.imageClipsCode.Equals(prevICCode))
            {
                Struct.list.Remove(entry);
                break;
            }
        }
    }

    public void ClearList()
    {
        Struct.list.Clear();
    }

    public void UpdateICCode(string prevICCode, string newICCode)
    {
        foreach (ImageClipsEntry entry in Struct.list)
        {
            if (prevICCode.Equals(entry.imageClipsCode))
            {
                entry.imageClipsCode = newICCode;
                break;
            }
        }
    }

    public void UpdateICName(string prevICCode, string newICName)
    {
        foreach (ImageClipsEntry entry in Struct.list)
        {
            if (prevICCode.Equals(entry.imageClipsCode))
            {
                entry.imageClipsName = newICName;
                break;
            }
        }
    }

    public void UpdateICPath(string prevICCode, string newICPath)
    {
        foreach (ImageClipsEntry entry in Struct.list)
        {
            if (prevICCode.Equals(entry.imageClipsCode))
            {
                entry.imageClipsPath = newICPath;
                break;
            }
        }
    }
}

[System.Serializable]
public class ImageClipsStruct
{
    public List<ImageClipsEntry> list = new();
}

[System.Serializable]
public class ImageClipsEntry
{
    public string imageClipsCode, imageClipsName, imageClipsPath;
}

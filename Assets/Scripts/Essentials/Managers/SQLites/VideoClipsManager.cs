using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VideoClipsManager : MonoBehaviour
{
    public static VideoClipsManager Instance { get; private set; }
    public VideoClipsStruct Struct;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string vCCode, string vCName,
        string vCPath)
    {
        VideoClipsEntry newEntry = new();
        newEntry.videoClipsCode = vCCode;
        newEntry.videoClipsName = vCName;
        newEntry.videoClipsPath = vCPath;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevVCCode)
    {
        foreach (VideoClipsEntry entry in Struct.list)
        {
            if (entry.videoClipsCode.Equals(prevVCCode))
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

    public void UpdateVCCode(string prevVCCode, string newVCCode)
    {
        foreach (VideoClipsEntry entry in Struct.list)
        {
            if (prevVCCode.Equals(entry.videoClipsCode))
            {
                entry.videoClipsCode = newVCCode;
                break;
            }
        }
    }

    public void UpdateVCName(string prevVCCode, string newVCName)
    {
        foreach (VideoClipsEntry entry in Struct.list)
        {
            if (prevVCCode.Equals(entry.videoClipsCode))
            {
                entry.videoClipsName = newVCName;
                break;
            }
        }
    }

    public void UpdateVCPath(string prevVCCode, string newVCPath)
    {
        foreach (VideoClipsEntry entry in Struct.list)
        {
            if (prevVCCode.Equals(entry.videoClipsCode))
            {
                entry.videoClipsPath = newVCPath;
                break;
            }
        }
    }
}

[System.Serializable]
public class VideoClipsStruct
{
    public List<VideoClipsEntry> list = new();
}

[System.Serializable]
public class VideoClipsEntry
{
    public string videoClipsCode, videoClipsName, videoClipsPath;
}

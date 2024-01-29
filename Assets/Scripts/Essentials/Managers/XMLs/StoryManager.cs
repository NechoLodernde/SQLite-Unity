using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance { get; private set; }
    public StoryStruct Struct;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string sCode, string sGroup,
        int sIndex, string sUCode, string dCode)
    {
        StoryEntry newEntry = new()
        {
            storyCode = sCode,
            storyGroup = sGroup,
            storyIndex = sIndex,
            storyUCode = sUCode,
            dialogueCode = dCode
        };

        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevSCode)
    {
        foreach (StoryEntry entry in Struct.list)
        {
            if (prevSCode.Equals(entry.storyCode))
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

    public void UpdateSCode(string prevSCode, string newSCode)
    {
        foreach (StoryEntry entry in Struct.list)
        {
            if (prevSCode.Equals(entry.storyCode))
            {
                entry.storyCode = newSCode;
                break;
            }
        }
    }

    public void UPdateSGroup(string prevSCode, string newSGroup)
    {
        foreach (StoryEntry entry in Struct.list)
        {
            if (prevSCode.Equals(entry.storyCode))
            {
                entry.storyGroup = newSGroup;
                break;
            }
        }
    }

    public void UpdateSIndex(string prevSCode, int newSIndex)
    {
        foreach (StoryEntry entry in Struct.list)
        {
            if (prevSCode.Equals(entry.storyCode))
            {
                entry.storyIndex = newSIndex;
                break;
            }
        }
    }

    public void UpdateSUCode(string prevSCode, string newSUCode)
    {
        foreach (StoryEntry entry in Struct.list)
        {
            if (prevSCode.Equals(entry.storyCode))
            {
                entry.storyUCode = newSUCode;
                break;
            }
        }
    }

    public void UpdateDCode(string prevSCode, string newDCode)
    {
        foreach (StoryEntry entry in Struct.list)
        {
            if (prevSCode.Equals(entry.storyCode))
            {
                entry.dialogueCode = newDCode;
                break;
            }
        }
    }
}

[System.Serializable]
public class StoryStruct
{
    public List<StoryEntry> list = new();
}

[System.Serializable]
public class StoryEntry
{
    public string storyCode, storyGroup;
    public int storyIndex;
    public string storyUCode, dialogueCode;
}

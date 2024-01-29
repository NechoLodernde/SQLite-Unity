using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCListManager : MonoBehaviour
{
    public static NPCListManager Instance { get; private set; }
    public NPCLStruct Struct;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string npcLCode, string npcLName,
        string npcLMPath, string npcLType)
    {
        NPCLEntry newEntry = new();
        newEntry.npcLCode = npcLCode;
        newEntry.npcLName = npcLName;
        newEntry.npcLMPath = npcLMPath;
        newEntry.npcLType = npcLType;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevNLCode)
    {
        foreach (NPCLEntry entry in Struct.list)
        {
            if (entry.npcLCode.Equals(prevNLCode))
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

    public void UpdateNLCode(string prevNLCode, string newNLCode)
    {
        foreach (NPCLEntry entry in Struct.list)
        {
            if (prevNLCode.Equals(entry.npcLCode))
            {
                entry.npcLCode = newNLCode;
                break;
            }
        }
    }

    public void UpdateNLName(string prevNLCode, string newNLName)
    {
        foreach (NPCLEntry entry in Struct.list)
        {
            if (prevNLCode.Equals(entry.npcLCode))
            {
                entry.npcLName = newNLName;
                break;
            }
        }
    }

    public void UpdateNLMPath(string prevNLCode, string newNLMPath)
    {
        foreach (NPCLEntry entry in Struct.list)
        {
            if (prevNLCode.Equals(entry.npcLCode))
            {
                entry.npcLMPath = newNLMPath;
                break;
            }
        }
    }

    public void UpdateNLType(string prevNLCode, string newNLType)
    {
        foreach (NPCLEntry entry in Struct.list)
        {
            if (prevNLCode.Equals(entry.npcLCode))
            {
                entry.npcLType = newNLType;
                break;
            }
        }
    }
}

[System.Serializable]
public class NPCLStruct
{
    public List<NPCLEntry> list = new();
}

[System.Serializable]
public class NPCLEntry
{
    public string npcLCode, npcLName, npcLMPath, npcLType;
}

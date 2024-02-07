using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class PlayerInventoryManager : MonoBehaviour
{
    public static PlayerInventoryManager Instance { get; private set; }
    public PlayerInventoryStruct Struct;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string invID, int invNumber,
        string invName, int invUseCode)
    {
        PlayerInventoryEntry newEntry = new();
        newEntry.inventoryID = invID;
        newEntry.inventoryNumber = invNumber;
        newEntry.inventoryName = invName;
        newEntry.inventoryUsableCode = invUseCode;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevInvID)
    {     
        foreach (PlayerInventoryEntry inv in Struct.list)
        {
            if (inv.inventoryID.Equals(prevInvID))
            {
                Struct.list.Remove(inv);
                break;
            }
        }     
    }

    public void ClearList()
    {
        Struct.list.Clear();
    }

    public void UpdateInvID(string prevInvID, string newIntID)
    {
        foreach (PlayerInventoryEntry entry in Struct.list)
        {
            if (prevInvID.Equals(entry.inventoryID))
            {
                entry.inventoryID = newIntID;
                break;
            }
        }
    }

    public void UpdateInvNumber(string prevInvID, int newInvNumber)
    {
        foreach (PlayerInventoryEntry entry in Struct.list)
        {
            if (prevInvID.Equals(entry.inventoryID))
            {
                entry.inventoryNumber = newInvNumber;
                break;
            }
        }
    }

    public void UpdateInvName(string prevInvID, string newInvName)
    {
        foreach (PlayerInventoryEntry entry in Struct.list)
        {
            if (prevInvID.Equals(entry.inventoryID))
            {
                entry.inventoryName = newInvName;
                break;
            }
        }
    }

    public void UpdateUsableCode (string prevInvID, int newUseID)
    {
        foreach (PlayerInventoryEntry entry in Struct.list)
        {
            if (prevInvID.Equals(entry.inventoryID))
            {
                entry.inventoryUsableCode = newUseID;
                break;
            }
        }
    }
}

[System.Serializable]
public class PlayerInventoryStruct
{
    public List<PlayerInventoryEntry> list = new();
}

[System.Serializable]
public class PlayerInventoryEntry
{
    public string inventoryID;
    public int inventoryNumber;
    public string inventoryName;
    public int inventoryUsableCode;
}

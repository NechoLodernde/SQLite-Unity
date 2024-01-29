using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class PlayerInventoryManager : MonoBehaviour
{
    public static PlayerInventoryManager PlayerInventoryInstance { get; private set; }
    public PlayerInventoryStruct inventoryStruct;

    private void Awake()
    {
        PlayerInventoryInstance = this;

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
        inventoryStruct.list.Add(newEntry);
    }

    public void DeleteData(string prevInvID)
    {     
        foreach (PlayerInventoryEntry inv in inventoryStruct.list)
        {
            if (inv.inventoryID.Equals(prevInvID))
            {
                inventoryStruct.list.Remove(inv);
                break;
            }
        }     
    }

    public void UpdateUsableCode (string prevInvID, int newUseID)
    {
        foreach (PlayerInventoryEntry inv in inventoryStruct.list)
        {
            if (prevInvID.Equals(inv.inventoryID))
            {
                inv.inventoryUsableCode = newUseID;
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

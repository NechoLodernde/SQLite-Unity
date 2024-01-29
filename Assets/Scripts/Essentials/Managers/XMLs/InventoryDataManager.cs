using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class InventoryDataManager : MonoBehaviour
{
    public static InventoryDataManager Instance { get; private set; }
    public InventoryDataStruct Struct;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string invID, string invName,
        string invType, double invWeight, int invUseCode)
    {
        InventoryDataEntry newEntry = new();
        newEntry.inventoryID = invID;
        newEntry.inventoryName = invName;
        newEntry.inventoryType = invType;
        newEntry.inventoryWeight = invWeight;
        newEntry.inventoryUsableCode = invUseCode;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevInvID)
    {
        foreach (InventoryDataEntry entry in Struct.list)
        {
            if (entry.inventoryID.Equals(prevInvID))
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

    public void UpdateInvID(string pInvID, string newInvID)
    {
        foreach (InventoryDataEntry entry in Struct.list)
        {
            if (pInvID.Equals(entry.inventoryID))
            {
                entry.inventoryID = newInvID;
                break;
            }
        }
    }

    public void UpdateInvName(string pInvID, string newInvName)
    {
        foreach (InventoryDataEntry entry in Struct.list)
        {
            if (pInvID.Equals(entry.inventoryID))
            {
                entry.inventoryName = newInvName;
                break;
            }
        }
    }

    public void UpdateInvType(string pInvID, string newInvType)
    {
        foreach (InventoryDataEntry entry in Struct.list)
        {
            if (pInvID.Equals(entry.inventoryID))
            {
                entry.inventoryType = newInvType;
                break;
            }
        }
    }

    public void UpdateWeight(string pInvID, double newWeight)
    {
        foreach(InventoryDataEntry inv in Struct.list)
        {
            if (pInvID.Equals(inv.inventoryID))
            {
                inv.inventoryWeight = newWeight;
                break;
            }
        }
    }

    public void UpdateUsableCode (string pInvID, int newUseID)
    {
        foreach(InventoryDataEntry inv in Struct.list)
        {
            if (pInvID.Equals(inv.inventoryID))
            {
                inv.inventoryUsableCode = newUseID;
                break;
            }
        }
    }
}

[System.Serializable]
public class InventoryDataStruct
{
    public List<InventoryDataEntry> list = new();
}

[System.Serializable]
public class InventoryDataEntry
{
    public string inventoryID, inventoryName, inventoryType;
    public double inventoryWeight;
    public int inventoryUsableCode;
}

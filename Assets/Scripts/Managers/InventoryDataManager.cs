using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class InventoryDataManager : MonoBehaviour
{
    public static InventoryDataManager InventoryDataInstance { get; private set; }
    public InventoryDataStruct inventoryStruct;

    private void Awake()
    {
        InventoryDataInstance = this;

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
        inventoryStruct.list.Add(newEntry);
    }

    public void UpdateWeight(string prevInvID, double newWeight)
    {
        foreach(InventoryDataEntry inv in inventoryStruct.list)
        {
            if (prevInvID.Equals(inv.inventoryID))
            {
                inv.inventoryWeight = newWeight;
            }
        }
    }

    public void UpdateUsableCode (string prevInvID, int newUseID)
    {
        foreach(InventoryDataEntry inv in inventoryStruct.list)
        {
            if (prevInvID.Equals(inv.inventoryID))
            {
                inv.inventoryUsableCode = newUseID;
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

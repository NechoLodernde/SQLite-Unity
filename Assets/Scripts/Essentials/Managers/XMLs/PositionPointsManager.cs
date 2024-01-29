using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionPointsManager : MonoBehaviour
{
    public static PositionPointsManager Instance { get; private set; }
    public PositionPStruct Struct;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string pPCode, string pPSCode,
        double pPXPos, double pPYPos, double pPZPos)
    {
        PositionPEntry newEntry = new();
        newEntry.positionPCode = pPCode;
        newEntry.positionPSCode = pPSCode;
        newEntry.positionPXPos = pPXPos;
        newEntry.positionPYPos = pPYPos;
        newEntry.positionPZPos = pPZPos;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevPPCode)
    {
        foreach (PositionPEntry entry in Struct.list)
        {
            if (entry.positionPCode.Equals(prevPPCode))
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

    public void UpdatePPCode(string prevPPCode, string newPPCode)
    {
        foreach (PositionPEntry entry in Struct.list)
        {
            if (prevPPCode.Equals(entry.positionPSCode))
            {
                entry.positionPCode = newPPCode;
                break;
            }
        }
    }

    public void UpdatePPSCode(string prevPPCode, string newPPSCode)
    {
        foreach (PositionPEntry entry in Struct.list)
        {
            if (prevPPCode.Equals(entry.positionPCode))
            {
                entry.positionPSCode = newPPSCode;
                break;
            }
        }
    }

    public void UpdatePPXPos(string prevPPCode, double newPPXPos)
    {
        foreach (PositionPEntry entry in Struct.list)
        {
            if (prevPPCode.Equals(entry.positionPCode))
            {
                entry.positionPXPos = newPPXPos;
                break;
            }
        }
    }

    public void UpdatePPYPos(string prevPPCode, double newPPYPos)
    {
        foreach (PositionPEntry entry in Struct.list)
        {
            if (prevPPCode.Equals(entry.positionPCode))
            {
                entry.positionPYPos = newPPYPos;
                break;
            }
        }
    }

    public void UpdatePPZPos(string prevPPCode, double newPPZPos)
    {
        foreach (PositionPEntry entry in Struct.list)
        {
            if (prevPPCode.Equals(entry.positionPCode))
            {
                entry.positionPZPos = newPPZPos;
                break;
            }
        }
    }
}

[System.Serializable]
public class PositionPStruct
{
    public List<PositionPEntry> list = new();
}

[System.Serializable]
public class PositionPEntry
{
    public string positionPCode, positionPSCode;
    public double positionPXPos, positionPYPos, positionPZPos;
}

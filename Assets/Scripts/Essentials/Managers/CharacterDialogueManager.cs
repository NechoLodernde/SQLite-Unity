using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogueManager : MonoBehaviour
{
    public static CharacterDialogueManager Instance { get; private set; }
    public CharDialStruct Struct;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void InsertNewData(string cDCode, string cDName,
        string cDTarget, string cDText)
    {
        CharDialEntry newEntry = new();
        newEntry.charDialCode = cDCode;
        newEntry.charDialName = cDName;
        newEntry.charDialTarget = cDTarget;
        newEntry.charDialText = cDText;
    }

    public void DeleteData(string prevCDCode)
    {
        foreach (CharDialEntry cDial in Struct.list)
        {
            if (cDial.charDialCode.Equals(prevCDCode))
            {
                Struct.list.Remove(cDial);
                break;
            }
        }
    }

    public void ClearList()
    {
        Struct.list.Clear();
    }

    public void UpdateCharDialCode (string prevCDCode, string newCDCode)
    {
        foreach (CharDialEntry cDial in Struct.list)
        {
            if (prevCDCode.Equals(cDial.charDialCode))
            {
                cDial.charDialCode = newCDCode;
                break;
            }
        }
    }

    public void UpdateCharDialName (string prevCDCode,
        string newCDName)
    {
        foreach (CharDialEntry cDial in Struct.list)
        {
            if (prevCDCode.Equals(cDial.charDialCode))
            {
                cDial.charDialName = newCDName;
                break;
            }
        }
    }

    public void UpdateCharDialTarget (string prevCDCode,
        string newCDTarget)
    {
        foreach (CharDialEntry cDial in Struct.list)
        {
            if (prevCDCode.Equals(cDial.charDialCode))
            {
                cDial.charDialTarget = newCDTarget;
                break;
            }
        }
    }

    public void UpdateCharDialText (string prevCDCode,
        string newCDText)
    {

    }
}

[System.Serializable]
public class CharDialStruct
{
    public List<CharDialEntry> list = new();
}

[System.Serializable]
public class CharDialEntry
{
    public string charDialCode;
    public string charDialName;
    public string charDialTarget;
    public string charDialText;
}

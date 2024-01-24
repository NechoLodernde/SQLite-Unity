using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public DialStruct Struct;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string dCode, string dCName,
        string dText, string cDCode)
    {
        DialEntry newEntry = new();
        newEntry.dialCode = dCode;
        newEntry.dialCharName = dCName;
        newEntry.dialText = dText;
        newEntry.charDialCode = cDCode;
        Struct.list.Add(newEntry);
    }

    public void DeleteData(string prevDCode)
    {
        foreach (DialEntry dial in Struct.list)
        {
            if (dial.dialCode.Equals(prevDCode))
            {
                Struct.list.Remove(dial);
                break;
            }
        }
    }

    public void ClearList()
    {
        Struct.list.Clear();
    }

    public void UpdateDialCode (string prevDCode, string newDCode)
    {
         foreach (DialEntry dial in Struct.list)
        {
            if (prevDCode.Equals(dial.dialCode))
            {
                dial.dialCode = newDCode;
                break;
            }
        }
    }

    public void UpdateDialCharName (string prevDCode, string newDCName)
    {
        foreach (DialEntry dial in Struct.list)
        {
            if (prevDCode.Equals(dial.dialCode))
            {
                dial.dialCharName = newDCName;
                break;
            }
        }
    }

    public void UpdateDialText (string prevDCode, string newDText)
    {
        foreach (DialEntry dial in Struct.list)
        {
            if (prevDCode.Equals(dial.dialCode))
            {
                dial.dialText = newDText;
                break;
            }
        }
    }

    public void UpdateCharDialCode(string prevDCode, string newCDCode)
    {
        foreach (DialEntry dial in Struct.list)
        {
            if (prevDCode.Equals(dial.dialCode))
            {
                dial.charDialCode = newCDCode;
                break;
            }
        }
    }
}

[System.Serializable]
public class DialStruct
{
    public List<DialEntry> list = new();
}

[System.Serializable]
public class DialEntry
{
    public string dialCode;
    public string dialCharName;
    public string dialText;
    public string charDialCode;
}

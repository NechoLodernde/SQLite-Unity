using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class CurriculumManager : MonoBehaviour
{
    public static CurriculumManager CurriculumManagerInstance { get; private set; }
    public CurriculumStruct curriculumStruct;

    private void Awake()
    {
        CurriculumManagerInstance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void InsertNewData(string sCode, string sName,
        string sFaculty, string sSProgram, int sTCredit,
        int sDuration, string sPCode, int sSemester,
        string sYear, string sRCode)
    {
        CurriculumEntry newEntry = new();
        newEntry.subjectCode = sCode;
        newEntry.subjectName = sName;
        newEntry.subjectFaculty = sFaculty;
        newEntry.subjectStudyProgram = sSProgram;
        newEntry.subjectTotalCredit = sTCredit;
        newEntry.subjectDuration = sDuration;
        newEntry.subjectPrerequisiteCode = sPCode;
        newEntry.subjectSemester = sSemester;
        newEntry.subjectYear = sYear;
        newEntry.subjectRoomCode = sRCode;
        curriculumStruct.list.Add(newEntry);
    }

    public void DeleteData(string prevSCode)
    {
        foreach (CurriculumEntry curri in curriculumStruct.list)
        {
            if (curri.subjectCode.Equals(prevSCode))
            {
                curriculumStruct.list.Remove(curri);
                break;
            }
        }
    }

    public void ClearList()
    {
        curriculumStruct.list.Clear();
    }

    public void UpdateSubjectCode (
        string prevSCode, string newSCode)
    {
        foreach (CurriculumEntry curri in
            curriculumStruct.list)
        {
            if (prevSCode.Equals(curri.subjectCode))
            {
                curri.subjectCode = newSCode;
                break;
            }
        }
    }

    public void UpdateSubjectName (
        string prevSCode, string newSName)
    {
        foreach (CurriculumEntry curri in
            curriculumStruct.list)
        {
            if (prevSCode.Equals(curri.subjectCode))
            {
                curri.subjectName = newSName;
                break;
            }
        }
    }

    public void UpdateSubjectTotalCredit (
        string prevSCode, int newTotalCredit)
    {
        foreach (CurriculumEntry curri in
            curriculumStruct.list)
        {
            if (prevSCode.Equals(curri.subjectCode))
            {
                curri.subjectTotalCredit = newTotalCredit;
                break;
            }
        }
    }

    public void UpdateSubjectTotalDuration(
        string prevSCode, int newSubjectDuration)
    {
        foreach (CurriculumEntry curri in
            curriculumStruct.list)
        {
            if (prevSCode.Equals(curri.subjectCode))
            {
                curri.subjectDuration = newSubjectDuration;
                break;
            }
        }
    }

    public void UpdateSubjectPrerequisite(
        string prevSCode, string newSP)
    {
        foreach (CurriculumEntry curri in
            curriculumStruct.list)
        {
            if (prevSCode.Equals(curri.subjectCode))
            {
                curri.subjectPrerequisiteCode = newSP;
                break;
            }
        }
    }

    public void UpdateSubjectSemester(
        string prevSCode, int newSSemester)
    {
        foreach (CurriculumEntry curri in
            curriculumStruct.list)
        {
            if (prevSCode.Equals(curri.subjectCode))
            {
                curri.subjectSemester = newSSemester;
                break;
            }
        }
    }

    public void UpdateSubjectYear(
        string prevSCode, string newSYear)
    {
        foreach (CurriculumEntry curri in
            curriculumStruct.list)
        {
            if (prevSCode.Equals(curri.subjectCode))
            {
                curri.subjectYear = newSYear;
                break;
            }
        }
    }

    public void UpdateSubjectRoom(
        string prevSCode, string newSRoom)
    {
        foreach (CurriculumEntry curri in
            curriculumStruct.list)
        {
            if (prevSCode.Equals(curri.subjectCode))
            {
                curri.subjectRoomCode = newSRoom;
                break;
            }
        }
    }
}

[System.Serializable] 
public class CurriculumStruct
{
    public List<CurriculumEntry> list = new();
}

[System.Serializable]
public class CurriculumEntry
{
    public string subjectCode;
    public string subjectName;
    public string subjectFaculty;
    public string subjectStudyProgram;
    public int subjectTotalCredit;
    public int subjectDuration;
    public string subjectPrerequisiteCode;
    public int subjectSemester;
    public string subjectYear;
    public string subjectRoomCode;
}
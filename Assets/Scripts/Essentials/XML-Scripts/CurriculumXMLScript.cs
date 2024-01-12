using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class CurriculumXMLScript : MonoBehaviour
{
    public static CurriculumXMLScript CurriculumXMLInstance { get; private set; }

    public string sCode, sName, sFaculty,
        sSturyProgram;
    public int sTotalCredit, sDuration;
    public string sPrerequisiteCode;
    public int sSemester;
    public string sYear, sRoomCode;


}

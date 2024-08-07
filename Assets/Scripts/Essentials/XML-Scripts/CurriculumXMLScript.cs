using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class CurriculumXMLScript : MonoBehaviour
{
    public static CurriculumXMLScript CurriculumXMLInstance { get; private set; }

    //public string sCode, sName, sFaculty,
    //    sSturyProgram;
    //public int sTotalCredit, sDuration;
    //public string sPrerequisiteCode;
    //public int sSemester;
    //public string sYear, sRoomCode;

    [SerializeField] private string filePath;

    private void Awake()
    {
        //filePath = Application.dataPath +
        //    @"/StreamingAssets/XML/Curriculum.xml";
        filePath = Application.persistentDataPath +
            @"/XML/Curriculum.xml";
        string dirPath = Application.persistentDataPath +
            @"/XML";

        GenerateDirectory(dirPath);

        CurriculumXMLInstance = this;
        if (!CheckFile())
        {
            InitializeFile();
        }

        DontDestroyOnLoad(gameObject);
    }

    public void GenerateDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
            Debug.Log("Directory Created!");
        }
    }

    public void InitializeFile()
    {
        XmlWriterSettings settings = new();
        settings.Encoding = System.Text.Encoding.GetEncoding("UTF-8");
        settings.Indent = true;
        settings.IndentChars = ("    ");
        settings.OmitXmlDeclaration = false;

        XmlWriter writer = XmlWriter.Create(filePath, settings);
        writer.WriteStartDocument();
        writer.WriteStartElement("Curriculum");
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Flush();
    }

    private bool CheckFile()
    {
        if (File.Exists(filePath))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetData()
    {
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            elmRoot.RemoveAll();
            xmlDoc.Save(filePath);
        }
        else
        {
            InitializeFile();
        }
    }

    public void SaveCurriculums()
    {
        ResetData();
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            foreach (CurriculumEntry curr in
                CurriculumManager.CurriculumManagerInstance
                .curriculumStruct.list)
            {
                Debug.Log("Current data: " +
                    curr.subjectCode);
                XmlElement elmNew = xmlDoc.CreateElement("CurriculumEntry");
                XmlElement currCode = xmlDoc.CreateElement("SubjectCode");
                XmlElement currName = xmlDoc.CreateElement("SubjectName");
                XmlElement currFaculty = xmlDoc.CreateElement("SubjectFaculty");
                XmlElement currSP = xmlDoc.CreateElement("SubjectStudyProgram");
                XmlElement currTC = xmlDoc.CreateElement("SubjectTotalCredit");
                XmlElement currDuration = xmlDoc.CreateElement("SubjectDuration");
                XmlElement currPC = xmlDoc.CreateElement("SubjectPrerequisiteCode");
                XmlElement currSemester = xmlDoc.CreateElement("SubjectSemester");
                XmlElement currYear = xmlDoc.CreateElement("SubjectYear");
                XmlElement currRC = xmlDoc.CreateElement("SubjectRoomCode");

                currCode.InnerText = curr.subjectCode;
                currName.InnerText = curr.subjectName;
                currFaculty.InnerText = curr.subjectFaculty;
                currSP.InnerText = curr.subjectStudyProgram;
                currTC.InnerText = curr.subjectTotalCredit.ToString();
                currDuration.InnerText = curr.subjectDuration.ToString();
                currPC.InnerText = curr.subjectPrerequisiteCode;
                currSemester.InnerText = curr.subjectSemester.ToString();
                currYear.InnerText = curr.subjectYear;
                currRC.InnerText = curr.subjectRoomCode;

                elmNew.AppendChild(currCode);
                elmNew.AppendChild(currName);
                elmNew.AppendChild(currFaculty);
                elmNew.AppendChild(currSP);
                elmNew.AppendChild(currTC);
                elmNew.AppendChild(currDuration);
                elmNew.AppendChild(currPC);
                elmNew.AppendChild(currSemester);
                elmNew.AppendChild(currYear);
                elmNew.AppendChild(currRC);
                elmRoot.AppendChild(elmNew);
            }

            xmlDoc.Save(filePath);
        }
        else
        {
            InitializeFile();
        }
    }

    public void LoadCurriculums()
    {
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);

            XmlNodeList CurrList = xmlDoc.GetElementsByTagName("CurriculumEntry");

            foreach (XmlNode CurrInfo in CurrList)
            {
                XmlNodeList CurrCon = CurrInfo.ChildNodes;
                CurriculumEntry newEntry = new();
                //string prevCurrCode = "";
                foreach (XmlNode CurrItems in CurrCon)
                {
                    switch (CurrItems.Name)
                    {
                        case "SubjectCode":
                            //prevCurrCode = CurrItems.InnerText;
                            newEntry.subjectCode = CurrItems.InnerText;
                            break;
                        case "SubjectName":
                            newEntry.subjectName = CurrItems.InnerText;
                            break;
                        case "SubjectFaculty":
                            newEntry.subjectFaculty = CurrItems.InnerText;
                            break;
                        case "SubjectStudyProgram":
                            newEntry.subjectStudyProgram = CurrItems.InnerText;
                            break;
                        case "SubjectTotalCredit":
                            int.TryParse(CurrItems.InnerText, out int TC);
                            int currTC = TC;
                            newEntry.subjectTotalCredit = currTC;
                            break;
                        case "SubjectDuration":
                            int.TryParse(CurrItems.InnerText, out int Durr);
                            int currDurr = Durr;
                            newEntry.subjectDuration = currDurr;
                            break;
                        case "SubjectPrerequisiteCode":
                            newEntry.subjectPrerequisiteCode = CurrItems.InnerText;
                            break;
                        case "SubjectSemester":
                            int.TryParse(CurrItems.InnerText, out int Sems);
                            int currSems = Sems;
                            newEntry.subjectSemester = currSems;
                            break;
                        case "SubjectYear":
                            newEntry.subjectYear = CurrItems.InnerText;
                            break;
                        case "SubjectRoomCode":
                            newEntry.subjectRoomCode = CurrItems.InnerText;
                            break;
                        default:
                            Debug.Log("End of the line");
                            break;
                    }
                }

                CurriculumManager.CurriculumManagerInstance.
                    curriculumStruct.list.Add(newEntry);
            }
        }
        else
        {
            InitializeFile();
        }
    }

    public void RemoveData(string targetCode)
    {
        if (CountData() >= 1)
        {
            XmlDocument xmlDoc = new();

            if (CheckFile())
            {
                xmlDoc.Load(filePath);

                XmlNodeList nodeList = xmlDoc.GetElementsByTagName("CurriculumEntry");

                foreach (XmlNode nodeInfo in nodeList)
                {
                    XmlNodeList nodeContent = nodeInfo.ChildNodes;

                    foreach (XmlNode nodeItem in nodeContent)
                    {
                        if (nodeItem.Name.Equals("SubjectCode"))
                        {
                            if (CodeMatches(nodeItem.InnerText, targetCode))
                            {
                                XmlNode parent = nodeInfo.ParentNode;

                                parent.RemoveChild(nodeInfo);

                                string newXML = xmlDoc.OuterXml;

                                Debug.Log(newXML);

                                xmlDoc.Save(filePath);
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("There's no file detected.");
            }
        }
        else
        {
            Debug.LogWarning("There's no data on the xml file.");
        }
    }

    public void ChangeCode(string targetCode, string data)
    {
        if (CountData() >= 1)
        {
            XmlDocument xmlDoc = new();

            if (CheckFile())
            {
                xmlDoc.Load(filePath);

                XmlNodeList nodeList = xmlDoc.GetElementsByTagName("CurriculumEntry");
                
                foreach (XmlNode nodeInfo in nodeList)
                {
                    XmlNodeList nodeContent = nodeInfo.ChildNodes;

                    foreach (XmlNode nodeItem in nodeContent)
                    {
                        if (nodeItem.Name.Equals("SubjectCode"))
                        {
                            if (CodeMatches(nodeItem.InnerText, targetCode))
                            {
                                nodeItem.InnerText = data;
                                break;
                            }
                        }
                    }
                }

                xmlDoc.Save(filePath);
            }
            else
            {
                Debug.LogWarning("There's no file detected.");
            }
        }
        else
        {
            Debug.LogWarning("There's no data on the xml file.");
        }
    }

    public void ChangeData(string targetCode, string targetItem, string data)
    {
        if (CountData() >= 1)
        {
            XmlDocument xmlDoc = new();

            if (CheckFile())
            {
                xmlDoc.Load(filePath);

                XmlNodeList nodeList = xmlDoc.GetElementsByTagName("CurriculumEntry");

                foreach (XmlNode nodeInfo in nodeList)
                {
                    XmlNodeList nodeContent = nodeInfo.ChildNodes;

                    string prevCode = "";
                    string targetedItem = targetItem;

                    foreach (XmlNode nodeItem in nodeContent)
                    {
                        if (nodeItem.Name.Equals("SubjectCode"))
                        {
                            if (nodeItem.InnerText.Equals(targetCode))
                                prevCode = nodeItem.InnerText;
                        }
                        else if (nodeItem.Name.Equals(targetItem))
                        {
                            if (CodeMatches(prevCode, targetCode))
                                nodeItem.InnerText = data;
                        }
                    }
                }

                xmlDoc.Save(filePath);
            }
            else
            {
                Debug.LogWarning("There's no file detected.");
            }
        } 
        else
        {
            Debug.LogWarning("There's no data on the xml file.");
        }
    }

    public int CountData()
    {
        int count = 0;
        XmlDocument xmlDoc = new();

        if (CheckFile())
        {
            xmlDoc.Load(filePath);

            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("CurriculumEntry");

            foreach (XmlNode nodeInfo in nodeList)
            {
                XmlNodeList nodeContent = nodeInfo.ChildNodes;

                foreach (XmlNode noteItem in nodeContent)
                {
                    switch (noteItem.Name)
                    {
                        case "SubjectCode":
                            count++;
                            break;

                        default:
                            Debug.Log("End of line.");
                            break;
                    }
                }
            }
        }

        return count;
    }

    public bool CodeMatches(string dataCode, string targetCode)
    {
        if (dataCode.Equals(targetCode))
            return true;

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class KHSTotalGradeXMLScript : MonoBehaviour
{
    public static KHSTotalGradeXMLScript Instance { get; private set; }
    [SerializeField] private string filePath;

    private void Awake()
    {
        filePath = Application.dataPath +
            "@/StreamingAssets/XML/KHSTotalGrade.xml";
        Instance = this;
        if (!CheckFile())
        {
            InitializeFile();
        }

        DontDestroyOnLoad(gameObject);
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
        writer.WriteStartElement("KHSTotalGrade");
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Flush();
    }

    public bool CheckFile()
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

    public void SaveKHSTGrades()
    {
        ResetData();
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            foreach (KHSTGEntry KTG in KHSTotalGradeManager.Instance.
                Struct.list)
            {
                Debug.Log("Current data: " + KTG.khsCode);
                XmlElement elmNew = xmlDoc.CreateElement("KHSTGradeEntry");
                XmlElement kTGCode = xmlDoc.CreateElement("KHSCode");
                XmlElement kTGSem = xmlDoc.CreateElement("KHSSemester");
                XmlElement kTG = xmlDoc.CreateElement("KHSTotalGrade");

                kTGCode.InnerText = KTG.khsCode;
                kTGSem.InnerText = KTG.khsSemester.ToString();
                kTG.InnerText = KTG.khsTotalGrade.ToString();

                elmNew.AppendChild(kTGCode);
                elmNew.AppendChild(kTGSem);
                elmNew.AppendChild(kTG);
                elmRoot.AppendChild(elmNew);
            }

            xmlDoc.Save(filePath);
        }
        else
        {
            InitializeFile();
        }
    }

    public void LoadKHSTGrades()
    {
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);

            XmlNodeList kList = xmlDoc.GetElementsByTagName(
                "KHSTGradeEntry");

            foreach (XmlNode kTGInfo in kList)
            {
                XmlNodeList kTGCon = kTGInfo.ChildNodes;
                KHSTGEntry newEntry = new();
                foreach (XmlNode kTGItems in kTGCon)
                {
                    switch (kTGItems.Name)
                    {
                        case "KHSCode":
                            newEntry.khsCode = kTGItems.InnerText;
                            break;
                        case "KHSSemester":
                            int.TryParse(kTGItems.InnerText, out int x);
                            int sem = x;
                            newEntry.khsSemester = sem;
                            break;
                        case "KHSTotalGrade":
                            double.TryParse(kTGItems.InnerText, out double y);
                            double totGrade = y;
                            newEntry.khsTotalGrade = totGrade;
                            break;
                        default:
                            Debug.Log("End of the data");
                            break;
                    }
                }

                KHSTotalGradeManager.Instance.Struct.list.Add(newEntry);
            }
        }
        else
        {
            InitializeFile();
        }
    }
}

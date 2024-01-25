using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class KRSInfoXMLScript : MonoBehaviour
{
    public static KRSInfoXMLScript Instance { get; private set; }
    [SerializeField] private string filePath;

    private void Awake()
    {
        filePath = Application.dataPath +
            "@/StreamingAssets/XML/KRSInfo.xml";
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
        writer.WriteStartElement("KRSInfo");
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

    public void SaveKRSInfos()
    {
        ResetData();
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            foreach (KRSIEntry KIE in KRSInfoManager.Instance.
                Struct.list)
            {
                Debug.Log("Current data: " + KIE.krsInfoCode);
                XmlElement elmNew = xmlDoc.CreateElement("KRSInfoEntry");
                XmlElement kICode = xmlDoc.CreateElement("KRSInfoCode");
                XmlElement kINumber = xmlDoc.CreateElement("KRSInfoNumber");
                XmlElement kISCode = xmlDoc.CreateElement("KRSInfoSubjectCode");
                XmlElement kISName = xmlDoc.CreateElement("KRSInfoSubjectName");
                XmlElement kITCredit = xmlDoc.CreateElement("KRSInfoTotalCredit");
                XmlElement kCode = xmlDoc.CreateElement("KRSCode");

                kICode.InnerText = KIE.krsInfoCode;
                kINumber.InnerText = KIE.krsInfoNumber.ToString();
                kISCode.InnerText = KIE.krsInfoSubjectCode;
                kISName.InnerText = KIE.krsInfoSubjectName;
                kITCredit.InnerText = KIE.krsInfoTotalCredit.ToString();
                kCode.InnerText = KIE.krsCode;

                elmNew.AppendChild(kICode);
                elmNew.AppendChild(kINumber);
                elmNew.AppendChild(kISCode);
                elmNew.AppendChild(kISName);
                elmNew.AppendChild(kITCredit);
                elmNew.AppendChild(kCode);
                elmRoot.AppendChild(elmNew);
            }

            xmlDoc.Save(filePath);
        }
        else
        {
            InitializeFile();
        }
    }

    public void LoadKRSInfos()
    {

    }
}

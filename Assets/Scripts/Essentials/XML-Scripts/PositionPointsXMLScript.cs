using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class PositionPointsXMLScript : MonoBehaviour
{
    public static PositionPointsXMLScript Instance { get; private set; }
    [SerializeField] private string filePath;

    private void Awake()
    {
        filePath = Application.dataPath +
            "@/StreamingAssets/XML/PositionPoints.xml";
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
        writer.WriteStartElement("PositionPoints");
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

    public void SavePositionPoints()
    {
        ResetData();
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            foreach (PositionPEntry PPE in PositionPointsManager.Instance.
                Struct.list)
            {
                Debug.Log("Current data: " + PPE.positionPCode);
                XmlElement elmNew = xmlDoc.CreateElement("PositionPointsEntry");
                XmlElement pPCode = xmlDoc.CreateElement("PositionPointsCode");
                XmlElement pPSCode = xmlDoc.CreateElement("PositionPointsSceneCode");
                XmlElement pPXPos = xmlDoc.CreateElement("PositionPointsXPos");
                XmlElement pPYPos = xmlDoc.CreateElement("PositionPointsYPos");
                XmlElement pPZPos = xmlDoc.CreateElement("PositionPointsZPos");

                pPCode.InnerText = PPE.positionPCode;
                pPSCode.InnerText = PPE.positionPSCode;
                pPXPos.InnerText = PPE.positionPXPos.ToString();
                pPYPos.InnerText = PPE.positionPYPos.ToString();
                pPZPos.InnerText = PPE.positionPZPos.ToString();

                elmNew.AppendChild(pPCode);
                elmNew.AppendChild(pPSCode);
                elmNew.AppendChild(pPXPos);
                elmNew.AppendChild(pPYPos);
                elmNew.AppendChild(pPZPos);
                elmRoot.AppendChild(elmNew);
            }

            xmlDoc.Save(filePath);
        }
        else
        {
            InitializeFile();
        }
    }

    public void LoadPositionPoints()
    {
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);

            XmlNodeList pPList = xmlDoc.GetElementsByTagName(
                "PositionPointsEntry");
            
            foreach (XmlNode pPInfo in pPList)
            {
                XmlNodeList pPCon = pPInfo.ChildNodes;
                PositionPEntry newEntry = new();
                foreach (XmlNode pPItems in pPCon)
                {
                    switch (pPItems.Name)
                    {
                        case "PositionPointsCode":
                            newEntry.positionPCode = pPItems.InnerText;
                            break;
                        case "PositionPointsSceneCode":
                            newEntry.positionPSCode = pPItems.InnerText;
                            break;
                        case "PositionPointsXPos":
                            double.TryParse(pPItems.InnerText, out double x);
                            double pX = x;
                            newEntry.positionPXPos = pX;
                            break;
                        case "PositionPointsYPos":
                            double.TryParse(pPItems.InnerText, out double y);
                            double pY = y;
                            newEntry.positionPYPos = pY;
                            break;
                        case "PositionPointsZPos":
                            double.TryParse(pPItems.InnerText, out double z);
                            double pZ = z;
                            newEntry.positionPZPos = pZ;
                            break;
                        default:
                            Debug.Log("End of the data");
                            break;
                    }
                }

                PositionPointsManager.Instance.Struct.list.Add(newEntry);
            }
        }
        else
        {
            InitializeFile();
        }
    }
}

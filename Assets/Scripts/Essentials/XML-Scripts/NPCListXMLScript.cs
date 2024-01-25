using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class NPCListXMLScript : MonoBehaviour
{
    public static NPCListXMLScript Instance { get; private set; }
    [SerializeField] private string filePath;

    private void Awake()
    {
        filePath = Application.dataPath +
            "@/StreamingAssets/XML/NPCList.xml";
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
        writer.WriteStartElement("NPCList");
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

    public void SaveNPCLists()
    {
        ResetData();
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            foreach (NPCLEntry NLE in NPCListManager.Instance.
                Struct.list)
            {
                Debug.Log("Current data: " + NLE.npcLCode);
                XmlElement elmNew = xmlDoc.CreateElement("NPCListEntry");
                XmlElement nLCode = xmlDoc.CreateElement("NPCListCode");
                XmlElement nLName = xmlDoc.CreateElement("NPCListName");
                XmlElement nLMPath = xmlDoc.CreateElement("NPCListModelPath");
                XmlElement nLType = xmlDoc.CreateElement("NPCListType");

                nLCode.InnerText = NLE.npcLCode;
                nLName.InnerText = NLE.npcLName;
                nLMPath.InnerText = NLE.npcLMPath;
                nLType.InnerText = NLE.npcLType;

                elmNew.AppendChild(nLCode);
                elmNew.AppendChild(nLName);
                elmNew.AppendChild(nLMPath);
                elmNew.AppendChild(nLType);
                elmRoot.AppendChild(elmNew);
            }

            xmlDoc.Save(filePath);
            
        }
        else
        {
            InitializeFile();
        }
    }

    public void LoadNPCLists()
    {
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);

            XmlNodeList nLList = xmlDoc.GetElementsByTagName(
                "NPCListEntry");

            foreach (XmlNode nLInfo in nLList)
            {
                XmlNodeList nLCon = nLInfo.ChildNodes;
                NPCLEntry newEntry = new();
                foreach (XmlNode nLItems in nLCon)
                {
                    switch (nLItems.Name)
                    {
                        case "NPCListCode":
                            newEntry.npcLCode = nLItems.InnerText;
                            break;
                        case "NPCListName":
                            newEntry.npcLName = nLItems.InnerText;
                            break;
                        case "NPCListModelPath":
                            newEntry.npcLMPath = nLItems.InnerText;
                            break;
                        case "NPCListType":
                            newEntry.npcLType = nLItems.InnerText;
                            break;
                        default:
                            Debug.Log("End of the data");
                            break;
                    }
                }
                NPCListManager.Instance.Struct.list.Add(newEntry);
            }
        }
        else
        {
            InitializeFile();
        }
    }
}

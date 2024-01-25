using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class InventoryDataXMLScript : MonoBehaviour
{
    public static InventoryDataXMLScript Instance { get; private set; }
    [SerializeField] private string filePath;

    private void Awake()
    {
        filePath = Application.dataPath +
            "@/StreamingAssets/XML/InventoryData.xml";
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
        writer.WriteStartElement("InventoryData");
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

    public void SaveInventoryDatas()
    {
        ResetData();
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            foreach (InventoryDataEntry IDE in InventoryDataManager.Instance
                .Struct.list)
            {
                Debug.Log("Current data: " + IDE.inventoryID);
                XmlElement elmNew = xmlDoc.CreateElement("InventoryDataEntry");
                XmlElement iID = xmlDoc.CreateElement("InventoryID");
                XmlElement iName = xmlDoc.CreateElement("InventoryName");
                XmlElement iType = xmlDoc.CreateElement("InventoryType");
                XmlElement iWeight = xmlDoc.CreateElement("InventoryWeight");
                XmlElement iUCode = xmlDoc.CreateElement("InventoryUsableCode");

                iID.InnerText = IDE.inventoryID;
                iName.InnerText = IDE.inventoryName;
                iType.InnerText = IDE.inventoryType;
                iWeight.InnerText = IDE.inventoryWeight.ToString();
                iUCode.InnerText = IDE.inventoryUsableCode.ToString();

                elmNew.AppendChild(iID);
                elmNew.AppendChild(iName);
                elmNew.AppendChild(iType);
                elmNew.AppendChild(iWeight);
                elmNew.AppendChild(iUCode);
                elmRoot.AppendChild(elmNew);
            }

            xmlDoc.Save(filePath);
        }
        else
        {
            InitializeFile();
        }
    }

    public void LoadInventoryDatas()
    {
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);

            XmlNodeList iList = xmlDoc.GetElementsByTagName(
                "InventoryDataEntry");

            foreach(XmlNode iInfo in iList)
            {
                XmlNodeList iCon = iInfo.ChildNodes;
                InventoryDataEntry newEntry = new();
                foreach (XmlNode iItems in iCon)
                {
                    switch (iItems.Name)
                    {
                        case "InventoryID":
                            newEntry.inventoryID = iItems.InnerText;
                            break;
                        case "InventoryName":
                            newEntry.inventoryName = iItems.InnerText;
                            break;
                        case "InventoryType":
                            newEntry.inventoryType = iItems.InnerText;
                            break;
                        case "InventoryWeight":
                            double.TryParse(iItems.InnerText, out double x);
                            double weight = x;
                            newEntry.inventoryWeight = weight;
                            break;
                        case "InventoryUsableCode":
                            int.TryParse(iItems.InnerText, out int y);
                            int uCode = y;
                            newEntry.inventoryUsableCode = uCode;
                            break;
                        default:
                            Debug.Log("End of the data");
                            break;
                    }
                }

                InventoryDataManager.Instance.Struct.list.Add(newEntry);
            }
        }
        else
        {
            InitializeFile();
        }
    }
}

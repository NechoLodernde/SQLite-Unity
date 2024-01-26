using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class AchievementListXMLScript : MonoBehaviour
{
    public static AchievementListXMLScript Instance { get; private set; }
    [SerializeField] private string filePath;

    private void Awake()
    {
        filePath = Application.dataPath +
            "@/StreamingAssets/XML/AchievementList.xml";
        Instance = this;
        if (!CheckFile())
        {
            InitializeFile();
        }

        DontDestroyOnLoad(gameObject);
    }

    public void InitializeFile()
    {
        XmlWriterSettings settings = new()
        {
            Encoding = System.Text.Encoding.GetEncoding("UTF-8"),
            Indent = true,
            IndentChars = ("    "),
            OmitXmlDeclaration = false
        };

        XmlWriter writer = XmlWriter.Create(filePath, settings);
        writer.WriteStartDocument();
        writer.WriteStartElement("AchievementList");
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

    public void SaveAchievementLists()
    {
        ResetData();
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            foreach (AchievementLEntry ALE in AchievementListManager.
                Instance.Struct.list)
            {
                Debug.Log("Current data: " + ALE.achievementID);
                XmlElement elmNew = xmlDoc.CreateElement("AchievementListEntry");
                XmlElement aID = xmlDoc.CreateElement("AchievementID");
                XmlElement aNum = xmlDoc.CreateElement("AchievementNumber");
                XmlElement aName = xmlDoc.CreateElement("AchievementName");
                XmlElement aDesc = xmlDoc.CreateElement("AchievementDescription");
                XmlElement aUCode = xmlDoc.CreateElement("AchievementUnlockCode");
                XmlElement uStatus = xmlDoc.CreateElement("UnlockStatus");

                aID.InnerText = ALE.achievementID;
                aNum.InnerText = ALE.achievementNumber.ToString();
                aName.InnerText = ALE.achievementName;
                aDesc.InnerText = ALE.achievementDesc;
                aUCode.InnerText = ALE.achievementUCode;
                uStatus.InnerText = ALE.unlockStatus.ToString();

                elmNew.AppendChild(aID);
                elmNew.AppendChild(aNum);
                elmNew.AppendChild(aName);
                elmNew.AppendChild(aDesc);
                elmNew.AppendChild(aUCode);
                elmNew.AppendChild(uStatus);
                elmRoot.AppendChild(elmNew);
            }

            xmlDoc.Save(filePath);
        }
        else
        {
            InitializeFile();
        }
    }

    public void LoadAchievementLists()
    {
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);

            XmlNodeList aLList = xmlDoc.GetElementsByTagName(
                "AchievementListEntry");

            foreach (XmlNode aLInfo in aLList)
            {
                XmlNodeList aLCon = aLInfo.ChildNodes;
                AchievementLEntry newEntry = new();
                foreach (XmlNode aLItems in aLCon)
                {
                    switch (aLItems.Name)
                    {
                        case "AchievementID":
                            newEntry.achievementID = aLItems.InnerText;
                            break;
                        case "AchievementNumber":
                            int.TryParse(aLItems.InnerText, out int x);
                            int aNum = x;
                            newEntry.achievementNumber = aNum;
                            break;
                        case "AchievementName":
                            newEntry.achievementName = aLItems.InnerText;
                            break;
                        case "AchievementDescription":
                            newEntry.achievementDesc = aLItems.InnerText;
                            break;
                        case "AchievementUnlockCode":
                            newEntry.achievementUCode = aLItems.InnerText;
                            break;
                        case "UnlockStatus":
                            int.TryParse(aLItems.InnerText, out int y);
                            int uStatus = y;
                            newEntry.unlockStatus = uStatus;
                            break;
                        default:
                            Debug.Log("End of the data");
                            break;
                    }
                }

                AchievementListManager.Instance.Struct.list.Add(newEntry);
            }
        }
        else
        {
            InitializeFile();
        }
    }
}

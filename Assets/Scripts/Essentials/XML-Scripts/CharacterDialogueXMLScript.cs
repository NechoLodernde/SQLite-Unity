using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class CharacterDialogueXMLScript : MonoBehaviour
{
    public static CharacterDialogueXMLScript Instance { get; private set; }
    [SerializeField] private string filePath;

    private void Awake()
    {
        filePath = Application.dataPath + 
            "@/StreamingAssets/XML/CharacterDialogue.xml";
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
        writer.WriteStartElement("CharacterDialogue");
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

    public void SaveCharDialogues()
    {
        ResetData();
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            foreach (CharDialEntry cD in CharacterDialogueManager
                .Instance.Struct.list)
            {
                Debug.Log("Current data: " + cD.charDialCode);
                XmlElement elmNew = xmlDoc.CreateElement("CharacterDialogueEntry");
                XmlElement cDCode = xmlDoc.CreateElement("CharacterDialogueCode");
                XmlElement cDName = xmlDoc.CreateElement("CharacterDialogueName");
                XmlElement cDTarget = xmlDoc.CreateElement("CharacterDialogueTarget");
                XmlElement cDText = xmlDoc.CreateElement("CharacterDialogueText");

                cDCode.InnerText = cD.charDialCode;
                cDName.InnerText = cD.charDialName;
                cDTarget.InnerText = cD.charDialTarget;
                cDText.InnerText = cD.charDialText;

                elmNew.AppendChild(cDCode);
                elmNew.AppendChild(cDName);
                elmNew.AppendChild(cDTarget);
                elmNew.AppendChild(cDText);
                elmRoot.AppendChild(elmNew);
            }

            xmlDoc.Save(filePath);
        }
        else
        {
            InitializeFile();
        }
    }

    public void LoadCharDialogues()
    {
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);

            XmlNodeList cDList = xmlDoc.GetElementsByTagName("CharacterDialogueEntry");

            foreach (XmlNode cDInfo in cDList)
            {
                XmlNodeList cDCon = cDInfo.ChildNodes;
                CharDialEntry newEntry = new();
                foreach (XmlNode cDItems in cDCon)
                {
                    switch (cDItems.Name)
                    {
                        case "CharacterDialogueCode":
                            newEntry.charDialCode = cDItems.InnerText;
                            break;
                        case "CharacterDialogueName":
                            newEntry.charDialName = cDItems.InnerText;
                            break;
                        case "CharacterDialogueTarget":
                            newEntry.charDialTarget = cDItems.InnerText;
                            break;
                        case "CharacterDialogueText":
                            newEntry.charDialText = cDItems.InnerText;
                            break;
                        default:
                            Debug.Log("End of the data");
                            break;
                    }
                }
                CharacterDialogueManager.Instance.Struct.list.Add(newEntry);
            }
        }
        else
        {
            InitializeFile();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class DialogueXMLScript : MonoBehaviour
{
    public static DialogueXMLScript Instance { get; private set; }
    [SerializeField] private string filePath;

    private void Awake()
    {
        filePath = Application.dataPath +
            "@/StreamingAssets/XML/Dialogue.xml";
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
        writer.WriteStartElement("Dialogue");
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

    public void SaveDialogues()
    {
        ResetData();
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            foreach (DialEntry dial in DialogueManager.Instance.
                Struct.list)
            {
                Debug.Log("Current data: " + dial.dialCode);
                XmlElement elmNew = xmlDoc.CreateElement("DialogueEntry");
                XmlElement dCode = xmlDoc.CreateElement("DialogueCode");
                XmlElement dCName = xmlDoc.CreateElement("DialogueCharName");
                XmlElement dText = xmlDoc.CreateElement("DialogueText");
                XmlElement cDCode = xmlDoc.CreateElement("CharacterDialogueCode");

                dCode.InnerText = dial.dialCode;
                dCName.InnerText = dial.dialCharName;
                dText.InnerText = dial.dialText;
                cDCode.InnerText = dial.charDialCode;

                elmNew.AppendChild(dCode);
                elmNew.AppendChild(dCName);
                elmNew.AppendChild(dText);
                elmNew.AppendChild(cDCode);
                elmRoot.AppendChild(cDCode);
            }

            xmlDoc.Save(filePath);
        }
        else
        {
            InitializeFile();
        }
    }

    public void LoadDialogues()
    {
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);

            XmlNodeList dList = xmlDoc.GetElementsByTagName("DialogueEntry");

            foreach (XmlNode dInfo in dList)
            {
                XmlNodeList dCon = dInfo.ChildNodes;
                DialEntry newEntry = new();
                foreach (XmlNode dItems in dCon)
                {
                    switch (dItems.Name)
                    {
                        case "DialogueCode":
                            newEntry.dialCode = dItems.InnerText;
                            break;
                        case "DialogueCharName":
                            newEntry.dialCharName = dItems.InnerText;
                            break;
                        case "DialogueText":
                            newEntry.dialText = dItems.InnerText;
                            break;
                        case "CharacterDialogueCode":
                            newEntry.charDialCode = dItems.InnerText;
                            break;
                        default:
                            Debug.Log("End of the data");
                            break;
                    }
                }
                DialogueManager.Instance.Struct.list.Add(newEntry);
            }
        }
        else
        {
            InitializeFile();
        }
    }
}

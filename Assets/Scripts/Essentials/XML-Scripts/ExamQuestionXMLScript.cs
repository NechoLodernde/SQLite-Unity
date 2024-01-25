using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class ExamQuestionXMLScript : MonoBehaviour
{
    public static ExamQuestionXMLScript Instance { get; private set; }
    [SerializeField] private string filePath;

    private void Awake()
    {
        filePath = Application.dataPath +
            "@/StreamingAssets/XML/ExamQuestion.xml";
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
        writer.WriteStartElement("ExamQuestion");
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

    public void SaveExamQuestions()
    {
        ResetData();
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            foreach (ExamQEntry EQE in ExamQuestionManager.Instance
                .Struct.list)
            {
                Debug.Log("Current data: " + EQE.examQCode);
                XmlElement elmNew = xmlDoc.CreateElement("ExamQuestionEntry");
                XmlElement eQCode = xmlDoc.CreateElement("ExamQuestionCode");
                XmlElement eQTerm = xmlDoc.CreateElement("ExamQuestionTerm");
                XmlElement eQPhase = xmlDoc.CreateElement("ExamQuestionPhase");
                XmlElement eQNum = xmlDoc.CreateElement("ExamQuestionNumber");
                XmlElement eQDesc = xmlDoc.CreateElement("ExamQuestionDescription");
                XmlElement eQChoi1 = xmlDoc.CreateElement("ExamQuestionChoice1");
                XmlElement eQChoi2 = xmlDoc.CreateElement("ExamQuestionChoice2");
                XmlElement eQChoi3 = xmlDoc.CreateElement("ExamQuestionChoice3");
                XmlElement eQChoi4 = xmlDoc.CreateElement("ExamQuestionChoice4");
                XmlElement eQAns = xmlDoc.CreateElement("ExamQuestionAnswer");
                XmlElement subCode = xmlDoc.CreateElement("SubjectCode");

                eQCode.InnerText = EQE.examQCode;
                eQTerm.InnerText = EQE.examQTerm;
                eQPhase.InnerText = EQE.examQPhase;
                eQNum.InnerText = EQE.examQNumber.ToString();
                eQDesc.InnerText = EQE.examQDescription;
                eQChoi1.InnerText = EQE.examQChoice1;
                eQChoi2.InnerText = EQE.examQChoice2;
                eQChoi3.InnerText = EQE.examQChoice3;
                eQChoi4.InnerText = EQE.examQChoice4;
                eQAns.InnerText = EQE.examQAnswer.ToString();
                subCode.InnerText = EQE.subjectCode;

                elmNew.AppendChild(eQCode);
                elmNew.AppendChild(eQTerm);
                elmNew.AppendChild(eQPhase);
                elmNew.AppendChild(eQNum);
                elmNew.AppendChild(eQDesc);
                elmNew.AppendChild(eQChoi1);
                elmNew.AppendChild(eQChoi2);
                elmNew.AppendChild(eQChoi3);
                elmNew.AppendChild(eQChoi4);
                elmNew.AppendChild(eQAns);
                elmNew.AppendChild(subCode);
                elmRoot.AppendChild(elmNew);
            }

            xmlDoc.Save(filePath);
        }
        else
        {
            InitializeFile();
        }
    }

    public void LoadExamQuestions()
    {
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);

            XmlNodeList eList = xmlDoc.GetElementsByTagName(
                "ExamQuestionEntry");

            foreach (XmlNode eInfo in eList)
            {
                XmlNodeList eCon = eInfo.ChildNodes;
                ExamQEntry newEntry = new();
                foreach (XmlNode eItems in eCon)
                {
                    switch (eItems.Name)
                    {
                        case "ExamQuestionCode":
                            newEntry.examQCode = eItems.InnerText;
                            break;
                        case "ExamQuestionTerm":
                            newEntry.examQTerm = eItems.InnerText;
                            break;
                        case "ExamQuestionPhase":
                            newEntry.examQPhase = eItems.InnerText;
                            break;
                        case "ExamQuestionNumber":
                            int.TryParse(eItems.InnerText, out int y);
                            int num = y;
                            newEntry.examQNumber = num;
                            break;
                        case "ExamQuestionDescription":
                            newEntry.examQDescription = eItems.InnerText;
                            break;
                        case "ExamQuestionChoice1":
                            newEntry.examQChoice1 = eItems.InnerText;
                            break;
                        case "ExamQuestionChoice2":
                            newEntry.examQChoice2 = eItems.InnerText;
                            break;
                        case "ExamQuestionChoice3":
                            newEntry.examQChoice3 = eItems.InnerText;
                            break;
                        case "ExamQuestionChoice4":
                            newEntry.examQChoice4 = eItems.InnerText;
                            break;
                        case "ExamQuestionAnswer":
                            int.TryParse(eItems.InnerText, out int x);
                            int ans = x;
                            newEntry.examQAnswer = ans;
                            break;
                        case "SubjectCode":
                            newEntry.subjectCode = eItems.InnerText;
                            break;
                        default:
                            Debug.Log("End of the data");
                            break;
                    }
                }

                ExamQuestionManager.Instance.Struct.list.Add(newEntry);
            }
        }
        else
        {
            InitializeFile();
        }
    }
}

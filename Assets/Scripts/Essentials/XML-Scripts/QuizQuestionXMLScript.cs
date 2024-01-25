using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class QuizQuestionXMLScript : MonoBehaviour
{
    public static QuizQuestionXMLScript Instance { get; private set; }
    [SerializeField] private string filePath;

    private void Awake()
    {
        filePath = Application.dataPath +
            "@/StreamingAssets/XML/QuizQuestion.xml";
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
        writer.WriteStartElement("QuizQuestion");
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

    public void SaveQuizQuestions()
    {
        ResetData();
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            foreach (QuizQEntry QQE in QuizQuestionManager.Instance
                .Struct.list)
            {
                Debug.Log("Current data: " + QQE.quizQCode);
                XmlElement elmNew = xmlDoc.CreateElement("QuizQuestionEntry");
                XmlElement qQCode = xmlDoc.CreateElement("QuizQuestionCode");
                XmlElement qQPhase = xmlDoc.CreateElement("QuizQuestionPhase");
                XmlElement qQNum = xmlDoc.CreateElement("QuizQuestionNumber");
                XmlElement qQDesc = xmlDoc.CreateElement("QuizQuestionDescription");
                XmlElement qQChoi1 = xmlDoc.CreateElement("QuizQuestionChoice1");
                XmlElement qQChoi2 = xmlDoc.CreateElement("QuizQuestionChoice2");
                XmlElement qQChoi3 = xmlDoc.CreateElement("QuizQuestionChoice3");
                XmlElement qQChoi4 = xmlDoc.CreateElement("QuizQuestionChoice4");
                XmlElement qQAns = xmlDoc.CreateElement("QuizQuestionAnswer");
                XmlElement subCode = xmlDoc.CreateElement("SubjectCode");

                qQCode.InnerText = QQE.quizQCode;
                qQPhase.InnerText = QQE.quizQPhase;
                qQNum.InnerText = QQE.quizQNumber.ToString();
                qQDesc.InnerXml = QQE.quizQDescription;
                qQChoi1.InnerText = QQE.quizQChoice1;
                qQChoi2.InnerText = QQE.quizQChoice2;
                qQChoi3.InnerText = QQE.quizQChoice3;
                qQChoi4.InnerText = QQE.quizQChoice4;
                qQAns.InnerText = QQE.quizQAnswer.ToString();
                subCode.InnerText = QQE.subjectCode;

                elmNew.AppendChild(qQCode);
                elmNew.AppendChild(qQPhase);
                elmNew.AppendChild(qQNum);
                elmNew.AppendChild(qQDesc);
                elmNew.AppendChild(qQChoi1);
                elmNew.AppendChild(qQChoi2);
                elmNew.AppendChild(qQChoi3);
                elmNew.AppendChild(qQChoi4);
                elmNew.AppendChild(qQAns);
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

    public void LoadQuizQuestions()
    {
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);

            XmlNodeList qList = xmlDoc.GetElementsByTagName(
                "QuizQuestionEntry");

            foreach (XmlNode qInfo in qList)
            {
                XmlNodeList qCon = qInfo.ChildNodes;
                QuizQEntry newEntry = new();
                foreach (XmlNode qItems in qCon)
                {
                    switch (qItems.Name)
                    {
                        case "QuizQuestionCode":
                            newEntry.quizQCode = qItems.InnerText;
                            break;
                        case "QuizQuestionPhase":
                            newEntry.quizQPhase = qItems.InnerText;
                            break;
                        case "QuizQuestionNumber":
                            int.TryParse(qItems.InnerText, out int x);
                            int num = x;
                            newEntry.quizQNumber = num;
                            break;
                        case "QuizQuestionDescription":
                            newEntry.quizQDescription = qItems.InnerText;
                            break;
                        case "QuizQuestionChoice1":
                            newEntry.quizQChoice1 = qItems.InnerText;
                            break;
                        case "QuizQuestionChoice2":
                            newEntry.quizQChoice2 = qItems.InnerText;
                            break;
                        case "QuizQuestionChoice3":
                            newEntry.quizQChoice3 = qItems.InnerText;
                            break;
                        case "QuizQuestionChoice4":
                            newEntry.quizQChoice4 = qItems.InnerText;
                            break;
                        case "QuizQuestionAnswer":
                            int.TryParse(qItems.InnerText, out int y);
                            int ans = y;
                            newEntry.quizQAnswer = ans;
                            break;
                        case "SubjectCode":
                            newEntry.subjectCode = qItems.InnerText;
                            break;
                        default:
                            Debug.Log("End of the data");
                            break;
                    }
                }

                QuizQuestionManager.Instance.Struct.list.Add(newEntry);
            }
        }
        else
        {
            InitializeFile();
        }
    }
}

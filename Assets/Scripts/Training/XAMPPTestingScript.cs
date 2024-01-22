using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using System.Xml;
using System.IO;

public class XAMPPTestingScript : MonoBehaviour
{
    public static XAMPPTestingScript instance { get; private set; }
    public CurrStruct curStruct;

    [SerializeField] private string filePath;
    [SerializeField] private string localPath;
    
    private void Awake()
    {
        filePath = "http://127.0.0.1/yarlisim/Curriculum.xml";
        localPath = Application.dataPath + @"/StreamingAssets/XML/Xampp.xml";
        instance = this;
        if (!CheckFile())
        {
            InitializeFile();
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //StartCoroutine(GetRequest("http://localhost/yarlisim/Curriculum.xml"));
        //StartCoroutine(GetRequest(filePath));
        LoadCurriculums();
    }
    
    public void InitializeFile()
    {
        XmlWriterSettings settings = new();
        settings.Encoding = System.Text.Encoding.GetEncoding("UTF-8");
        settings.Indent = true;
        settings.IndentChars = ("    ");
        settings.OmitXmlDeclaration = false;

        XmlWriter writer = XmlWriter.Create(localPath, settings);
        writer.WriteStartDocument();
        writer.WriteStartElement("Curriculum");
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Flush();
    }

    private bool CheckFile()
    {
        if (File.Exists(localPath))
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
            xmlDoc.Load(localPath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            elmRoot.RemoveAll();
            xmlDoc.Save(localPath);
        }
        else
        {
            InitializeFile();
        }
    }

    public void SaveCurriculums()
    {
        ResetData();
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(localPath);
            XmlElement elmRoot = xmlDoc.DocumentElement;
            foreach ( CurrEntry cur in instance.curStruct.list)
            {
                Debug.Log("Current data: " + cur.subCode);
                XmlElement elmNew = xmlDoc.CreateElement("CurriculumEntry");
                XmlElement currCode = xmlDoc.CreateElement("SubjectCode");
                XmlElement currName = xmlDoc.CreateElement("SubjectName");
                XmlElement currFaculty = xmlDoc.CreateElement("SubjectFaculty");
                XmlElement currSP = xmlDoc.CreateElement("SubjectStudyProgram");
                XmlElement currTC = xmlDoc.CreateElement("SubjectTotalCredit");
                XmlElement currDuration = xmlDoc.CreateElement("SubjectDuration");
                XmlElement currPC = xmlDoc.CreateElement("SubjectPrerequisiteCode");
                XmlElement currSemester = xmlDoc.CreateElement("SubjectSemester");
                XmlElement currYear = xmlDoc.CreateElement("SubjectYear");
                XmlElement currRC = xmlDoc.CreateElement("SubjectRoomCode");

                currCode.InnerText = cur.subCode;
                currName.InnerText = cur.subName;
                currFaculty.InnerText = cur.subFac;
                currSP.InnerText = cur.subStud;
                currTC.InnerText = cur.subToCre.ToString();
                currDuration.InnerText = cur.subDur.ToString();
                currPC.InnerText = cur.subPreq;
                currSemester.InnerText = cur.subSem.ToString();
                currYear.InnerText = cur.subYear;
                currRC.InnerText = cur.subRoom;

                elmNew.AppendChild(currCode);
                elmNew.AppendChild(currName);
                elmNew.AppendChild(currFaculty);
                elmNew.AppendChild(currSP);
                elmNew.AppendChild(currTC);
                elmNew.AppendChild(currDuration);
                elmNew.AppendChild(currPC);
                elmNew.AppendChild(currSemester);
                elmNew.AppendChild(currYear);
                elmNew.AppendChild(currRC);
                elmRoot.AppendChild(elmNew);
            }
            xmlDoc.Save(localPath);
        }
        else
        {
            InitializeFile();
        }
    }

    public void LoadCurriculums()
    {
        XmlDocument xmlDoc = new();
        if (CheckFile())
        {
            xmlDoc.Load(filePath);

            XmlNodeList CurrList = xmlDoc.GetElementsByTagName("CurriculumEntry");

            foreach (XmlNode CurrInfo in CurrList)
            {
                XmlNodeList CurrCon = CurrInfo.ChildNodes;
                CurrEntry newEntry = new();
                foreach (XmlNode CurrItems in CurrCon)
                {
                    switch (CurrItems.Name)
                    {
                        case "SubjectCode":
                            //prevCurrCode = CurrItems.InnerText;
                            newEntry.subCode = CurrItems.InnerText;
                            break;
                        case "SubjectName":
                            newEntry.subName = CurrItems.InnerText;
                            break;
                        case "SubjectFaculty":
                            newEntry.subFac = CurrItems.InnerText;
                            break;
                        case "SubjectStudyProgram":
                            newEntry.subStud = CurrItems.InnerText;
                            break;
                        case "SubjectTotalCredit":
                            int.TryParse(CurrItems.InnerText, out int TC);
                            int currTC = TC;
                            newEntry.subToCre = currTC;
                            break;
                        case "SubjectDuration":
                            int.TryParse(CurrItems.InnerText, out int Durr);
                            int currDurr = Durr;
                            newEntry.subDur= currDurr;
                            break;
                        case "SubjectPrerequisiteCode":
                            newEntry.subPreq = CurrItems.InnerText;
                            break; 
                        case "SubjectSemester":
                            int.TryParse(CurrItems.InnerText, out int Sems);
                            int currSems = Sems;
                            newEntry.subSem = currSems;
                            break;
                        case "SubjectYear":
                            newEntry.subYear = CurrItems.InnerText;
                            break;
                        case "SubjectRoomCode":
                            newEntry.subRoom = CurrItems.InnerText;
                            break;
                        default:
                            Debug.Log("End of the line");
                            break;
                    }
                }
                instance.curStruct.list.Add(newEntry);
            }
        }
        else
        {
            InitializeFile();
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(filePath))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    Debug.Log("What type of data: " + webRequest.downloadHandler.text.GetType());
                    break;
            }
        }
    }
}

[System.Serializable]
public class CurrStruct
{
    public List<CurrEntry> list = new();
}

[System.Serializable]
public class CurrEntry
{
    public string subCode;
    public string subName;
    public string subFac;
    public string subStud;
    public int subToCre;
    public int subDur;
    public string subPreq;
    public int subSem;
    public string subYear;
    public string subRoom;
}

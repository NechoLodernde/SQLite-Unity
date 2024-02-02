using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mono.Data.Sqlite;
#if UNITY_EDITOR
using UnityEditor;
#endif

// TODO: Optimize the script for testing class and methods,
//       don't forget to check for any mistype


public class TestingScripts : MonoBehaviour
{
    public TMP_Text sentence_ui;

    private void Awake()
    {
        //AwakePlayerData();
        //AwakeInventoryData();
        //AwakePlayerInventoryData();
    }

    private void Start()
    {
        //TestingPlayerData();
        //TestingInventoryData();
        //TestingPlayerInventory();
        System.Version version =
            typeof(SqliteConnection).Assembly.GetName().Version;

        Debug.Log($"Mono.Data.Sqlite Version: {version}");

        TestCurriculumXML();
    }

    private void AwakePlayerData()
    {
        PlayerDBScript.Instance.CreateDB();
        PlayerDBScript.Instance.DeleteDB();
    }

    private void AwakeInventoryData()
    {
        InventoryDBScript.InventoryDBInstance.CreateDB();
        InventoryDBScript.InventoryDBInstance.DeleteDB();
    }

    private void AwakePlayerInventoryData()
    {
        PlayerInventoryDBScript.PlayerInvDBInstance.CreateDB();
        PlayerInventoryDBScript.PlayerInvDBInstance.DeleteDB();
    }

    private void TestingPlayerData()
    {
        StartCoroutine(LoadingTime());

        PlayerDBScript.Instance.AddPlayer("Vanitas",
            "Perempuan", "Fakultas Teknologi Informasi",
            "Perpustakaan & Sains Informasi", 1);

        PlayerDBScript.Instance.AddPlayer("Misono Mika",
            "Perempuan", "Fakultas Teknik",
            "Teknik Nuklir", 1);

        Debug.Log("Total number of data: " +
            PlayerDBScript.Instance.CountData());
        // PlayerDBScript.PlayerDBScriptInstance.LoadPlayers();
        sentence_ui.text = "";

        foreach (PlayerDEntry players in 
            PlayerDataManager.Instance.Struct.list)
        {
            sentence_ui.text += "ID: " + players.playerID + "\n";
            sentence_ui.text += "Name: " + players.playerName + "\n";
            sentence_ui.text += "Gender: " + players.playerGender + "\n";
            sentence_ui.text += "Faculty: " + players.playerFaculty + "\n";
            sentence_ui.text += "Study Program: " + players.playerStudyProgram + "\n";
            sentence_ui.text += "Semester: " + players.playerSemester + "\n";
            sentence_ui.text += "Active Quest: " + players.activeQuestCode + "\n";
        }
    }

    private void TestingInventoryData()
    {
        StartCoroutine(LoadingTime());

        InventoryDBScript.InventoryDBInstance.AddItem("Notebook",
            "Essential", 2.0, 1);

        InventoryDBScript.InventoryDBInstance.AddItem("Backpack",
            "Essential", 4.0, 1);

        Debug.Log("Total number of data: " +
            InventoryDBScript.InventoryDBInstance.CountData());
        sentence_ui.text = "";

        foreach (InventoryDataEntry inv in 
            InventoryDataManager.Instance.Struct.list)
        {
            sentence_ui.text += "ID: " + inv.inventoryID + "\n";
            sentence_ui.text += "Inventory Name: " + inv.inventoryName
                + "\n";
            sentence_ui.text += "Inventory Type: " + inv.inventoryType
                + "\n";
            sentence_ui.text += "Inventory Weight: " + inv.inventoryWeight
                + "\n";
            sentence_ui.text += "Inventory Usable Code " +
                inv.inventoryUsableCode + "\n";
        }
    }

    private void TestingPlayerInventory()
    {
        StartCoroutine(LoadingTime());

        foreach (InventoryDataEntry inv 
            in InventoryDataManager.Instance.Struct.list)
        {
            PlayerInventoryDBScript.PlayerInvDBInstance.AddInventory(
                inv.inventoryID);
        }

        Debug.Log("Total number of data: " +
            PlayerInventoryDBScript.PlayerInvDBInstance.CountData());
        sentence_ui.text = "";

        foreach (PlayerInventoryEntry inv in
            PlayerInventoryManager.PlayerInventoryInstance.inventoryStruct.list)
        {
            sentence_ui.text += "ID: " + inv.inventoryID + "\n";
            sentence_ui.text += "Inventory Number: " +
                inv.inventoryNumber + "\n";
            sentence_ui.text += "Inventory Name: " +
                inv.inventoryName + "\n";
            sentence_ui.text += "Inventory Usable Code " +
                inv.inventoryUsableCode + "\n";
        }

        PlayerInventoryEntry[] testDelete =
            PlayerInventoryDBScript.PlayerInvDBInstance.ReturnPIEArray();
        foreach(PlayerInventoryEntry inv in testDelete)
        {
            Debug.Log(inv.inventoryID);
            Debug.Log(inv.inventoryNumber);
            Debug.Log(inv.inventoryName);
            Debug.Log(inv.inventoryUsableCode);
        }
        //PlayerInventoryDBScript.PlayerInvDBInstance.DeleteInventory(testDelete[0].inventoryID);
        PlayerInventoryManager.PlayerInventoryInstance.UpdateUsableCode(
            testDelete[0].inventoryID, 0);
    }

    private void TestCurriculumXML()
    {
        //StartCoroutine(LoadingTime());

        //CurriculumXMLScript.CurriculumXMLInstance.LoadCurriculums();

        CurriculumManager.CurriculumManagerInstance.
            InsertNewData("KK140075_22",
            "Pengembangan Permainan Digital",
            "FTI", "Teknik Infomratika", 15,
            750, "KK140056_18", 5, "Ganjil",
            "UY.05R003");
        CurriculumManager.CurriculumManagerInstance.
            InsertNewData("KK140056_18",
            "Pemrograman Berorientasi Objek",
            "FTI", "Teknik Informatika", 4,
            200, "KK140053_18", 3, "Ganjil",
            "FO");

        Debug.Log(" Total number of data in list: " +
            CurriculumManager.CurriculumManagerInstance.
            curriculumStruct.list.Count);
        sentence_ui.text = "";

        CurriculumXMLScript.CurriculumXMLInstance.
            SaveCurriculums();

        //foreach (PlayerDataEntry players in
        //    PlayerDataManager.PlayerDataInstance.playerStruct.list)
        //{
        //    sentence_ui.text += "ID: " + players.playerID + "\n";
        //    sentence_ui.text += "Name: " + players.playerName + "\n";
        //    sentence_ui.text += "Gender: " + players.playerGender + "\n";
        //    sentence_ui.text += "Faculty: " + players.playerFaculty + "\n";
        //    sentence_ui.text += "Study Program: " + players.playerStudyProgram + "\n";
        //    sentence_ui.text += "Semester: " + players.playerSemester + "\n";
        //    sentence_ui.text += "Active Quest: " + players.activeQuestCode + "\n";
        //}
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    IEnumerator LoadingTime()
    {
        yield return new WaitForSeconds(5);
        TestCurriculumXML();
    }
}
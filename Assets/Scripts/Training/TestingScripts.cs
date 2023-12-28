using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TestingScripts : MonoBehaviour
{
    public TMP_Text sentence_ui;

    private void Start()
    {
        PlayerDBScript.PlayerDBScriptInstance.DeleteDB();
        PlayerDBScript.PlayerDBScriptInstance.CreateDB();
        StartCoroutine(LoadingTime());

        PlayerDBScript.PlayerDBScriptInstance.AddPlayer("Vanitas",
            "Perempuan", "Fakultas Teknologi Informasi",
            "Perpustakaan & Sains Informasi", 1);

        PlayerDBScript.PlayerDBScriptInstance.AddPlayer("Misono Mika",
            "Perempuan", "Fakultas Teknik",
            "Teknik Nuklir", 1);

        Debug.Log("Total number of data: ");
        Debug.Log(PlayerDBScript.PlayerDBScriptInstance.CountData());
        // PlayerDBScript.PlayerDBScriptInstance.LoadPlayers();
        sentence_ui.text = "";

        foreach(PlayerDataEntry players in PlayerDataManager.PlayerDataInstance.playerStruct.list)
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
        yield return new WaitForSeconds(3);
    }
}



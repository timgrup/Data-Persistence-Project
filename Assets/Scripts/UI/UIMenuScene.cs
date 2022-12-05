using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIMenuScene : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;
    public Button hallOfFameButton;
    public TMP_InputField playerNameInput;

    void Start()
    {
        playButton.onClick.AddListener(StartGame);
        hallOfFameButton.onClick.AddListener(OpenHallOfFame);
        exitButton.onClick.AddListener(ExitGame);
    }

    public void StartGame()
    {
        string playerName = playerNameInput.text;
        GameManager.Instance.PlayerName = playerName;
        Debug.Log($"Start Game as {playerName}");
        SceneManager.LoadScene(1);
    }


    public void OpenHallOfFame() {
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private TMP_Text greetingsText;

    private PlayerManager _playerManager;

    private void Start()
    {
        _playerManager = PlayerManager.Instance;

        var playerData = _playerManager.GetPlayerData();
        Debug.Log(playerData.player.playerName);
        greetingsText.text = $"Hi, {playerData.player.playerName}";

        startGameButton.onClick.AddListener(ChangeScene);
        logoutButton.onClick.AddListener(OnLogoutButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("InGame");
    }

    private void OnLogoutButtonClick()
    {
        _playerManager.ClearPlayerData();
    }

    private void OnQuitButtonClick()
    {
#if UNITY_EDITOR
        _playerManager.ClearPlayerData();
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

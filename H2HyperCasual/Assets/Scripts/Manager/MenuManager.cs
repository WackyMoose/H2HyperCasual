using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject authenticationPanel;
    [SerializeField] private GameObject menuPanel;

    private PlayerManager _playerManager;

    private void Start()
    {
        _playerManager = PlayerManager.Instance;
        _playerManager.OnPlayerLogin += OnPlayerLogin;
        _playerManager.OnPlayerLogout += OnPlayerLogout;

        authenticationPanel.SetActive(!_playerManager.IsAuthenticated);
        menuPanel.SetActive(_playerManager.IsAuthenticated);
    }

    private void OnPlayerLogin()
    {
        authenticationPanel.SetActive(!_playerManager.IsAuthenticated);
        menuPanel.SetActive(_playerManager.IsAuthenticated);
    }

    private void OnPlayerLogout()
    {
        authenticationPanel.SetActive(!_playerManager.IsAuthenticated);
        menuPanel.SetActive(_playerManager.IsAuthenticated);
    }
}

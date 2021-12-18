using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;

    private void Start()
    {
        loginButton.onClick.AddListener(Login);
        registerButton.onClick.AddListener(Register);
    }

    private async void Login()
    {
        await APIManager.Instance.LoginAsync(playerNameInput.text, passwordInput.text);
    }

    private async void Register()
    {
        await APIManager.Instance.RegisterAsync(playerNameInput.text, passwordInput.text);
    }
}

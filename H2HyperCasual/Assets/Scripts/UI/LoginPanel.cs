using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;

    private void Start()
    {
        loginButton.onClick.AddListener(Login);
    }

    private async void Login()
    {
        await APIManager._instance.Login(playerNameInput.text, passwordInput.text);
    }
}

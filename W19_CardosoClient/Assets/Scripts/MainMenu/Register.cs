using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;

public class Register : MonoBehaviour
{
    // Cached references
    public InputField emailInputField;
    public InputField passwordInputField;
    public InputField confirmPasswordInputField;
    public Button registerButton;
    public Text messageBoardText;
    private Player playerManager;

    string httpServer;

    private void Start()
    {
        playerManager = FindObjectOfType<Player>();
        httpServer = playerManager.GetHttpServer();
    }

    public void OnRegisterButtonClick()
    {
        StartCoroutine(RegisterNewUser());
    }

    private IEnumerator RegisterNewUser()
    {
        if (string.IsNullOrEmpty(emailInputField.text))
        {
            throw new NullReferenceException("Email can't be void");
        }
        else if (string.IsNullOrEmpty(passwordInputField.text))
        {
            throw new NullReferenceException("Password can't be void");
        }
        else if (passwordInputField.text != confirmPasswordInputField.text)
        {
            throw new Exception("Passwords don't match");
        }

        UnityWebRequest htppClient = new UnityWebRequest(httpServer + "/api/Account/Register", "POST");

        AspNetUserRegister newUser = new AspNetUserRegister();
        newUser.Email = emailInputField.text;
        newUser.Password = passwordInputField.text;
        newUser.ConfirmPassword = confirmPasswordInputField.text;

        string jsonData = JsonUtility.ToJson(newUser);
        byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);
        htppClient.uploadHandler = new UploadHandlerRaw(dataToSend);
        htppClient.SetRequestHeader("Content-Type", "application/json");
        //DON'T NEED CERTIFICATED
        yield return htppClient.SendWebRequest();

        if (htppClient.isNetworkError || htppClient.isHttpError)
        {
            throw new Exception("ERROR: TRY TO REGISTER " + htppClient.responseCode);

            htppClient.Dispose();
        }


    }

}

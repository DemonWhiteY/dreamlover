using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    public Text password;
    public Text account;
    public Text warning;
    public void login()
    {
        SceneManager.LoadScene("Loading");
    }
    public void sendLogin()
    {
        Message msg;
        msg = new Message();
        msg.command = "login";
        msg.ask= account.text + "_"+password.text;
        PythonClient.Instance.AddMessage(msg);
    }

    public void Logincheck(int n)
    {
        if (n == 3)
            login();
        else if (n == 2)
            warning.text = "密码错误";
        else if(n == 1)
            warning.text = "用户名不存在";


    }

   
}

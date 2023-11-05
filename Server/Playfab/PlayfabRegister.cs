using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabRegister : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField username;
    public TMP_InputField password;

    // Start is called before the first frame update
    void Start()
    {
        this.email.text = "lucpham5661@gmail.com";
        this.username.text = "lucpham5661";
        this.password.text = "123456";
    }

    public virtual void Register()
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest
        {
            Email=this.email.text,
            Password=this.password.text,
            Username=this.username.text,
            DisplayName=this.username.text,
        };  

        PlayFabClientAPI.RegisterPlayFabUser(request, this.RegisterSuccess,this.RegisterError);
    }

    public virtual void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Đăng ký thành công");
    }

    public virtual void RegisterError(PlayFabError error)
    {
        string textError = error.GenerateErrorReport();
        Debug.Log("Đăng ký thất bại "+ textError);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject loginTab;
    public GameObject registerTab;
    public GameObject menuTab;
    public GameObject createCharacterTab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showLoginTab()
    {
        loginTab.SetActive(true);
        registerTab.SetActive(false);
        menuTab.SetActive(false);
    }
    public void showRegisterTab()
    {
        loginTab.SetActive(false);
        registerTab.SetActive(true);
        menuTab.SetActive(false);
    }

    public void showCreateCharacterTab()
    {
        loginTab.SetActive(false);
        registerTab.SetActive(false);
        menuTab.SetActive(false);
        createCharacterTab.SetActive(true);
    }
}

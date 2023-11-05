using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Xml;
using TMPro;
using UltimaQuestSystem.Example;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabController : MonoBehaviour
{
    public static PlayFabController PFC;

    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField nickname;
    private void OnEnable()
    {
        if (PlayFabController.PFC == null)
        {
            PlayFabController.PFC = this;
        }
        else
        {
            if (PlayFabController.PFC != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        this.username.text = "lucpham566";
        this.password.text = "123456";

    }

    #region Login

    public virtual void Login()
    {
        LoginWithPlayFabRequest loginRequest = new LoginWithPlayFabRequest
        {
            Password = this.password.text,
            Username = this.username.text,
        };

        PlayFabClientAPI.LoginWithPlayFab(loginRequest, this.OnLoginSuccess, this.OnLoginFailure);

    }

    private void OnLoginSuccess(LoginResult result)
    {

        //UserDataManager.Instance.SetUserData(username.text, result.SessionTicket, result.PlayFabId);

        Debug.Log("login success");
        Debug.Log("playfab id " + result.PlayFabId);
        Debug.Log("sessionTicket " + result.SessionTicket);

        UserDataManager.Instance.SetUserData(username.text, result.SessionTicket, result.PlayFabId);
        //SetStats();
        GetInventory();
        GetDataStatisticsUserLogin();

        if (playerLevel > 0)
        {
            SceneManager.LoadScene("photon");
        }
        else
        {

        }
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void OnCreateCharacter()
    {
        CreateCharacter(name);
        SceneManager.LoadScene("photon");

    }

    public void CreateCharacter(string nickName)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "CreateCharacter",
            FunctionParameter = new
            {
                PlayerLevel = 1,
                PlayerHealth = playerHealth,
                PlayerDamage = playerDamage,
            },
            GeneratePlayStreamEvent = true
        },
        result => {
            Debug.Log("User statistics updated");
        },
        error => {
            Debug.LogError(error.GenerateErrorReport()
            );
        });
    }


    #endregion Login
    public string playerName;
    public int playerLevel;
    public int gameLevel;
    public int playerHealth;
    public int playerDamage;
    public int magicDefence;
    public int physicalDefence;
    public int playerXp;

    #region PlayerStats
    public void SetStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "SetPlayerStaticData",
            FunctionParameter = new
            {
                PlayerLevel = playerLevel,
                PlayerHealth = playerHealth,
                PlayerDamage = playerDamage,
            },
            GeneratePlayStreamEvent = true
        },
        result => { Debug.Log("User statistics updated"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "SetPlayerTitleData",
            FunctionParameter = new
            {
            },
            GeneratePlayStreamEvent = true
        },
        result => { Debug.Log("User player data updated"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });

    }
    void GetStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStats,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }
    void OnGetStats(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
            switch (eachStat.StatisticName)
            {
                case "PlayerLevel":
                    playerLevel = eachStat.Value;
                    break;
                case "GameLevel":
                    gameLevel = eachStat.Value;
                    break;
                case "PlayerHealth":
                    playerHealth = eachStat.Value;
                    break;
                case "PlayerDamage":
                    playerDamage = eachStat.Value;
                    break;
             
            }
        }
    }
    #endregion PlayerStats

    #region PlayerInventory

    void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(
            new GetUserInventoryRequest(),
            OngetGetInventory,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OngetGetInventory(GetUserInventoryResult result)
    {
        string json = JsonUtility.ToJson(result);
        Debug.Log("data là " + json);
        Debug.Log("VirtualCurrency là " + JsonUtility.ToJson(result.VirtualCurrency));
    }

    struct GetDataStatisticsUserLoginResult
    {
        public string playerName;
        public int playerLevel;
        public int gameLevel;
        public int playerHealth;
        public int playerDamage;
        public int magicDefence;
        public int physicalDefence;
        public int playerXp;

    }

    void GetDataStatisticsUserLogin()
    {

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "playerLogin",
            FunctionParameter = new
            {
            },
            GeneratePlayStreamEvent = true
        },
        result =>
        {
            var jsonEncodedResult = result.FunctionResult.ToString();
            var resultObject = JsonUtility.FromJson<GetDataStatisticsUserLoginResult>(jsonEncodedResult);

            Debug.Log("resultObject.playerHP" + resultObject.playerHealth);
            playerName = resultObject.playerName;
            playerHealth = resultObject.playerHealth;
            playerDamage = resultObject.playerDamage;
            magicDefence = resultObject.magicDefence;


            Debug.Log("playerLogin" + result.FunctionResult);
        },
        error => { Debug.LogError(error.GenerateErrorReport()); });


    }


    #endregion PlayerInventory
}
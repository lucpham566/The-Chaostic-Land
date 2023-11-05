using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance;

    public string Username { get; private set; }
    public string Ticket { get; private set; }
    public string CharacterId { get; private set; }
    public string PlayFabId { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetUserData(string username, string ticket, string playFabId)
    {
        Username = username;
        Ticket = ticket;
        CharacterId = playFabId;
        PlayFabId = playFabId;
    }
}

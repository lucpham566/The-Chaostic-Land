using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDataControl : MonoBehaviour
{
    public TextMeshProUGUI username;
    public TextMeshProUGUI ticket;

    void Start()
    {
        //this.username.text = UserDataManager.Instance.Username;
        //this.ticket.text = UserDataManager.Instance.Ticket;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("nhấn i" );

            GetFriendsList();
        }
       
    }

    private void logUserInfor()
    {
        var request = new GetCharacterDataRequest
        {
            PlayFabId = UserDataManager.Instance.PlayFabId,
            CharacterId = UserDataManager.Instance.CharacterId,
            Keys = new List<string> { "Health", "Mana", "Position" }
        };

        PlayFabClientAPI.GetCharacterData(request, OnGetCharacterDataSuccess, OnGetCharacterDataFailure);
    }

    private void OnGetCharacterDataSuccess(GetCharacterDataResult result)
    {
        Debug.Log("Lấy dữ liệu nhân vật thành công!" + result.ToString());
        // Xử lý dữ liệu ở đây, nó sẽ được lưu trong result.Data
    }

    // Xử lý khi lấy dữ liệu nhân vật thất bại
    private void OnGetCharacterDataFailure(PlayFabError error)
    {
        Debug.LogError("Lấy dữ liệu nhân vật thất bại: " + error.GenerateErrorReport());
    }


    public void GetPlayerInfo()
    {
        var request = new GetPlayerCombinedInfoRequest
        {
            PlayFabId = UserDataManager.Instance.PlayFabId, // Thay thế bằng PlayFabId của người chơi
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetUserAccountInfo = true,
                GetCharacterInventories = true,
                // Thêm các thông tin khác cần lấy ở đây
            }
        };

        PlayFabClientAPI.GetPlayerCombinedInfo(request, OnGetPlayerInfoSuccess, OnGetPlayerInfoFailure);
    }

    private void OnGetPlayerInfoSuccess(GetPlayerCombinedInfoResult result)
    {

        // Xử lý thông tin người chơi ở đây
        var characters = result.InfoResultPayload.CharacterList;
        foreach (var character in characters)
        {
            Debug.Log("CharacterId: " + character.CharacterId);
            // Xử lý thông tin của mỗi nhân vật
        }

    }

    private void OnGetPlayerInfoFailure(PlayFabError error)
    {
        Debug.LogError("GetPlayerInfo failed: " + error.GenerateErrorReport());
    }

    public void GetFriendsList()
    {
        var request = new GetFriendsListRequest
        {
        };

        PlayFabClientAPI.GetFriendsList(request, OnGetFriendsListSuccess, OnGetFriendsListFailure);
    }

    private void OnGetFriendsListSuccess(GetFriendsListResult result)
    {
        Debug.Log("Friend : " + result.Friends.Count);

        foreach (var friendInfo in result.Friends)
        {
            Debug.Log("Friend: " + friendInfo.Username);
        }
    }

    private void OnGetFriendsListFailure(PlayFabError error)
    {
        Debug.LogError("Failed to get friends list: " + error.ErrorMessage);
    }
}

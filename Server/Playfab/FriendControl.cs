using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendControl : MonoBehaviour
{
    private string playFabIdToAdd; // PlayFab ID của người chơi bạn muốn kết bạn với

    public void SendFriendRequest()
    {
        playFabIdToAdd = "980C2AA12FD766C";
        // Tạo yêu cầu kết bạn
        var request = new AddFriendRequest
        {
            FriendPlayFabId = playFabIdToAdd // PlayFab ID của người chơi bạn muốn kết bạn
        };

        // Gửi yêu cầu đến PlayFab
        PlayFabClientAPI.AddFriend(request, OnFriendRequestSuccess, OnFriendRequestFailure);
    }

    private void OnFriendRequestSuccess(AddFriendResult result)
    {
        Debug.Log("Yêu cầu kết bạn đã được gửi thành công!");
    }

    private void OnFriendRequestFailure(PlayFabError error)
    {
        Debug.LogError("Lỗi khi gửi yêu cầu kết bạn: " + error.ErrorMessage);
    }
}

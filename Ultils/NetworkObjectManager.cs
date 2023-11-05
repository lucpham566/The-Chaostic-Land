using Fusion;
using System.Collections;
using UnityEngine;
using static Unity.Collections.Unicode;

public class NetworkObjectManager : NetworkBehaviour
{
    public static NetworkObjectManager Instance;

    private void Awake()
    {
        // Đảm bảo chỉ có một thể hiện của script tồn tại
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Định nghĩa hàm DestroyNetworkObject
    public void DestroyNetworkObject(NetworkObject objectToDespawn, float time = 0f)
    {
        if (Runner != null)
        {
            StartCoroutine(DespawnAfterDelay(objectToDespawn, time));
        }
    }


    private IEnumerator DespawnAfterDelay(NetworkObject objectToDespawn,float time)
    {
        if (time>0)
        {
            yield return new WaitForSeconds(time); // Thời gian chờ trước khi despawn (ví dụ: 3 giây)
        }

        if (Runner != null) // Đảm bảo Runner đã được gán giá trị
        {
            // Sử dụng Runner.Despawn để despawn GameObject sau khoảng thời gian
            Runner.Despawn(objectToDespawn);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneToLoad; // Tên của cảnh mới

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu nhân vật va chạm với cổng
        {
            if (sceneToLoad != "" && sceneToLoad != null)
            {
            SceneManager.LoadScene(sceneToLoad); // Chuyển đến cảnh mới

            }
        }
    }
}

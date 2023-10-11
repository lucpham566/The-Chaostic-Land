using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Khai báo các biến và phương thức AudioManager của bạn ở đây.

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Đảm bảo AudioManager tồn tại qua các cảnh
        }
        else
        {
            Destroy(gameObject); // Nếu đã có một instance khác tồn tại, hủy bỏ instance này.
        }
    }

    public void PlaySound(AudioClip clip)
    {
        // Đây là nơi bạn có thể phát âm thanh
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
    // Các phương thức xử lý âm thanh ở đây.
}

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// クリア画面（Clear Game シーン）の管理スクリプト
public class ClearManager : MonoBehaviour
{
    [Header("撃破数表示")]
    [SerializeField] private TextMeshProUGUI killCountText; // 撃破数を表示するテキスト

    void Start()
    {
        // StageManager が PlayerPrefs に保存したスコアを取得して表示
        int score = PlayerPrefs.GetInt("ClearScore", 0);
        if (killCountText != null)
            killCountText.text = "Score: " + score;
    }

    // リトライボタン用：ゲーム本編（SampleScene）へ
    public void RetryGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // タイトルボタン用：タイトル画面（Title Scene）へ
    public void GoToTitle()
    {
        SceneManager.LoadScene("Title Scene");
    }
}

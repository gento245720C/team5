using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // リトライボタン用：ゲーム本編（SampleScene）へ
    public void RetryGame()
    {
        // あなたのメインゲームのシーン名に合わせてください
        SceneManager.LoadScene("SampleScene"); 
    }

    // タイトルボタン用：タイトル画面（Title Scene）へ
    public void GoToTitle()
    {
        // あなたのタイトル画面のシーン名に合わせてください
        SceneManager.LoadScene("Title Scene");
    }
}
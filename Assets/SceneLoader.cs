using UnityEngine;
using UnityEngine.SceneManagement; // これが重要

public class SceneLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        // 遷移したいシーンの名前（GameScene）をここに入力
        SceneManager.LoadScene("SampleScene");
    }
}
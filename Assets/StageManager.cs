using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [Header("ステージクリア")]
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private TextMeshProUGUI clearText;

    [Header("撃破数表示")]
    [SerializeField] private TextMeshProUGUI killCountText;
    private int killCount = 0;

    [Header("スコア表示")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int scorePerKill = 100;
    private int score = 0;

    [Header("HP表示（赤丸アイコン）")]
    [SerializeField] private GameObject[] hpCircles; // HP分の赤丸オブジェクト（3つ設定する）

    private bool cleared;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateKillCountUI();
        UpdateScoreUI();
        UpdateHPUI(hpCircles != null ? hpCircles.Length : 3);
    }

    public void AddKill()
    {
        killCount++;
        score += scorePerKill;
        UpdateKillCountUI();
        UpdateScoreUI();
    }

    public void UpdateHP(int currentLives)
    {
        UpdateHPUI(currentLives);
    }

    public void TriggerClear()
    {
        if (cleared) return;
        cleared = true;

        if (clearPanel != null) clearPanel.SetActive(true);
        if (clearText != null) clearText.text = "CLEAR!!";

        Time.timeScale = 0f;
    }

    private void UpdateKillCountUI()
    {
        if (killCountText != null)
            killCountText.text = "               Kills: " + killCount;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    private void UpdateHPUI(int currentLives)
    {
        if (hpCircles == null) return;
        for (int i = 0; i < hpCircles.Length; i++)
        {
            if (hpCircles[i] != null)
                hpCircles[i].SetActive(i < currentLives);
        }
    }
}

using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    public static StageManager Instance { get; private set; }

    [Header("ステージクリア")]
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private TextMeshProUGUI clearText;

    [Header("スコア表示")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    private int score = 0;
    private int highScore = 0;

    [Header("HP表示（赤丸アイコン）")]
    [SerializeField] private GameObject[] hpCircles; 

    private bool cleared;

    // ★追加：SE再生用のコンポーネント
    private AudioSource seAudioSource;

    void Awake()
    {
        Instance = this;
        // ★追加：自分自身にSE専用のAudioSourceを付ける
        seAudioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        UpdateScoreUI();
        UpdateHighScoreUI();
        UpdateHPUI(hpCircles != null ? hpCircles.Length : 3);
    }

    // 外から音を鳴らすための命令
    // これを使うと、複数の音が重なっても打ち消し合わずに全て鳴ります
    public void PlaySE(AudioClip clip, float volume)
    {
        if (clip != null && seAudioSource != null)
        {
            seAudioSource.PlayOneShot(clip, volume);
        }
    }

    public void AddKill()
    {
        AddScore(100);
    }

    public void AddScore(int addedScore)
    {
        score += addedScore;
        UpdateHighScore();
        UpdateScoreUI();
        UpdateHighScoreUI();
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

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    private void UpdateHighScore()
    {
        if (score <= highScore) return;

        highScore = score;
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.Save();
    }

    private void UpdateHighScoreUI()
    {
        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore;
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
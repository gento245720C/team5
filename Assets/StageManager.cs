using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [SerializeField] private GameObject clearPanel;
    [SerializeField] private TextMeshProUGUI clearText;

    private bool cleared;

    void Awake()
    {
        Instance = this;
    }

    public void TriggerClear()
    {
        if (cleared) return;
        cleared = true;

        if (clearPanel != null) clearPanel.SetActive(true);
        if (clearText != null) clearText.text = "CLEAR!!";

        Time.timeScale = 0f;
    }
}

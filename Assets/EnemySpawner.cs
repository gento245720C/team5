using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;         // 四角い敵のプレハブ
    public GameObject triangleEnemyPrefab; // 三角の敵のプレハブ

    [Header("初期スポーン設定")]
    public float initialSpawnInterval = 3.0f; // 最初の出現間隔（秒）

    [Header("敵数管理")]
    public int targetEnemyCount = 5; // 画面上に維持したい敵の数

    [Header("難易度上昇設定")]
    public float minSpawnInterval = 0.8f;     // 出現間隔の最小値（これ以上は速くならない）
    public float difficultyInterval = 15.0f;  // 何秒おきに難易度を上げるか
    public float spawnIntervalDecrement = 0.3f; // 1段階ごとに縮める秒数

    private float currentSpawnInterval;  // 現在の出現間隔
    private float spawnTimer = 0f;       // 次の敵を出すまでのタイマー
    private float difficultyTimer = 0f;  // 次の難易度上昇までのタイマー

    void Start()
    {
        // 最初の出現間隔をセット
        currentSpawnInterval = initialSpawnInterval;
    }

    void Update()
    {
        // --- 難易度上昇タイマー ---
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= difficultyInterval)
        {
            difficultyTimer = 0f;
            // 出現間隔を短くする（minSpawnInterval を下回らないようにクランプ）
            currentSpawnInterval = Mathf.Max(currentSpawnInterval - spawnIntervalDecrement, minSpawnInterval);
            Debug.Log("難易度アップ！出現間隔: " + currentSpawnInterval + "秒");
        }

        // --- 敵のスポーンタイマー ---
        // 画面上の敵が少ないほどスポーン間隔を短くして敵数を一定に保つ
        int currentEnemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        float adjustedInterval = currentEnemyCount < targetEnemyCount
            ? currentSpawnInterval * 0.3f  // 敵が少ないときは早めにスポーン
            : currentSpawnInterval;        // 十分いるときは通常間隔

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= adjustedInterval)
        {
            spawnTimer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // 出す敵の種類をランダムに選ぶ（四角か三角か）
        GameObject prefabToSpawn = Random.value > 0.5f ? enemyPrefab : triangleEnemyPrefab;

        if (prefabToSpawn != null)
        {
            // 敵を生成（位置はEnemyスクリプトのStartで決まるのでゼロ地点でOK）
            Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
        }
    }
}

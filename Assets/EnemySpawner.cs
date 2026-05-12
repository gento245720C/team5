using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 四角い敵のプレハブ
    public GameObject triangleEnemyPrefab; // 三角の敵のプレハブ
    public float spawnInterval = 3.0f; // 何秒おきに敵を出すか

    void Start()
    {
        // 3秒おき（spawnInterval）に敵を生成し続ける
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
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
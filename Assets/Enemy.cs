using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public float downDistance = 0.5f;
    private int direction = 1;

    // ★追加：敵の弾のプレハブ
    public GameObject enemyBulletPrefab;
    // ★追加：発射間隔（秒）
    public float shotInterval = 3f;
    // ★追加：タイマー
    private float timer;

    void Start()
{
    // 最初の発射までの時間を0からshotIntervalの間でランダムに決める
    timer = Random.Range(0f, shotInterval);
}

    void Update()
    {
        // 移動の処理（変更なし）
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
        if (transform.position.x > 8.0f || transform.position.x < -8.0f)
        {
            direction *= -1;
            Vector3 pos = transform.position;
            pos.y -= downDistance;
            transform.position = pos;
            transform.position += Vector3.right * direction * 0.1f;
        }

        // ★追加：自動発射の処理
        timer += Time.deltaTime; // 毎フレーム時間を足す
        if (timer > shotInterval)
        {
            Shoot();
            timer = 0; // タイマーをリセット
        }
    }

    // ★追加：弾を撃つ関数
    void Shoot()
    {
        Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
    }

    // 自機の弾が当たった時の処理（変更なし）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
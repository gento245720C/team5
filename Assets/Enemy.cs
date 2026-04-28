using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("移動設定")]
    public float speed = 2f;
    public float downDistance = 0.5f;
    private int direction = 1;

    [Header("攻撃設定")]
    public GameObject enemyBulletPrefab;
    public float shotInterval = 3f;
    private float timer;

    void Start()
    {
        // 敵ごとに発射タイミングをバラつかせる
        timer = Random.Range(0f, shotInterval);
    }

    void Update()
    {
        // 横移動
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // 端での反転と降下
        if (transform.position.x > 8.5f || transform.position.x < -8.5f)
        {
            direction *= -1;
            Vector3 pos = transform.position;
            pos.y -= downDistance;
            transform.position = pos;
            transform.position += Vector3.right * direction * 0.1f;
        }

        // 自動発射
        timer += Time.deltaTime;
        if (timer > shotInterval)
        {
            Shoot();
            timer = 0;
        }
    }

    void Shoot()
    {
        if (enemyBulletPrefab != null)
        {
            Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
        }
    }

    // ★敵に何かが当たった時の判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 自機の弾（Bulletタグ）に当たった場合
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("敵を撃破！");
            Destroy(collision.gameObject); // 当たった自機の弾を消す
            Destroy(gameObject);           // 自分（敵）を消す
        }
    }
}
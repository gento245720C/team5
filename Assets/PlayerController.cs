using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float speed = 8f;
    public float xLimit = 8.5f;

    [Header("発射設定")]
    public GameObject bulletPrefab;
    public float shotInterval = 0.5f;
    private float timer = 0f;

    [Header("残機設定")]
    public int lives = 3; // ★初期の残機
    private Vector3 startPosition; // ★復活する場所

    void Start()
    {
        // 最初にいた場所を覚えておく
        startPosition = transform.position;
    }

    void Update()
    {
        // 移動処理
        float move = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * move * speed * Time.deltaTime);

        // 画面端の制限
        float xPos = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

        // 発射処理
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && timer >= shotInterval)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject); // 敵の弾を消す
            PlayerDamaged(); // ★ダメージ処理へ
        }
    }

    void PlayerDamaged()
    {
        lives--; // 残機を1減らす
        Debug.Log("残り残機: " + lives);

        if (lives > 0)
        {
            // まだ残機があるなら、初期位置に戻る
            transform.position = startPosition;
        }
        else
        {
            // 残機がなくなったら消滅
            Debug.Log("ゲームオーバー...");
            Destroy(gameObject);
        }
    }
}
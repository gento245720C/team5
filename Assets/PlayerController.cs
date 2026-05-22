using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float speed = 8f;
    public float xLimit = 8.5f;
    // ★新しく縦の制限を追加（画面サイズに合わせて後で調整できます）
    public float yMin = -4.5f; // 下の限界
    public float yMax = 4.5f;  // 上の限界

    [Header("発射設定")]
    public GameObject bulletPrefab;
    public float shotInterval = 0.5f;
    private float timer = 0f;

    [Header("残機設定")]
    public int lives = 3; // ★初期の残機
    private Vector3 startPosition; // ★復活する場所

    [Header("無敵設定")]
    public float invincibleDuration = 2f; // 無敵時間（秒）
    public float blinkInterval = 0.1f;    // 点滅間隔（秒）
    private bool isInvincible = false;
    private float invincibleTimer = 0f;
    private float blinkTimer = 0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // ★横と縦の入力受け取り
        float moveX = Input.GetAxis("Horizontal"); // 左右（A/Dキー、←/→キー）
        float moveY = Input.GetAxis("Vertical");   // 上下（W/Sキー、↑/↓キー）

        // 移動処理（XとY両方に動きを適用）
        transform.Translate(new Vector2(moveX, moveY) * speed * Time.deltaTime);

        // ★画面端の制限（X軸とY軸両方にはみ出さないように計算）
        float xPos = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
        float yPos = Mathf.Clamp(transform.position.y, yMin, yMax);
        
        // 計算した結果の座標を適用
        transform.position = new Vector3(xPos, yPos, transform.position.z);

        // 発射処理
        timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && timer >= shotInterval) // 押しっぱなしで連射
        {
            Shoot();
            timer = 0f;
        }

        // 無敵時間の処理
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            blinkTimer -= Time.deltaTime;

            if (blinkTimer <= 0f)
            {
                blinkTimer = blinkInterval;
                if (spriteRenderer != null)
                    spriteRenderer.enabled = !spriteRenderer.enabled;
            }

            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
                if (spriteRenderer != null)
                    spriteRenderer.enabled = true;
            }
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
            if (isInvincible) return; // 無敵中はダメージなし
            Destroy(collision.gameObject);
            PlayerDamaged();
        }
    }

    void PlayerDamaged()
    {
        lives--; // 残機を1減らす
        Debug.Log("残り残機: " + lives);
        StageManager.Instance?.UpdateHP(lives); // HP表示を更新

        if (lives > 0)
        {
            // 無敵状態を開始（その場で点滅）
            isInvincible = true;
            invincibleTimer = invincibleDuration;
            blinkTimer = blinkInterval;
        }
        else
        {
            // 残機がなくなったら消滅
            Debug.Log("ゲームオーバー...");
            Destroy(gameObject);
        }
    }
}
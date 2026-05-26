using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float speed = 8f;
    public float xLimit = 2.1f; 
    public float yMin = -4.5f;
    public float yMax = 4.5f;

    [Header("発射設定")]
    public GameObject bulletPrefab;
    public float shotInterval = 0.5f;
    private float timer = 0f;

    [Header("残機設定")]
    public int lives = 3;
    public GameObject[] lifeIcons; 
    private Vector3 startPosition;

    [Header("無敵設定")]
    public float invincibleDuration = 2f;
    public float blinkInterval = 0.1f;
    private bool isInvincible = false;
    private float invincibleTimer = 0f;
    private float blinkTimer = 0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateLifeUI(); 
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        transform.Translate(new Vector2(moveX, moveY) * speed * Time.deltaTime);

        float xPos = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
        float yPos = Mathf.Clamp(transform.position.y, yMin, yMax);
        transform.position = new Vector3(xPos, yPos, transform.position.z);

        timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && timer >= shotInterval)
        {
            Shoot();
            timer = 0f;
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0f)
            {
                blinkTimer = blinkInterval;
                if (spriteRenderer != null) spriteRenderer.enabled = !spriteRenderer.enabled;
            }
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
                if (spriteRenderer != null) spriteRenderer.enabled = true;
            }
        }
    }

    void Shoot() { 
        if (bulletPrefab != null) Instantiate(bulletPrefab, transform.position, Quaternion.identity); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("Enemy"))
        {
            if (isInvincible) return;

            if (collision.gameObject.CompareTag("EnemyBullet"))
            {
                Destroy(collision.gameObject);
            }
            PlayerDamaged();
        }
    }

    void PlayerDamaged()
    {
        lives--;
        UpdateLifeUI(); 

        if (lives > 0)
        {
            isInvincible = true;
            invincibleTimer = invincibleDuration;
            blinkTimer = blinkInterval;
        }
        else
        {
            // シーン名が正確に GameOverScene であることを確認してください
            SceneManager.LoadScene("GameOverScene");
        }
    }

    // ★この関数の定義がファイルから消えていたか、クラスの外に出ていたのがエラーの原因です
    void UpdateLifeUI()
    {
        if (lifeIcons == null) return;
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (lifeIcons[i] != null)
            {
                if (i < lives) lifeIcons[i].SetActive(true);
                else lifeIcons[i].SetActive(false);
            }
        }
    }
}
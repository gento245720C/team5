using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [Header("サウンド設定")]
    public AudioClip shotSound;
    public AudioClip damageSound;
    public AudioClip explosionSound;
    private AudioSource audioSource;

    [Header("音量調整 (0.0〜1.0)")]
    [Range(0, 1)] public float shotVolume = 0.3f;
    [Range(0, 1)] public float damageVolume = 0.8f;
    [Range(0, 1)] public float explosionVolume = 1.0f;

    [Header("ゲームオーバー演出")]
    public float gameOverDelay = 1.5f; 

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        UpdateLifeUI(); 
    }

    void Update()
    {
        // 死亡演出中（無効化中）は移動や射撃をさせない
        if (!spriteRenderer.enabled && lives <= 0) return;

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

    void Shoot() 
    { 
        if (bulletPrefab != null) 
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity); 
            if (shotSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shotSound, shotVolume);
            }
        }
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
            if (damageSound != null && audioSource != null)
                audioSource.PlayOneShot(damageSound, damageVolume);

            isInvincible = true;
            invincibleTimer = invincibleDuration;
            blinkTimer = blinkInterval;
        }
        else
        {
            StartCoroutine(GameOverRoutine());
        }
    }

    IEnumerator GameOverRoutine()
    {
        Debug.Log("自機爆発！");

        // 1. 爆発音を鳴らす
        if (explosionSound != null)
            AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, explosionVolume);

        // 2. 自機の見た目を消し、動けないようにする
        spriteRenderer.enabled = false;
        isInvincible = true; // 当たり判定を実質無効化

        // 3. 指定した秒数（gameOverDelay）だけ待機
        yield return new WaitForSeconds(gameOverDelay);

        // 4. シーンを切り替える
        SceneManager.LoadScene("GameOverScene");
    }

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
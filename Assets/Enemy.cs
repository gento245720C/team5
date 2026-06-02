using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("移動設定")]
    public float speed = 2f;
    public float descentSpeed = 0.5f;
    private int direction = 1;

    [Header("攻撃設定")]
    public GameObject enemyBulletPrefab;
    public float shotInterval = 3f;
    private float timer;

    [Header("弾の設定")]
    public float minShotInterval = 1.0f;
    public float maxShotInterval = 3.0f;
    private float currentShotInterval;
    public float minBulletSpeed = 2f;
    public float maxBulletSpeed = 8f;
    private float currentBulletSpeed;

    [Header("弾の大きさ設定")]
    public float minBulletScale = 0.2f;
    public float maxBulletScale = 0.8f;
    private float currentBulletScale;

    [Header("出現・移動範囲")]
    public float spawnYOffset = 1.0f;  
    public float minMoveXRange = 1.0f;
    public float maxMoveXRange = 2.3f;
    private float currentMoveXRange;

    [Header("敵本体のサイズ")]
    public float minScale = 0.5f;
    public float maxScale = 1.5f;

    [Header("体力・スコア設定")]
    public int maxHealth = 1;
    public int scoreValue = 100;
    private int currentHealth;

    [Header("サウンド設定")]
    public AudioClip killSound;
    [Range(0, 1)] public float killVolume = 1.0f; 

    void Start()
    {
        currentHealth = maxHealth;
        InitializeEnemy();
        timer = Random.Range(0f, shotInterval);
    }

    void Update()
    {
        float xMove = direction * speed * Time.deltaTime;
        float yMove = -descentSpeed * Time.deltaTime;
        transform.Translate(new Vector3(xMove, yMove, 0));

        if (transform.position.x > currentMoveXRange)
        {
            direction = -1;
            transform.position = new Vector3(currentMoveXRange, transform.position.y, 0);
        }
        else if (transform.position.x < -currentMoveXRange)
        {
            direction = 1;
            transform.position = new Vector3(-currentMoveXRange, transform.position.y, 0);
        }

        if (transform.position.y < -5.5f) { Destroy(gameObject); }

        if (IsOnScreen())
        {
            timer += Time.deltaTime;
            if (timer > currentShotInterval)
            {
                Shoot();
                timer = 0;
                currentShotInterval = Random.Range(minShotInterval, maxShotInterval);
            }
        }
    }

    bool IsOnScreen()
    {
        float cameraTop = Camera.main.transform.position.y + Camera.main.orthographicSize;
        return transform.position.y <= cameraTop;
    }

    void Shoot()
    {
        if (enemyBulletPrefab != null)
        {
            GameObject bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.localScale = new Vector3(currentBulletScale, currentBulletScale, 1);
            bullet.SendMessage("SetSpeed", currentBulletSpeed, SendMessageOptions.DontRequireReceiver);
        }
    }

    void InitializeEnemy()
    {
        float cameraTop = Camera.main.transform.position.y + Camera.main.orthographicSize + spawnYOffset;
        float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        currentMoveXRange = Random.Range(minMoveXRange, maxMoveXRange);
        float randomX = Random.Range(-cameraHalfWidth, cameraHalfWidth);
        float randomY = cameraTop;
        transform.position = new Vector3(randomX, randomY, 0);

        float randomSize = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        currentBulletSpeed = Random.Range(minBulletSpeed, maxBulletSpeed);
        currentBulletScale = Random.Range(minBulletScale, maxBulletScale);
        currentShotInterval = Random.Range(minShotInterval, maxShotInterval);
        
        speed = Random.Range(1f, 4f);
        descentSpeed = Random.Range(0.3f, 1.2f);
        timer = 0;
        direction = (Random.value > 0.5f) ? 1 : -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            int damage = bullet != null ? bullet.attackPower : 1;

            Destroy(collision.gameObject);
            TakeDamage(damage);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth > 0) return;

        // PlayOneShot形式なので、大量に倒しても音が消えなくなります
        if (killSound != null && StageManager.Instance != null)
        {
            StageManager.Instance.PlaySE(killSound, killVolume);
        }

        Debug.Log("敵を撃破！");
        StageManager.Instance?.AddScore(scoreValue);
        Destroy(gameObject);
    }
}

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
    public float minSpawnYPos = 4.0f;
    public float maxSpawnYPos = 6.0f;
    public float minMoveXRange = 1.0f;
    public float maxMoveXRange = 2.3f;
    private float currentMoveXRange;

    [Header("敵本体のサイズ")]
    public float minScale = 0.5f;
    public float maxScale = 1.5f;

    void Start()
    {
        InitializeEnemy();
        // 敵ごとに発射タイミングをバラつかせる
        timer = Random.Range(0f, shotInterval);
    }

    void Update()
    {
        // 1. 移動処理
        float xMove = direction * speed * Time.deltaTime;
        float yMove = -descentSpeed * Time.deltaTime;
        transform.Translate(new Vector3(xMove, yMove, 0));

        // 2. 端での反転処理
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

        // 3. 画面下部での消滅処理
        if (transform.position.y < -5.5f) 
        { 
            Destroy(gameObject); 
        }

        // 4. 射撃処理
        timer += Time.deltaTime;
        if (timer > currentShotInterval)
        {
            Shoot();
            timer = 0;
            // 次の射撃間隔をランダムに設定
            currentShotInterval = Random.Range(minShotInterval, maxShotInterval);
        }
    }

    void Shoot()
    {
        if (enemyBulletPrefab != null)
        {
            GameObject bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
            
            // ★弾の大きさをセットする
            bullet.transform.localScale = new Vector3(currentBulletScale, currentBulletScale, 1);

            // 弾のスピードをセット（スクリプトが HomingBullet でも Bullet でも動くように SendMessage を使う）
            bullet.SendMessage("SetSpeed", currentBulletSpeed, SendMessageOptions.DontRequireReceiver);
        }
    }

    void InitializeEnemy()
    {
        currentMoveXRange = Random.Range(minMoveXRange, maxMoveXRange);
        float randomX = Random.Range(-currentMoveXRange, currentMoveXRange);
        float randomY = Random.Range(minSpawnYPos, maxSpawnYPos);
        transform.position = new Vector3(randomX, randomY, 0);

        float randomSize = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        // 弾の設定をランダムに決める
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
        // 自機の弾（Bulletタグ）に当たった場合
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("敵を撃破！");
            Destroy(collision.gameObject); // 当たった自機の弾を消す
            Destroy(gameObject);           // 自分（敵）を消す
        }
    }
}
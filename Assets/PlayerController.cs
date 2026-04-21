using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    // ★追加：弾の設計図（プレハブ）を格納する変数
    public GameObject bulletPrefab;

    void Update()
    {
        // 左右の移動
        float move = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * move * speed * Time.deltaTime);

        // ★追加：スペースキーが押されたら弾を生成する
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate（インスタンシエイト）は、オブジェクトの実体を作る関数です
        // 第1引数：作るもの（弾のプレハブ）
        // 第2引数：場所（今の自分の位置）
        // 第3引数：回転（そのまま）
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }
    // ★追加：敵の弾が当たった時の処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たった相手のタグが「EnemyBullet」だったら
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("ゲームオーバー！"); // コンソールに文字を出す
            Destroy(collision.gameObject); // 敵の弾を消す
            Destroy(gameObject);           // 自分（自機）を消す

            // 本来はここに「ゲームオーバー画面」を出す処理などを書きます
            // 今回はとりあえず自機が消えるところまで実装します
        }
    }
}
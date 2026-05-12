using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject bulletPrefab;

    // 画面の移動制限（この数値を調整して止まる位置を決めます）
    public float xMin = -2.5f;
    public float xMax = 2.5f;
    public float yMin = -4f;
    public float yMax = 2.5f;

    void Update()
    {
        // --- 移動の処理 ---
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 moveDir = new Vector2(moveX, moveY);
        transform.Translate(moveDir * speed * Time.deltaTime);

        // ★追加：画面外に出ないように座標を制限する処理
        Vector3 pos = transform.position; // 現在の位置を取得

        // x座標を xMin から xMax の間に収める
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        // y座標を yMin から yMax の間に収める
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);

        transform.position = pos; // 制限した後の座標を自分に反映


        // --- 発射の処理 ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
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
            Debug.Log("ゲームオーバー！");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}

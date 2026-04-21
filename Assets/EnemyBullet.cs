using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f; // 下向きの速度

    void Update()
    {
        // 下方向に移動
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        // 画面外（下端）に行ったら消す
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}
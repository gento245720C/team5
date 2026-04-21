using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        // 上に飛んでいく
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // 画面の外（上側）に出たら自動で消えるようにする
        if (transform.position.y > 6.0f)
        {
            Destroy(gameObject);
        }
    }
}
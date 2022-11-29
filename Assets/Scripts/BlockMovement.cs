using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    public float moveSpeed;
    public float destroyTime;
    void FixedUpdate()
    {
        this.transform.Translate(new Vector2(-moveSpeed * Time.deltaTime, 0));
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0)
            Destroy(gameObject);
    }
}

using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Rigidbody2D rb;
    public int health = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int demage)
    {
        health -= demage;

        if(health < 0)
            Die();
    }

    void Update()
    {
        if (Physics2D.gravity.y > 0)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 270);
    }

    void FixedUpdate()
    {
        rb.angularVelocity = 0f;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}

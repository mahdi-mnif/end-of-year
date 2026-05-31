using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30f;
    public float damage = 20f;
    public float lifetime = 3f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }


}

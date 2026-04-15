using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Thrust = 10.0f;

    public int Damage = 1;

    public float Life = 10f;

    private GameObject lastHit = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Life -= Time.deltaTime;
        if(Life < 0)
        {
            Break();
        }
    }

    public void ThrowProjectile(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction * Thrust, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && collision.gameObject != lastHit)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
            lastHit = collision.gameObject;
        }
    }

    private void Break()
    {
        Destroy(this.gameObject);
    }
}

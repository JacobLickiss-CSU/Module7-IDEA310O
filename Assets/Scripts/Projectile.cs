using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Thrust = 10.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowProjectile(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction * Thrust, ForceMode.Impulse);
    }
}

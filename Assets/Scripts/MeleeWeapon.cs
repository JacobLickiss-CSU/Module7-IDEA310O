using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public int Damage = 1;

    public bool active = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (active && collider.tag == "Enemy")
        {
            collider.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
            active = false;
        }
    }

    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }
}

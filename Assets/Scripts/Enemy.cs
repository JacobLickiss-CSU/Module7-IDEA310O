using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool BehaviorThrow = false;

    public bool BehaviorSpawn = false;

    public int Health = 1;

    public int ThrowDamage = 1;

    public float ThrowRange = 1f;

    public float ThrowForce = 20f;

    public float ThrowCooldown = 3f;

    public float SpawnSleep = 1f;

    public float SpawnRange = 20f;

    public float SpawnCooldown = 3f;

    public Vector3 SpawnForce = new Vector3(0, 10f, 0);

    public Transform SpawnLocation = null;

    public GameObject SpawnObject = null;

    public GameObject BreakObject = null;

    public float BreakForce = 2f;

    // TODO break sound

    private float ThrowingCooldown = 0f;
    public bool IsThrowing
    {
        get
        {
            return ThrowingCooldown > 0;
        }
    }

    private float SleepTimer = 1f;

    private bool WasSpawned = false;

    private float SpawnTimer = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SleepTimer = SpawnSleep;
        SpawnTimer = SpawnCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        SleepTimer -= Time.deltaTime;
        if(SleepTimer <= 0)
        {
            if (BehaviorThrow && !IsThrowing && PlayerManager.Instance != null)
            {
                if (Vector3.Distance(PlayerManager.Instance.MainCamera.transform.position, transform.position) < ThrowRange || WasSpawned)
                {
                    ThrowingCooldown = ThrowCooldown;
                    GetComponent<Rigidbody>().AddForce(Vector3.Normalize(PlayerManager.Instance.MainCamera.transform.position - transform.position) * ThrowForce, ForceMode.Impulse);
                    WasSpawned = false; // It was still spawned of course, but we no longer need the special case
                }
            }

            if (IsThrowing)
            {
                ThrowingCooldown -= Time.deltaTime;
            }

            if (BehaviorSpawn && PlayerManager.Instance != null)
            {
                SpawnTimer -= Time.deltaTime;
                if (SpawnTimer <= 0 && Vector3.Distance(PlayerManager.Instance.MainCamera.transform.position, transform.position) < SpawnRange)
                {
                    Spawn();

                    SpawnTimer = SpawnCooldown;
                }
            }
        }
    }

    public void Spawn()
    {
        GameObject spawn = Instantiate(SpawnObject);
        spawn.transform.position = SpawnLocation.position;
        spawn.transform.rotation = SpawnLocation.rotation;
        spawn.GetComponent<Enemy>().SetSpawned();
        spawn.GetComponent<Rigidbody>().AddForce(SpawnForce, ForceMode.Impulse);
    }

    public void SetSpawned()
    {
        WasSpawned = true;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            Break();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(IsThrowing)
        {
            if (collision.gameObject.tag == "Player")
            {
                PlayerManager.Instance.TakeDamage(ThrowDamage);
                Break();
            }
        }
    }

    private void Break()
    {
        // TODO Play break sound
        if (BreakObject != null)
        {
            GameObject broken = Instantiate(BreakObject);
            broken.transform.position = transform.position;
            broken.transform.rotation = transform.rotation;
            if(GetComponent<Rigidbody>() != null)
            {
                TransferForce(broken);
            }
        }
        Destroy(this.gameObject);
    }

    private void TransferForce(GameObject shatter)
    {
        Vector3 force = GetComponent<Rigidbody>().linearVelocity;
        foreach(Transform sherd in shatter.transform)
        {
            Vector3 variation = new Vector3(
                Random.Range(-BreakForce * force.magnitude, BreakForce * force.magnitude),
                Random.Range(-BreakForce * force.magnitude, BreakForce * force.magnitude),
                Random.Range(-BreakForce * force.magnitude, BreakForce * force.magnitude)
            );
            sherd.gameObject.GetComponent<Rigidbody>().AddForce(force + variation, ForceMode.Impulse);
        }
    }
}

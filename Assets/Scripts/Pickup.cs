using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int Health = 0;

    public int RockAmmo = 0;

    public int PistolAmmo = 0;

    public int ShotgunAmmo = 0;

    public bool UnlockPistol;

    public bool UnlockShotgun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * 30, 0, Space.World);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            ConsumePickup();
        }
    }

    public void ConsumePickup()
    {
        PlayerManager.Instance.CurrentHealth += Health;
        if(PlayerManager.Instance.CurrentHealth > PlayerManager.Instance.MaxHealth)
        {
            PlayerManager.Instance.CurrentHealth = PlayerManager.Instance.MaxHealth;
        }
        PlayerManager.Instance.RocksAmmo += RockAmmo;
        PlayerManager.Instance.PistolAmmo += PistolAmmo;
        PlayerManager.Instance.ShotgunAmmo += ShotgunAmmo;
        if(UnlockPistol)
        {
            PlayerManager.Instance.HasPistol = true;
        }
        if (UnlockShotgun)
        {
            PlayerManager.Instance.HasShotgun = true;
        }

        // TODO create effect objects
        
        Destroy(this.gameObject);
    }
}

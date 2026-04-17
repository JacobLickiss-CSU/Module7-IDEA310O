using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int Health = 0;

    public int RockAmmo = 0;

    public int PistolAmmo = 0;

    public int ShotgunAmmo = 0;

    public bool UnlockPistol;

    public bool UnlockShotgun;

    public SoundPlayer SoundPickup;

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
            PlayerManager.Instance.PistolLoaded = PlayerManager.Instance.PistolCapacity;
            PlayerManager.Instance.CurrentWeapon = Weapon.Pistol;
        }
        if (UnlockShotgun)
        {
            PlayerManager.Instance.HasShotgun = true;
            PlayerManager.Instance.ShotgunLoaded = PlayerManager.Instance.ShotgunCapacity;
            PlayerManager.Instance.CurrentWeapon = Weapon.Shotgun;
        }

        if (SoundPickup != null) Instantiate(SoundPickup.gameObject);

        Destroy(this.gameObject);
    }
}

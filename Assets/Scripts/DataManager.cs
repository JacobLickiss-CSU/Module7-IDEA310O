using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public int DefaultHealth { get; internal set; } = 100;

    public int DefaultRocksAmmo { get; internal set; } = 0;

    public int DefaultPistolAmmo { get; internal set; } = 0;

    public int DefaultShotgunAmmo { get; internal set; } = 0;

    public int DefaultRocksLoaded { get; internal set; } = 1;

    public int DefaultPistolLoaded { get; internal set; } = 0;

    public int DefaultShotgunLoaded { get; internal set; } = 0;

    public bool DefaultHasPistol { get; internal set; } = false;

    public bool DefaultHasShotgun { get; internal set; } = false;

    public Weapon DefaultWeapon { get; internal set; } = Weapon.Trowel;

    public string DefaultLevel { get; internal set; } = "Level1";

    public int SavedHealth { get; set; }

    public int SavedRocksAmmo { get; set; }

    public int SavedPistolAmmo { get; set; }

    public int SavedShotgunAmmo { get; set; }

    public int SavedRocksLoaded { get; set; }

    public int SavedPistolLoaded { get; set; }

    public int SavedShotgunLoaded { get; set; }

    public bool SavedHasPistol { get; set; }

    public bool SavedHasShotgun { get; set; }

    public Weapon SavedWeapon { get; set; }

    public string SavedLevel { get; set; } = "Level1";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            RestoreDefaults();
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RestoreDefaults()
    {
        SavedHealth = DefaultHealth;
        SavedRocksAmmo = DefaultRocksAmmo;
        SavedPistolAmmo = DefaultPistolAmmo;
        SavedShotgunAmmo = DefaultShotgunAmmo;
        SavedRocksLoaded = DefaultRocksLoaded;
        SavedPistolLoaded = DefaultPistolLoaded;
        SavedShotgunLoaded = DefaultShotgunLoaded;
        SavedHasPistol = DefaultHasPistol;
        SavedHasShotgun = DefaultHasShotgun;
        SavedWeapon = DefaultWeapon;
        SavedLevel = DefaultLevel;
    }

    public void LoadState(bool loadScene = false)
    {
        if (loadScene)
        {
            SceneManager.LoadScene(SavedLevel);
        }

        if (PlayerManager.Instance == null) return;

        PlayerManager player = PlayerManager.Instance;

        player.CurrentHealth = SavedHealth;
        player.RocksAmmo = SavedRocksAmmo;
        player.PistolAmmo = SavedPistolAmmo;
        player.ShotgunAmmo = SavedShotgunAmmo;
        player.RocksLoaded = SavedRocksLoaded;
        player.PistolLoaded = SavedPistolLoaded;
        player.ShotgunLoaded = SavedShotgunLoaded;
        player.HasPistol = SavedHasPistol;
        player.HasShotgun = SavedHasShotgun;
        player.CurrentWeapon = SavedWeapon;
        player.ShowWeapon();
        player.DisplayRocks();
    }

    public void SaveState(string scene = "")
    {
        if (PlayerManager.Instance == null) return;

        PlayerManager player = PlayerManager.Instance;

        SavedHealth = player.CurrentHealth;
        SavedRocksAmmo = player.RocksAmmo;
        SavedPistolAmmo = player.PistolAmmo;
        SavedShotgunAmmo = player.ShotgunAmmo;
        SavedRocksLoaded = player.RocksLoaded;
        SavedPistolLoaded = player.PistolLoaded;
        SavedShotgunLoaded = player.ShotgunLoaded;
        SavedHasPistol = player.HasPistol;
        SavedHasShotgun = player.HasShotgun;
        SavedWeapon = player.CurrentWeapon;
        SavedLevel = scene == "" ? SceneManager.GetActiveScene().name : scene;
    }
}

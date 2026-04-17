using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponMonitor : MonoBehaviour
{
    public RawImage Panel;

    public TextMeshProUGUI LabelCurrentAmmo = null;

    public TextMeshProUGUI LabelMaxAmmo = null;

    public RawImage Icon = null;

    public Texture IconImage = null;

    public Texture UnlockedPanel;

    public Texture LockedPanelPistol;

    public Texture LockedPanelShotgun;

    public Weapon WatchedWeapon;

    public Vector3 TargetPosition;

    private Vector3 NormalPosition;

    public Vector3 LargeSize = new Vector3(1.3f, 1.3f, 1.3f);

    public Vector3 NormalSize = new Vector3(1.0f, 1.0f, 1.0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NormalPosition = GetComponent<RectTransform>().anchoredPosition3D;
        TargetPosition = NormalPosition + TargetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance != null)
        {
            SetIcon();
            SetPanel();
            UpdateAmmo();
            SetSize();
        }
    }

    private void SetIcon()
    {
        if(Icon != null && IconImage != null && PlayerManager.Instance != null)
        {
            if (WatchedWeapon == Weapon.Pistol || WatchedWeapon == Weapon.Shotgun)
            {
                if (WatchedWeapon == Weapon.Pistol && PlayerManager.Instance.HasPistol)
                {
                    Icon.texture = IconImage;
                }
                if (WatchedWeapon == Weapon.Shotgun && PlayerManager.Instance.HasShotgun)
                {
                    Icon.texture = IconImage;
                }
            }
            else
            {
                Icon.texture = IconImage;
            }
        }
    }

    private void SetPanel()
    {
        if(WatchedWeapon == Weapon.Pistol)
        {
            Panel.texture = PlayerManager.Instance.HasPistol ? UnlockedPanel : LockedPanelPistol;
        }
        if (WatchedWeapon == Weapon.Shotgun)
        {
            Panel.texture = PlayerManager.Instance.HasShotgun ? UnlockedPanel : LockedPanelShotgun;
        }
    }

    private void UpdateAmmo()
    {
        switch(WatchedWeapon)
        {
            case Weapon.Trowel:
                {
                    LabelCurrentAmmo.text = "";
                    LabelMaxAmmo.text = "";
                    break;
                }
            case Weapon.Rocks:
                {
                    LabelCurrentAmmo.text = "" + PlayerManager.Instance.RocksLoaded;
                    LabelMaxAmmo.text = "" + PlayerManager.Instance.RocksAmmo;
                    break;
                }
            case Weapon.Pistol:
                {
                    if(PlayerManager.Instance.HasPistol)
                    {
                        LabelCurrentAmmo.text = "" + PlayerManager.Instance.PistolLoaded;
                        LabelMaxAmmo.text = "" + PlayerManager.Instance.PistolAmmo;
                    }
                    else
                    {
                        LabelCurrentAmmo.text = "";
                        LabelMaxAmmo.text = "";
                    }
                    break;
                }
            case Weapon.Shotgun:
                {
                    if(PlayerManager.Instance.HasShotgun)
                    {
                        LabelCurrentAmmo.text = "" + PlayerManager.Instance.ShotgunLoaded;
                        LabelMaxAmmo.text = "" + PlayerManager.Instance.ShotgunAmmo;
                    }
                    else
                    {
                        LabelCurrentAmmo.text = "";
                        LabelMaxAmmo.text = "";
                    }
                    break;
                }
        }
    }

    private void SetSize()
    {
        if(PlayerManager.Instance.CurrentWeapon == WatchedWeapon)
        {
            GetComponent<RectTransform>().localScale = Vector3.Lerp(GetComponent<RectTransform>().localScale, LargeSize, Time.deltaTime * 5f);
            if (TargetPosition != null)
            {
                GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(GetComponent<RectTransform>().anchoredPosition3D, TargetPosition, Time.deltaTime * 5f);
            }
        }
        else
        {
            GetComponent<RectTransform>().localScale = Vector3.Lerp(GetComponent<RectTransform>().localScale, NormalSize, Time.deltaTime * 5f);
            GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(GetComponent<RectTransform>().anchoredPosition3D, NormalPosition, Time.deltaTime * 5f);
        }
    }
}

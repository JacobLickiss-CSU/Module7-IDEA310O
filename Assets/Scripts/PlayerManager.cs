using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public int MaxHealth = 100;

    public int CurrentHealth = 100;

    public int RocksAmmo = 0;

    public int PistolAmmo = 0;

    public int ShotgunAmmo = 0;

    public int RocksCapacity = 3;

    public int PistolCapacity = 8;

    public int ShotgunCapacity = 2;

    public int RocksLoaded = 0;

    public int PistolLoaded = 0;

    public int ShotgunLoaded = 0;

    public bool HasPistol = false;

    public bool HasShotgun = false;

    public Weapon CurrentWeapon = Weapon.Trowel;

    public PlayerState CurrentState = PlayerState.Ready;

    public float MeleeAttackTime = 1.0f;

    public float RocksAttackTime = 1.0f;

    public float PistolAttackTime = 0.5f;

    public float ShotgunAttackTime = 1.0f;

    public float MeleeAttackDelay = 0.05f;

    public float RocksAttackDelay = 0.2f;

    public float PistolAttackDelay = 0.0f;

    public float ShotgunAttackDelay = 0.0f;

    public float RocksReloadTime = 2.0f;

    public float PistolReloadTime = 2.0f;

    public float ShotgunReloadTime = 2.0f;

    public int MeleeDamage = 1;

    public int RockDamage = 1;

    public int PistolDamage = 2;

    public int ShotgunDamage = 5;

    public float PistolRange = 40f;

    public float ShotgunRange = 20f;

    public GameObject MainCamera;

    public GameObject TrowelDisplay;

    public GameObject RocksDisplay;

    public GameObject PistolDisplay;

    public GameObject ShotgunDisplay;

    public GameObject RockProjectile;

    public GameObject ThrownRock;

    public bool IsPaused = false;

    public GameObject PauseMenu;

    public PlayerInput PlayerInput;

    public SoundPlayer SoundTrowelFire;

    public SoundPlayer SoundTrowelReady;

    public SoundPlayer SoundRocksFire;

    public SoundPlayer SoundRocksReload;

    public SoundPlayer SoundRocksReady;

    public SoundPlayer SoundPistolFire;

    public SoundPlayer SoundPistolReload;

    public SoundPlayer SoundPistolReady;

    public SoundPlayer SoundShotgunFire;

    public SoundPlayer SoundShotgunReload;

    public SoundPlayer SoundShotgunReady;

    public AudioSource MusicPlayer = null;

    public List<LevelMusic> LevelMusic = new List<LevelMusic>();

    public float musicTimer = 0f;

    public float MusicVolume = .5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Use a static instance for convenient access
        // because we only need one player.
        Instance = this;

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Load the state
        DataManager.Instance.LoadState();
        ShowWeapon();

        // Unpause, if we're paused
        Continue();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAttack();
        SelectWeapon();
        CheckPause();
        ProcessMusic();
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if(CurrentHealth <= 0 )
        {
            CurrentHealth = 0;
            GameOver();
        }
    }

    void GameOver()
    {
        CurrentState = PlayerState.Dead;
        // TODO game over screen?
        DataManager.Instance.LoadState(true);
    }

    void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(IsPaused)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }
    }

    void SelectWeapon()
    {
        if(CurrentState == PlayerState.Ready && !IsPaused)
        {
            if(Input.mouseScrollDelta.y > 0)
            {
                EquipNextWeapon(false);
            }
            else if(Input.mouseScrollDelta.y < 0)
            {
                EquipNextWeapon(true);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CurrentWeapon = Weapon.Trowel;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CurrentWeapon = Weapon.Rocks;
            }

            if (HasPistol && Input.GetKeyDown(KeyCode.Alpha3))
            {
                CurrentWeapon = Weapon.Pistol;
            }

            if (HasShotgun && Input.GetKeyDown(KeyCode.Alpha4))
            {
                CurrentWeapon = Weapon.Shotgun;
            }

            ShowWeapon();
        
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }
    }

    void EquipNextWeapon(bool backwards)
    {
        int targetWeapon = ((int)CurrentWeapon + (backwards ? -1 : 1));
        int weaponCount = Enum.GetValues(typeof(Weapon)).Length;

        if (targetWeapon < 0)
        {
            targetWeapon = weaponCount + targetWeapon;
        }
        CurrentWeapon = (Weapon) (targetWeapon % weaponCount);

        if(CurrentWeapon == Weapon.Pistol && !HasPistol)
        {
            EquipNextWeapon(backwards);
        }

        if (CurrentWeapon == Weapon.Shotgun && !HasShotgun)
        {
            EquipNextWeapon(backwards);
        }
    }

    public void ShowWeapon()
    {
        if(TrowelDisplay != null)
        {
            if (CurrentWeapon == Weapon.Trowel && !TrowelDisplay.activeSelf)
            {
                if (SoundTrowelReady != null) Instantiate(SoundTrowelReady.gameObject);
            }
            TrowelDisplay.SetActive(CurrentWeapon == Weapon.Trowel);
        }
        if (RocksDisplay != null)
        {
            if (CurrentWeapon == Weapon.Rocks && !RocksDisplay.activeSelf)
            {
                if (SoundRocksReady != null) Instantiate(SoundRocksReady.gameObject);
            }
            RocksDisplay.SetActive(CurrentWeapon == Weapon.Rocks);
        }
        if (PistolDisplay != null)
        {
            if(CurrentWeapon == Weapon.Pistol && !PistolDisplay.activeSelf)
            {
                if (SoundPistolReady != null) Instantiate(SoundPistolReady.gameObject);
            }
            PistolDisplay.SetActive(CurrentWeapon == Weapon.Pistol);
        }
        if (ShotgunDisplay != null)
        {
            if (CurrentWeapon == Weapon.Shotgun && !ShotgunDisplay.activeSelf)
            {
                if (SoundShotgunReady != null) Instantiate(SoundShotgunReady.gameObject);
            }
            ShotgunDisplay.SetActive(CurrentWeapon == Weapon.Shotgun);
        }
    }

    IEnumerator HoldState(PlayerState state, float time, PlayerState releaseState = PlayerState.Ready)
    {
        CurrentState = state;
        yield return new WaitForSeconds(time);
        CurrentState = releaseState;
    }

    void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1") && !IsPaused)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (CurrentState == PlayerState.Ready)
        {
            switch (CurrentWeapon)
            {
                case Weapon.Trowel:
                    {
                        Animation displayAnimation = TrowelDisplay.GetComponent<Animation>();
                        StartCoroutine(StartAttack(MeleeAttackTime, MeleeAttackDelay, displayAnimation, GetAttackVariant(Weapon.Trowel), Weapon.Trowel));
                        break;
                    }
                case Weapon.Rocks:
                    {
                        if (RocksLoaded > 0)
                        {
                            RocksLoaded--;
                            Animation displayAnimation = RocksDisplay.GetComponent<Animation>();
                            StartCoroutine(StartAttack(RocksAttackTime, RocksAttackDelay, displayAnimation, GetAttackVariant(Weapon.Rocks), Weapon.Rocks));
                        }
                        else
                        {
                            Reload();
                        }
                        break;
                    }
                case Weapon.Pistol:
                    {
                        if (PistolLoaded > 0)
                        {
                            PistolLoaded--;
                            Animation displayAnimation = PistolDisplay.GetComponent<Animation>();
                            StartCoroutine(StartAttack(PistolAttackTime, PistolAttackDelay, displayAnimation, GetAttackVariant(Weapon.Pistol), Weapon.Pistol));
                        }
                        else
                        {
                            Reload();
                        }
                        break;
                    }
                case Weapon.Shotgun:
                    {
                        if (ShotgunLoaded > 0)
                        {
                            ShotgunLoaded--;
                            Animation displayAnimation = ShotgunDisplay.GetComponent<Animation>();
                            StartCoroutine(StartAttack(ShotgunAttackTime, ShotgunAttackDelay, displayAnimation, GetAttackVariant(Weapon.Shotgun), Weapon.Shotgun));
                        }
                        else
                        {
                            Reload();
                        }
                        break;
                    }
            }
        }
    }

    string GetAttackVariant(Weapon weapon)
    {
        switch (weapon)
        {
            case Weapon.Trowel:
                {
                    return "TrowelAttack";
                }
            case Weapon.Rocks:
                {
                    if(RocksLoaded == 2)
                    {
                        return "RockAttack3";
                    }
                    if (RocksLoaded == 1)
                    {
                        return "RockAttack2";
                    }
                    if (RocksLoaded == 0)
                    {
                        return "RockAttack1";
                    }
                    return "RockAttack3";
                }
            case Weapon.Pistol:
                {
                    return "PistolAttack";
                }
            case Weapon.Shotgun:
                {
                    return "ShotgunAttack";
                }
        }

        return "";
    }

    IEnumerator StartAttack(float attackTime, float attackDelay, Animation animation, string animationName, Weapon weapon)
    {
        CurrentState = PlayerState.Attacking;
        animation.Play(animationName);
        yield return new WaitForSeconds(attackDelay);
        ExecuteAttack(weapon);
        yield return StartCoroutine(HoldState(PlayerState.Attacking, attackTime - attackDelay));
        EndAttack(weapon);
    }

    void ExecuteAttack(Weapon weapon)
    {
        // TODO
        switch (weapon)
        {
            case Weapon.Trowel:
                {
                    TrowelDisplay.GetComponent<MeleeWeapon>().Damage = MeleeDamage;
                    TrowelDisplay.GetComponent<MeleeWeapon>().Activate();
                    if (SoundTrowelFire != null) Instantiate(SoundTrowelFire.gameObject);
                    break;
                }
            case Weapon.Rocks:
                {
                    Transform projectileLocation = RocksDisplay.transform.GetChild(RocksLoaded);
                    ThrownRock = Instantiate(RockProjectile);
                    ThrownRock.transform.position = projectileLocation.position;
                    ThrownRock.transform.rotation = projectileLocation.rotation;
                    RocksDisplay.transform.GetChild(RocksLoaded).gameObject.SetActive(false);
                    ThrownRock.GetComponent<Projectile>().ThrowProjectile(MainCamera.transform.forward + new Vector3(0, .05f, 0));
                    if (SoundRocksFire != null) Instantiate(SoundRocksFire.gameObject);

                    break;
                }
            case Weapon.Pistol:
                {
                    RaycastAttack(MainCamera.transform.position, MainCamera.transform.forward, PistolDamage, PistolRange);
                    if(SoundPistolFire != null) Instantiate(SoundPistolFire.gameObject);
                    break;
                }
            case Weapon.Shotgun:
                {
                    RaycastAttack(MainCamera.transform.position, MainCamera.transform.forward, ShotgunDamage, ShotgunRange);
                    if (SoundShotgunFire != null) Instantiate(SoundShotgunFire.gameObject);
                    break;
                }
        }
    }

    void RaycastAttack(Vector3 position, Vector3 direction, int damage, float range)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + direction, direction, out hit, range))
        {
            if(hit.collider.gameObject.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(direction * damage * 5, ForceMode.Impulse);
                hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }

    void EndAttack(Weapon weapon)
    {
        switch (weapon)
        {
            case Weapon.Trowel:
                {
                    TrowelDisplay.GetComponent<MeleeWeapon>().Deactivate();
                    break;
                }
            case Weapon.Rocks:
                {
                    break;
                }
            case Weapon.Pistol:
                {
                    break;
                }
            case Weapon.Shotgun:
                {
                    break;
                }
        }
    }

    void Reload()
    {
        if(CurrentState == PlayerState.Ready)
        {
            switch(CurrentWeapon)
            {
                case Weapon.Trowel:
                    {
                        // Do nothing, can't reload a shovel
                        break;
                    }
                case Weapon.Rocks:
                    {
                        if (CanReload(Weapon.Rocks))
                        {
                            // Special case, needs processing
                            Animation displayAnimation = RocksDisplay.GetComponent<Animation>();
                            StartCoroutine(ReloadRocks(RocksReloadTime, displayAnimation));
                        }
                        break;
                    }
                case Weapon.Pistol:
                    {
                        if(CanReload(Weapon.Pistol))
                        {
                            if (SoundPistolReload != null) Instantiate(SoundPistolReload.gameObject);
                            Animation displayAnimation = PistolDisplay.GetComponent<Animation>();
                            StartCoroutine(StartReloading(PistolReloadTime, displayAnimation, GetReloadVariant(Weapon.Pistol), Weapon.Pistol));
                        }
                        break;
                    }
                case Weapon.Shotgun:
                    {
                        if (CanReload(Weapon.Shotgun))
                        {
                            if (SoundShotgunReload != null) Instantiate(SoundShotgunReload.gameObject);
                            Animation displayAnimation = ShotgunDisplay.GetComponent<Animation>();
                            StartCoroutine(StartReloading(ShotgunReloadTime, displayAnimation, GetReloadVariant(Weapon.Shotgun), Weapon.Shotgun));
                        }
                        break;
                    }
            }
        }
    }
    
    bool CanReload(Weapon weapon)
    {
        switch (weapon)
        {
            case Weapon.Trowel:
                {
                    return false;
                }
            case Weapon.Rocks:
                {
                    return RocksLoaded < RocksCapacity && RocksAmmo > 0;
                }
            case Weapon.Pistol:
                {
                    return PistolLoaded < PistolCapacity && PistolAmmo > 0;
                }
            case Weapon.Shotgun:
                {
                    return ShotgunLoaded < ShotgunCapacity && ShotgunAmmo > 0;
                }
        }

        return false;
    }

    string GetReloadVariant(Weapon weapon)
    {
        switch (weapon)
        {
            case Weapon.Trowel:
                {
                    return "";
                }
            case Weapon.Rocks:
                {
                    // TODO
                    return "";
                }
            case Weapon.Pistol:
                {
                    return "PistolReload";
                }
            case Weapon.Shotgun:
                {
                    if(ShotgunAmmo + ShotgunLoaded > 1)
                    {
                        return "ShotgunReload2";
                    }
                    else
                    {
                        return "ShotgunReload1";
                    }
                }
        }

        return "";
    }

    IEnumerator StartReloading(float reloadTime, Animation animation, string animationName, Weapon weapon)
    {
        CurrentState = PlayerState.Reloading;
        animation.Play(animationName);
        yield return StartCoroutine(HoldState(PlayerState.Reloading, reloadTime));
        PerformReload(weapon);
    }

    IEnumerator ReloadRocks(float reloadTime, Animation animation)
    {
        CurrentState = PlayerState.Reloading;
        if(RocksLoaded <= 0 && (RocksAmmo + RocksLoaded) >= 1)
        {
            if (SoundRocksReload != null) Instantiate(SoundRocksReload.gameObject);
            RocksDisplay.transform.GetChild(0).gameObject.SetActive(true);
            animation.Play("RockReload1");
            yield return new WaitForSeconds(reloadTime / 3);
        }
        if (RocksLoaded <= 1 && (RocksAmmo + RocksLoaded) >= 2)
        {
            if (SoundRocksReload != null) Instantiate(SoundRocksReload.gameObject);
            RocksDisplay.transform.GetChild(1).gameObject.SetActive(true);
            animation.Play("RockReload2");
            yield return new WaitForSeconds(reloadTime / 3);
        }
        if (RocksLoaded <= 2 && (RocksAmmo + RocksLoaded) >= 3)
        {
            if (SoundRocksReload != null) Instantiate(SoundRocksReload.gameObject);
            RocksDisplay.transform.GetChild(2).gameObject.SetActive(true);
            animation.Play("RockReload3");
            yield return new WaitForSeconds(reloadTime / 3);
        }
        CurrentState = PlayerState.Ready;
        PerformReload(Weapon.Rocks);
    }

    public void DisplayRocks()
    {
        RocksDisplay.transform.GetChild(0).gameObject.SetActive(RocksLoaded >= 1);
        RocksDisplay.transform.GetChild(1).gameObject.SetActive(RocksLoaded >= 2);
        RocksDisplay.transform.GetChild(2).gameObject.SetActive(RocksLoaded >= 3);
    }

    void PerformReload(Weapon weapon)
    {
        if (!CanReload(weapon)) return;

        switch (weapon)
        {
            case Weapon.Trowel:
                {
                    // Do nothing, can't reload a shovel
                    break;
                }
            case Weapon.Rocks:
                {
                    int ammoNeeded = RocksCapacity - RocksLoaded;
                    RocksAmmo -= ammoNeeded;
                    RocksLoaded = RocksCapacity;
                    if (RocksAmmo < 0)
                    {
                        RocksLoaded += RocksAmmo;
                        RocksAmmo = 0;
                    }
                    break;
                }
            case Weapon.Pistol:
                {
                    int ammoNeeded = PistolCapacity - PistolLoaded;
                    PistolAmmo -= ammoNeeded;
                    PistolLoaded = PistolCapacity;
                    if (PistolAmmo < 0)
                    {
                        PistolLoaded += PistolAmmo;
                        PistolAmmo = 0;
                    }
                    break;
                }
            case Weapon.Shotgun:
                {
                    int ammoNeeded = ShotgunCapacity - ShotgunLoaded;
                    ShotgunAmmo -= ammoNeeded;
                    ShotgunLoaded = ShotgunCapacity;
                    if (ShotgunAmmo < 0)
                    {
                        ShotgunLoaded += ShotgunAmmo;
                        ShotgunAmmo = 0;
                    }
                    break;
                }
        }
    }

    public void Pause()
    {
        // Release the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0.0f;

        if(PauseMenu != null)
        {
            PauseMenu.SetActive(true);
        }

        if(PlayerInput != null)
        {
            PlayerInput.actions.Disable();
        }

        if(MusicPlayer != null)
        {
            MusicPlayer.Pause();
        }

        IsPaused = true;
    }

    public void Continue()
    {
        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1.0f;

        if(PauseMenu != null)
        {
            PauseMenu.SetActive(false);
        }

        if (PlayerInput != null)
        {
            PlayerInput.actions.Enable();
        }

        if (MusicPlayer != null && musicTimer >= 0 && MusicPlayer.clip != null)
        {
            MusicPlayer.Play();
        }

        IsPaused = false;
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        DataManager.Instance.LoadState(true);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void RenewMusic()
    {
        if(MusicPlayer != null)
        {
            if(!MusicPlayer.isPlaying)
            {
                if(MusicPlayer.clip == null)
                {
                    MusicPlayer.clip = GetMusic();
                }
                
                if(MusicPlayer.clip != null)
                {
                    MusicPlayer.Play();
                }
            }

            musicTimer = musicTimer > 0 ? 15f : 18f;
            ProcessMusic();
        }
    }

    public void ProcessMusic()
    {
        musicTimer -= Time.deltaTime;

        if(MusicPlayer != null)
        {
            if (musicTimer > 15)
            {
                MusicPlayer.volume = ((3f - (musicTimer - 15)) / 3f) * MusicVolume;
            }
            else if (musicTimer < 3)
            {
                MusicPlayer.volume = (musicTimer / 3f) * MusicVolume;
            }
            else
            {
                MusicPlayer.volume = 1 * MusicVolume;
            }

            if (musicTimer <= 0)
            {
                musicTimer = 0;
                MusicPlayer.volume = 0;
            }
        }
    }

    public AudioClip GetMusic()
    {
        string level = SceneManager.GetActiveScene().name;

        foreach(LevelMusic levelMusic in LevelMusic)
        {
            if(levelMusic.level == level)
            {
                return levelMusic.music;
            }
        }

        return null;
    }
}

public enum Weapon
{
    Trowel,
    Rocks,
    Pistol,
    Shotgun
}

public enum PlayerState
{
    Unaware,
    Ready,
    Attacking,
    Reloading,
    Dead,
    Victory,
    Frozen
}

[Serializable]
public struct LevelMusic
{
    public string level;
    public AudioClip music;
}
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;



public class PlayerController : MonoBehaviour, IDamage, Iheal, IOpen, IPush
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] int HP;
    [SerializeField] int Speed;
    [SerializeField] int SprintMod;
    [SerializeField] int JumpSpeed;
    [SerializeField] int JumpMax;
    [SerializeField] int gravity;
    [SerializeField] int pushVelTime;

    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] gunStats startingGun;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] float bulletForce = 20f;
    [SerializeField] TMP_Text ammoText;

    [SerializeField] float recoilAmount = 0.1f;
    [SerializeField] float recoilSpeed = 10f;

    [SerializeField] GameObject gunModel;
   

    [SerializeField] AudioSource aud;
    [SerializeField] float reloadTime = 1.5f;
    [SerializeField] AudioClip reloadSound;


    bool isReloading = false;
    public bool isPowerWeapon;
    public float powerDuration = 10f;
    public AudioClip powerMusic;
    bool isInvincible = false;
    int originalDamage;
    AudioClip originalMusic;
    bool isSwinging = false;
    public float swingCooldown = 0.5f;
    float nextSwingTime = 0f;
    public Transform gun_Model;

    int jumpCount;
    int HPOrig;
    float shootTimer;
    int gunListPos;

    Vector3 moveDir;
    Vector3 playerVel;
    Vector3 pushVel;
    Vector3 gunStartPos;
    GameObject currentGun;
    Quaternion gunStartRot;
    private Vector3 platformVelocity;
    //Transform gunVisual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        spawnPlayer();
        aud = GetComponent<AudioSource>();

        if (startingGun != null)
        {
            getGunStats(startingGun);
        }
        gunStartPos = gun_Model.localPosition;
        gunStartRot = gun_Model.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        movement();
        sprint();
        updatePlayerUI();
        interact();

        if (!isSwinging && !isReloading)
        {
            gunModel.transform.localPosition = Vector3.Lerp(
                gunModel.transform.localPosition,
                gunStartPos,
                Time.deltaTime * recoilSpeed);
        }

        if (!isReloading)
        {
            gunModel.transform.localPosition = Vector3.Lerp(
                gunModel.transform.localPosition,
                gunStartPos,
                recoilSpeed * Time.deltaTime
            );
        }
    }

    public void spawnPlayer()
    {
        controller.transform.position = gameManager.instance.playerSpawnPos.transform.position;
        Physics.SyncTransforms();
        HP = HPOrig;
        updatePlayerUI();
    }

    void movement()
    {
        shootTimer += Time.deltaTime;

        pushVel = Vector3.Lerp(pushVel, Vector3.zero, pushVelTime * Time.deltaTime);

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist);
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }
        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move((moveDir * Speed + platformVelocity) * Time.deltaTime);

        Jump();
        controller.Move((playerVel + pushVel) * Time.deltaTime);

        playerVel.y -= gravity * Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && gunList.Count > 0 && gunList[gunListPos].ammoCur > 0 && shootTimer >= shootRate)
        {
            shoot();
        }

        selectGun();
        reload();

    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < JumpMax)
        {
            playerVel.y = JumpSpeed;
            jumpCount++;
        }
    }
    void sprint()
    {
        if (Input.GetButtonDown("sprint"))
        {
            Speed *= SprintMod;
        }
        else if (Input.GetButtonUp("sprint"))
        {
            Speed /= SprintMod;
        }
    }
    void shoot()
    {

        //if (isReloading) return;
        if (gunList.Count == 0) return;
        if (gunListPos >= gunList.Count) return;

        if (gunList[gunListPos].isPowerWeapon)
        {
            SwingBat();
            return;
        }


        shootTimer = 0;

        gunList[gunListPos].ammoCur--;
        aud.PlayOneShot(gunList[gunListPos].shootSound[Random.Range(0, gunList[gunListPos].shootSound.Length)], gunList[gunListPos].shootSoundVol);

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootDist, ~ignoreLayer))
        { 
            

            if (gunList[gunListPos].hitEffect != null)
                Instantiate(gunList[gunListPos].hitEffect, hit.point, Quaternion.identity);

            IDamage dmg = hit.collider.GetComponentInParent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
            else
            {
                Debug.Log("[SHOOT] no hit");
            }
        
        updateAmmoUI();
    }

    void SwingBat()
    {
        Debug.Log("BAT SWING");

        if (!isSwinging)
        {
            StartCoroutine(BatSwingAnim());
        }

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3f))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                IDamage dmg = hit.collider.GetComponent<IDamage>();

                if (dmg != null)
                {
                    dmg.takeDamage(shootDamage);
                }
            }
        }
    }

    void reload()
    {
        if (isReloading) return;
        if (Input.GetButtonDown("Reload") && !isReloading && gunList.Count > 0)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    IEnumerator BatSwingAnim()
    {
        isSwinging = true;

        float t = 0;

        Vector3 startPos = gunStartPos;
        Quaternion startRot = gunStartRot;


        Quaternion readyRot = Quaternion.Euler(60f, 100f, 20f);

        gun_Model.localRotation = readyRot;
        gun_Model.localPosition = startPos + new Vector3(0.7f, -0.4f, 0.2f);

        yield return new WaitForSeconds(0.05f);


        Quaternion hitRot = Quaternion.Euler(0f, -160f, -30f);


        bool didHitPause = false;

        while (t < 1)
        {
            t += Time.deltaTime * 4f;

            gun_Model.localRotation = Quaternion.Lerp(readyRot, hitRot, t);

            gun_Model.localPosition = Vector3.Lerp(
                startPos + new Vector3(0.2f, -0.2f, 0.1f),
                startPos + new Vector3(-1.2f, -0.2f, 0.3f),
                t
            );


            if (!didHitPause && t > 0.5f)
            {
                didHitPause = true;
                yield return new WaitForSeconds(0.05f);
            }

            yield return null;
        }


        float resetT = 0;

        while (resetT < 1)
        {
            resetT += Time.deltaTime * 6f;

            gun_Model.localPosition = Vector3.Lerp(
                gun_Model.localPosition,
                startPos,
                resetT
            );

            gun_Model.localRotation = Quaternion.Lerp(
                gun_Model.localRotation,
                startRot,
                resetT
            );

            yield return null;
        }

        gun_Model.localPosition = startPos;
        gun_Model.localRotation = startRot;

        isSwinging = false;
    }
    public void takeDamage(int amount)
    {
        if (isInvincible) return;
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(flashScreen());

        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }
    }
    public void healDamage(int healAmount)
    {
        HP += healAmount;

        if (HP > HPOrig)
        {
            HP = HPOrig;
        }


        updatePlayerUI();
    }

    IEnumerator flashScreen()
    {
        gameManager.instance.PlayerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.PlayerDamageFlash.SetActive(false);
    }

    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;

        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }
    public void getGunStats(gunStats gun)
    {
        gunList.Add(gun);
        gunListPos = gunList.Count - 1;

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDist;
        shootRate = gun.shootRate;



        gunModel.SetActive(true);

        changeGun();
        updateAmmoUI();

        if (gun.isPowerWeapon)
        {
            StartCoroutine(powerWeaponRoutine(gun));
        }
    }

    void changeGun()
    {
        foreach (Transform child in gun_Model.transform)
        {
            Destroy(child.gameObject);
        }

        currentGun = Instantiate(gunList[gunListPos].gunModel, gun_Model.transform);


        currentGun.transform.localPosition = Vector3.zero;
        currentGun.transform.localRotation = Quaternion.identity;

        shootPoint = currentGun.transform.Find("Shoot Point");

        if (shootPoint == null)
        {
            Debug.LogError("NO SHOOT POINT FOUND ON mEW GUN");
        }

        shootDamage = gunList[gunListPos].shootDamage;
        shootDist = gunList[gunListPos].shootDist;
        shootRate = gunList[gunListPos].shootRate;

        gunStartPos = gun_Model.localPosition;
        gunStartRot = gun_Model.localRotation;
        updateAmmoUI();
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gunListPos < gunList.Count -1)
        {
            gunListPos++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
        {
            gunListPos--;
            changeGun();
        }
    }

    public void getPushVel(Vector3 pushAmount)
    {
        pushVel += pushAmount;
    }

    void interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3f))
            {
                IPickup pickup = hit.collider.GetComponent<IPickup>();

                Debug.Log(hit.collider.name);
                if (pickup != null)
                {
                    pickup.pickup(this);
                }
            }
        }
    }
    void updateAmmoUI()
    {
        if (gunList.Count == 0) return;

        ammoText.text = $"{gunList[gunListPos].ammoCur} / {gunList[gunListPos].ammoMax}";
    }



    IEnumerator ReloadRoutine()
    {
        isReloading = true;

        Vector3 startPos = gun_Model.localPosition;
        Vector3 downPos = startPos + new Vector3(0, -1.5f, 0); // deeper drop

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 3f;
            gun_Model.localPosition = Vector3.Lerp(startPos, downPos, t);
            yield return null;
        }


        if (reloadSound != null)
        {
            aud.PlayOneShot(reloadSound);
        }


        yield return new WaitForSeconds(reloadTime * 0.8f);


        gunList[gunListPos].ammoCur = gunList[gunListPos].ammoMax;
        updateAmmoUI();

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 1.5f;
            gun_Model.localPosition = Vector3.Lerp(downPos, startPos, t);
            yield return null;
        }

        gun_Model.localPosition = startPos;

        isReloading = false;
    }

    IEnumerator powerWeaponRoutine(gunStats gun)
    {

        originalDamage = shootDamage;
        originalMusic = aud.clip;


        isInvincible = true;
        shootDamage = gun.shootDamage;


        if (gun.powerMusic != null)
        {
            aud.Stop();
            aud.clip = gun.powerMusic;
            aud.loop = true;
            aud.Play();
        }

        yield return new WaitForSeconds(gun.powerDuration);


        isInvincible = false;
        shootDamage = originalDamage;


        gunList.Remove(gun);

        if (gunList.Count > 0)
        {
            gunListPos = 0;
            changeGun();
        }
        else
        {
            foreach (Transform child in gun_Model.transform)
            {
                Destroy(child.gameObject);
            }
        }


        aud.Stop();
        aud.clip = originalMusic;
        aud.Play();
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("MovingPlatform"))
        {
            platformVelocity = hit.gameObject.GetComponent<sideToSidePlatform>().GetVelocity();
        }
        else
        {
            platformVelocity = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }
}

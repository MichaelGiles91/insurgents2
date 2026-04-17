using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;


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

    bool isReloading = false;

    int jumpCount;
    int HPOrig;
    float shootTimer;
    int gunListPos;

    Vector3 moveDir;
    Vector3 playerVel;
    Vector3 pushVel;
    Vector3 gunStartPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        spawnPlayer();

        if (startingGun != null)
        {
            getGunStats(startingGun);
        }
        gunStartPos = gunModel.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        movement();
        sprint();
        updatePlayerUI();
        interact();

        gunModel.transform.localPosition = Vector3.Lerp(
    gunModel.transform.localPosition,
    gunStartPos,
    Time.deltaTime * recoilSpeed);

        if (Input.GetMouseButtonDown(0))
        {
            shoot();
        }

        gunModel.transform.localPosition = Vector3.Lerp(
     gunModel.transform.localPosition,
     gunStartPos,
     recoilSpeed * Time.deltaTime
 );
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
        controller.Move(moveDir * Speed * Time.deltaTime);

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
        }else if (Input.GetButtonUp("sprint"))
        {
            Speed /= SprintMod;
        }
    }
    void shoot()
    {
        if (isReloading) return;
        if (gunList.Count == 0) return;
        if (gunListPos >= gunList.Count) return;

        shootTimer = 0;

        gunModel.transform.localPosition -= new Vector3(0, 0, recoilAmount);
        if (gunListPos >= gunList.Count)
            gunListPos = 0;
        aud.PlayOneShot(gunList[gunListPos].shootSound[Random.Range(0, gunList[gunListPos].shootSound.Length)], gunList[gunListPos].shootSoundVol);

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = Camera.main.transform.forward * bulletForce;

        gunList[gunListPos].ammoCur--;
        updateAmmoUI();

        Debug.Log("SHOOTING");
    }

    void reload()
    {
        if (Input.GetButtonDown("Reload") && gunList.Count > 0 && !isReloading)
        {
            StartCoroutine(reloadRoutine());
        }
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(flashScreen());

        if(HP <= 0)
        {
            gameManager.instance.youLose();
        }
    }
    public void healDamage(int healAmount)
    {
         HP += healAmount;

        if(HP > HPOrig )
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
    }

    void changeGun()
    {
        foreach (Transform child in gunModel.transform)
        {
            Destroy(child.gameObject);
        }

        GameObject newGun = Instantiate(gunList[gunListPos].gunModel, gunModel.transform);

        newGun.transform.localPosition = Vector3.zero;
        newGun.transform.localRotation = Quaternion.identity;

        shootPoint = newGun.transform.Find("Shoot Point");

        if (shootPoint == null)
        {
            Debug.LogError("NO SHOOT POINT FOUND ON NEW GUN");
        }

        shootDamage = gunList[gunListPos].shootDamage;
        shootDist = gunList[gunListPos].shootDist;
        shootRate = gunList[gunListPos].shootRate;
        updateAmmoUI();
    }

    void selectGun()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && gunListPos < gunList.Count -1)
        {
            gunListPos++;
            changeGun();
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
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

    IEnumerator reloadRoutine()
    {
        isReloading = true;

        Quaternion startRot = gunModel.transform.localRotation;
        Quaternion reloadRot = Quaternion.Euler(60f, 0f, 0f); // tilt down

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 5f;
            gunModel.transform.localRotation = Quaternion.Lerp(startRot, reloadRot, t);
            yield return null;
        }

        yield return new WaitForSeconds(reloadTime * 0.5f);

        gunList[gunListPos].ammoCur = gunList[gunListPos].ammoMax;
        updateAmmoUI();

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 5f;
            gunModel.transform.localRotation = Quaternion.Lerp(reloadRot, startRot, t);
            yield return null;
        }

        isReloading = false;
    }

}

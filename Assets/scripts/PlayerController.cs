using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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

    [SerializeField] int batDmg;
    [SerializeField] float batRange = 2f;
    [SerializeField] float swingRate = 0.6f;
    [SerializeField] GameObject batModel;

    [SerializeField] GameObject gunModel;

    [SerializeField] AudioSource aud;

    int jumpCount;
    int HPOrig;
    float shootTimer;
    int gunListPos;
    bool hasBat;
    bool isSwinging;
    float batTimer;

    Vector3 moveDir;
    Vector3 playerVel;
    Vector3 pushVel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        spawnPlayer();


    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        updatePlayerUI();

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
        batTimer += Time.deltaTime;

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

        if (Input.GetButtonDown("Fire1"))
        {
            if (hasBat && batTimer >= swingRate && !isSwinging)
            {
                StartCoroutine(BatSwing());
            }
            else if (gunList.Count > 0 && gunList[gunListPos].ammoCur > 0 && shootTimer >= shootRate)
            {
                shoot();
            }
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
        shootTimer = 0;

        gunList[gunListPos].ammoCur--;
        aud.PlayOneShot(gunList[gunListPos].shootSound[Random.Range(0, gunList[gunListPos].shootSound.Length)], gunList[gunListPos].shootSoundVol);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            Instantiate(gunList[gunListPos].hitEffect, hit.point, Quaternion.identity);

            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
    }

    void reload()
    {
        if (Input.GetButtonDown("Reload") && gunList.Count > 0)
        {
            gunList[gunListPos].ammoCur = gunList[gunListPos].ammoMax;
        }
    }
    public void takeDamage(int amount)
    {
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
    }
    public void getGunStats(gunStats gun)
    {
        gunList.Add(gun);
        gunListPos = gunList.Count - 1;

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDist;
        shootRate = gun.shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        changeGun();
    }

    void changeGun()
    {
        shootDamage = gunList[gunListPos].shootDamage;
        shootDist = gunList[gunListPos].shootDist;
        shootRate = gunList[gunListPos].shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPos].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[gunListPos].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gunListPos < gunList.Count - 1)
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
    IEnumerator BatSwing()
    {
        isSwinging = true;
        batTimer = 0f;

        Vector3 startPos = batModel.transform.localPosition;
        Quaternion startRot = batModel.transform.localRotation;

        Vector3 windupPos = startPos + new Vector3(0.08f, 0.05f, -0.08f);
        Quaternion windupRot = startRot * Quaternion.Euler(-20f, -10f, 15f);

        Vector3 hitPos = startPos + new Vector3(0.0f, -0.18f, 0.18f);
        Quaternion hitRot = startRot * Quaternion.Euler(55f, 0f, 0f);

        float t = 0f;

        // quick windup
        while (t < 1f)
        {
            t += Time.deltaTime * 14f;
            batModel.transform.localPosition = Vector3.Lerp(startPos, windupPos, t);
            batModel.transform.localRotation = Quaternion.Lerp(startRot, windupRot, t);
            yield return null;
        }

        t = 0f;
        bool didDamage = false;

        // fast bonk forward/down
        while (t < 1f)
        {
            t += Time.deltaTime * 24f;
            batModel.transform.localPosition = Vector3.Lerp(windupPos, hitPos, t);
            batModel.transform.localRotation = Quaternion.Lerp(windupRot, hitRot, t);

            if (!didDamage && t >= 0.55f)
            {
                didDamage = true;

                RaycastHit hit;
                if (Physics.SphereCast(Camera.main.transform.position, 0.45f, Camera.main.transform.forward, out hit, batRange, ~ignoreLayer))
                {
                    Debug.Log("Bat hit: " + hit.collider.name);

                    IDamage dmg = hit.collider.GetComponent<IDamage>();
                    if (dmg != null)
                    {
                        dmg.takeDamage(batDmg);
                    }
                }
            }

            yield return null;
        }

        t = 0f;

        // return to idle
        while (t < 1f)
        {
            t += Time.deltaTime * 16f;
            batModel.transform.localPosition = Vector3.Lerp(hitPos, startPos, t);
            batModel.transform.localRotation = Quaternion.Lerp(hitRot, startRot, t);
            yield return null;
        }

        batModel.transform.localPosition = startPos;
        batModel.transform.localRotation = startRot;

        isSwinging = false;
    }
    public void getBat()
    {
        hasBat = true;

        if (batModel != null)
        {
            batModel.SetActive(true);

        }
        if (gunModel != null)
        {
            gunModel.SetActive(false);
        }
    }
}
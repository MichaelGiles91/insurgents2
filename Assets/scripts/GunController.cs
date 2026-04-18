using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] gunStats gun;

    float shootTimer;

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer >= gun.shootRate && gun.ammoCur > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        shootTimer = 0;
        gun.ammoCur--;

        Debug.Log("Shot for " + gun.shootDamage);
    }
}
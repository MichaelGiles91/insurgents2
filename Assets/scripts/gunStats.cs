using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    public bool isPowerWeapon;
    public float powerDuration = 10f;
    public AudioClip powerMusic;
    public GameObject gunModel;
    bool isInvincible = false;

    [Range(1, 25)] public int shootDamage;
    [Range(1, 1000)] public int shootDist;
    [Range(0.1f, 2f)] public float shootRate;

    public int ammoCur;
    public int ammoReserve;
    public int ammoReserveMax;
    [Range(5, 50)] public int ammoMax;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    [Range(0, 1)] public float shootSoundVol;

    public AudioClip hitSound;
    [Range(0, 1)] public float hitSoundVol;
}
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

    [Range(1, 10)] public int shootDamage;
    [Range(15, 1000)] public int shootDist;
    [Range(0.1f, 2f)] public float shootRate;

    public int ammoCur;
    [Range(5, 50)] public int ammoMax;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    [Range(0, 1)] public float shootSoundVol;

   
}

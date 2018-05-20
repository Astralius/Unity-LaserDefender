using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    private static ScoreKeeper scoreKeeper;
    private static ObjectPooler objectPooler;
  
    public GameObject Projectile;
    public Transform Weapon;
    public AudioClip[] ExplosionSounds;

    public int Health = 200;
    public int ScoreValue = 10;
    [Range(0f, 0.1f)]
    public float ThreatLevel = 0.003f;
    public float ShootingInterval = 1f;

    private float shootingTimer;

    public EnemyDestroyedEvent OnEnemyDestroyed;

    private void Awake()
    {
        if (!scoreKeeper)
        {
            scoreKeeper = FindObjectOfType<ScoreKeeper>();
        }
        if (!objectPooler)
        {
            objectPooler = FindObjectOfType<ObjectPooler>();
        }

        if (objectPooler)
        {
            objectPooler.AddPool(Projectile, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var projectile = collision.GetComponent<Projectile>();
        if (projectile && OnEnemyDestroyed != null)
        {
            Health -= projectile.Damage;
            projectile.Hit();
            if(Health <= 0)
            {
                SelfDestruct();
            }           
        }
    }

    private void Update()
    {
        if (shootingTimer > ShootingInterval && UnityEngine.Random.value < ThreatLevel)
        {
            Fire();
            shootingTimer = 0;
        }
        shootingTimer += Time.deltaTime;
    }

    private void Fire()
    {
        if (objectPooler)
        {
            objectPooler.SpawnInstance(Projectile, Weapon.position, Weapon.rotation);
        }
        else
        {
            Instantiate(Projectile, Weapon.position, Weapon.rotation);
        }
    }

    private void SelfDestruct()
    {      
        if (scoreKeeper)
        {
            scoreKeeper.Score(ScoreValue);
        }
        AudioSource.PlayClipAtPoint(DrawRandomClip(ExplosionSounds), transform.position);
        OnEnemyDestroyed.Invoke(this);      
        Destroy(this.gameObject);
    }

    private static AudioClip DrawRandomClip(IList<AudioClip> clips)
    {
        return clips[UnityEngine.Random.Range(0, clips.Count - 1)];
    }

    [Serializable]
    public class EnemyDestroyedEvent : UnityEvent<EnemyController> { }
}

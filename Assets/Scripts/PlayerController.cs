using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(min: 0f, max: 10f)]
    public float MovementSpeed = 1.0f;

    [Tooltip("Amount of damage the player's ship can take before destruction.")]
    public float Health = 200f;

    [Tooltip("Location of the first weapon (place for spawning its projectiles).")]
    public Transform MainWeaponSlot;

    [Tooltip("Available weapon types for the main weapon slot.")]
    public GameObject[] Projectiles;

    [Tooltip("The sound player ship makes when being destroyed. ")]
    public AudioClip DeathSound;

    private new Transform transform;
    private float MaxPositionLeft, MaxPositionRight;
    private float MaxPositionTop, MaxPositionBottom;
    private Vector3 movementVector;

    private int selectedWeapon = 0;
    private float weaponDelay = 0.3f;
    private float shootingDelay;
    private ObjectPooler objectPooler;

    private void Awake()
    {
        this.transform = GetComponent<Transform>();
        this.objectPooler = FindObjectOfType<ObjectPooler>();
        if (objectPooler != null)
        {
            objectPooler.AddPool(Projectiles[selectedWeapon], 10);
        }
        ComputePositionLimits();       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var projectile = collision.GetComponent<Projectile>();
        if (projectile)
        {
            Health -= projectile.Damage;
            projectile.Hit();
            if (Health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(DeathSound, transform.position);   
        FindObjectOfType<LevelManager>().LoadLevelDelayed("Win Screen", 2f);
        Destroy(this.gameObject);
    }

    private void ComputePositionLimits()
    {       
        Camera mainCamera = Camera.main;

        var bottomLeft = new Vector3(0f, 0f, 0f);
        var bottomRight = new Vector3(1f, 0f, 0f);
        var topLeft = new Vector3(0f, 1f, 0f);

        float viewportWidth =
            mainCamera.ViewportToWorldPoint(bottomRight).x -
            mainCamera.ViewportToWorldPoint(bottomLeft).x;

        float viewportHeight =
            mainCamera.ViewportToWorldPoint(topLeft).y -
            mainCamera.ViewportToWorldPoint(bottomLeft).y;

        Vector2 spriteExtents = GetComponent<SpriteRenderer>().bounds.extents;

        this.MaxPositionLeft = -viewportWidth / 2 + spriteExtents.x;
        this.MaxPositionRight = viewportWidth / 2 - spriteExtents.x;
        this.MaxPositionBottom = -viewportHeight / 2 + spriteExtents.y;
        this.MaxPositionTop = viewportHeight / 2 - spriteExtents.y;
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
    }
    
    private void HandleMovement()
    {
        movementVector = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            movementVector.x = -MovementSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            movementVector.x = MovementSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            movementVector.y = -MovementSpeed;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            movementVector.y = MovementSpeed;
        }

        movementVector = transform.position + movementVector * Time.deltaTime;
        movementVector.x = Mathf.Clamp(movementVector.x, MaxPositionLeft, MaxPositionRight);
        movementVector.y = Mathf.Clamp(movementVector.y, MaxPositionBottom, MaxPositionTop);

        transform.position = movementVector;
    }

    private void HandleShooting()
    {
        shootingDelay += Time.deltaTime;

        if ((Input.GetKey(KeyCode.Space) && shootingDelay > weaponDelay) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            if(objectPooler != null)
            {
                objectPooler.SpawnInstance(
                    Projectiles[selectedWeapon],
                    MainWeaponSlot.position,
                    MainWeaponSlot.rotation);
            }
            else
            {
                Debug.LogWarning("No object pooler detected! Using standard instantiation..");
                GameObject.Instantiate(
                    Projectiles[selectedWeapon],
                    MainWeaponSlot.position,
                    MainWeaponSlot.rotation);
            }
            shootingDelay = 0;
        }        
    }
}

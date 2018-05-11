using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(min: 0f, max: 10f)]
    public float MovementSpeed = 1.0f;

    private float MaxPositionLeft, MaxPositionRight;
    private float MaxPositionTop, MaxPositionBottom;   
    private Vector3 movementVector;
    private new Transform transform;

    private void Awake()
    {
        this.transform = GetComponent<Transform>();  
        ComputePositionLimits();       
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
}

using UnityEngine;

public class Propulsion : MonoBehaviour
{
    public float Speed = 5f;
    private new Transform transform;
    private new Rigidbody2D rigidbody;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (rigidbody != null)
        {
            rigidbody.velocity =  Vector3.up * Speed;
        }
    }

    private void Update()
    {
        if (rigidbody == null)
        {
            transform.position += Vector3.up * Speed * Time.deltaTime;
        }      
    }
}

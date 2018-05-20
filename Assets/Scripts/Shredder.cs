using UnityEngine;

public class Shredder : MonoBehaviour
{
    private ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (objectPooler)
        {
            collision.gameObject.SetActive(false);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}

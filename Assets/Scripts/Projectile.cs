using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage;

    private ObjectPooler objectPooler;

    public void Awake()
    {
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

    public void Hit()
    {
        if (objectPooler)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float SpawnInterval = 0.1f;
    public float Speed = 5;

    private new Transform transform;
    private Vector3 initialPosition;
    private readonly List<SpriteRenderer> enemies = new List<SpriteRenderer>();
    private SpriteRenderer leftmostEnemy, rightmostEnemy;
    private bool movingRight = true;
    private float leftEdge, rightEdge;

    private void Start()
    {
        this.transform = GetComponent<Transform>();
        this.initialPosition = transform.position;

        var distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        this.leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera)).x;
        this.rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera)).x;

        SpawnEnemies();
    }

    private void Update()
    {
        if (leftmostEnemy == null || rightmostEnemy == null)
        {
            return;
        }

        transform.position += ((movingRight) ? Vector3.right : Vector3.left) * Speed * Time.deltaTime;

        var leftmostEnemyX = leftmostEnemy.transform.position.x - leftmostEnemy.bounds.extents.x;
        var rightmostEnemyX = rightmostEnemy.transform.position.x + rightmostEnemy.bounds.extents.x;

        if ((!movingRight && leftmostEnemyX < leftEdge) ||
            (movingRight && rightmostEnemyX > rightEdge))
        {
            movingRight = !movingRight;
        }
    }

    private void SpawnEnemies()
    {
        Transform nextFreeSlot = FindNextFreeSlot();
        if (nextFreeSlot)
        {
            var enemy = Instantiate(enemyPrefab, nextFreeSlot.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = nextFreeSlot;
            enemy.GetComponent<EnemyController>().OnEnemyDestroyed.AddListener(OnEnemyDestroyed);
            enemies.Add(enemy.GetComponent<SpriteRenderer>());
            Invoke("SpawnEnemies", SpawnInterval);
        }
        else
        {
            FindSides();
        }      
    }

    private void OnEnemyDestroyed(EnemyController enemyController)
    {
        enemies.Remove(enemyController.GetComponent<SpriteRenderer>());
        enemyController.OnEnemyDestroyed.RemoveAllListeners();

        if (enemies.Count > 0)
        {
            FindSides();
        }
        else
        {
            transform.position = initialPosition;
            SpawnEnemies();
        }
    }

    private void FindSides()
    {
        var leftmost = Vector3.right * rightEdge;
        var rightmost = Vector3.right * leftEdge;

        foreach(SpriteRenderer renderer in enemies)
        {
            var enemyPosition = renderer.transform.position;
            if (enemyPosition.x < leftmost.x)
            {
                leftmost = enemyPosition;
                leftmostEnemy = renderer;
            }
            if (enemyPosition.x > rightmost.x)
            {
                rightmost = enemyPosition;
                rightmostEnemy = renderer;
            }
        }
    }

    private Transform FindNextFreeSlot()
    {
        foreach (Transform enemyPosition in transform)
        {
            if (enemyPosition.childCount == 0)
            {
                return enemyPosition;
            }
        }
        return null;
    }
}

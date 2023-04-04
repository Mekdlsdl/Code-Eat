using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBehaviour2 : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    private BoxCollider2D boundary;
    private Rigidbody2D rb;

    private float minBoundX, minBoundY, maxBoundX, maxBoundY;
    private Vector2 wayPoint;
    private

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall") {
            SetNewDestination();
        }
    }

    void Start()
    {
        boundary = transform.parent.GetComponent<BoxCollider2D>();
        rb = transform.GetComponent<Rigidbody2D>();

        minBoundX = boundary.bounds.min.x;
        minBoundY = boundary.bounds.min.y;
        
        maxBoundX = boundary.bounds.max.x;
        maxBoundY = boundary.bounds.max.y;

        SetNewDestination();
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, wayPoint) < range)
        {
            SetNewDestination();
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);
        }
    }

    void SetNewDestination()
    {
        wayPoint = new Vector2(Random.Range(minBoundX, maxBoundX), Random.Range(minBoundY, maxBoundY));
    }
}

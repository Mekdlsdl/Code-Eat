using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float directionChangeInterval = 0.5f;
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private LayerMask obstacleLayer;

    private Transform detectedPlayer;
    private Rigidbody2D rb;
    private Vector2 wayPoint;
    private Vector3 moveDir, lastDir;

    private float directionTimer = 0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (GameManager.isProblemMode) {
            rb.velocity = Vector2.zero;
            return;
        }
        
        if (DetectPlayer())
        {
            // 플레이어로부터 멀어진다
            moveDir = transform.position - detectedPlayer.position;
            lastDir = moveDir;
        }
        else
        {   
            // 랜덤으로 움직인다
            if (Time.time >= directionTimer)
            {
                if (lastDir != Vector3.zero) {
                    moveDir = lastDir;
                    lastDir = Vector3.zero;
                }
                else
                    moveDir = Random.insideUnitCircle;
                directionTimer = Time.time + directionChangeInterval;
            }
        }
        DetectObstacle();
        transform.Translate(moveDir.normalized * speed * Time.deltaTime);
    }

    private bool DetectPlayer()
    {
        foreach (Transform player in PlayerSpawn.instance.PlayerTransforms) {
            if (Vector2.Distance(transform.position, player.position) < detectionRadius) {
                detectedPlayer = player;
                return true;
            }
        }
        return false;
    }

    private void DetectObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 1.5f, obstacleLayer);

        if (hit.collider != null) {
            Vector2 newDir = Vector2.Perpendicular(hit.normal);
            moveDir = Vector2.Lerp(moveDir, newDir, 0.5f);
            directionTimer = Time.time + directionChangeInterval;
        }
    }
}

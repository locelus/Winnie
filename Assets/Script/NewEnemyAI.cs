using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class NewEnemyAI : MonoBehaviour {

    public bool seen;
    RaycastHit2D hit;
    Vector3 rayDirection;
    float rayAngle;
    float rayLength;
    public float maxViewAngle = 20;
    public float maxViewDistance = 100;
    public float maxDamagingDistance = 30;
    public LayerMask mask;
    public GameObject sightPoint;
    public GameObject statusIndicator;

    Vector3 target;

    float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;
    private EnemyWeapon weapon;

    public Path path;

    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    private Vector3 lastSeenPosition;

    public bool searching = false;

    public GameObject head;
    float headAngle;

    public Vector3 playerPos;

    public enum Direction { Left, Right }
    public Direction directionFacing;

    public float jumpForce = 10f;
    public float maxMovementSpeed = 10f;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<EnemyWeapon>();

        target = GameMaster.playerObject.transform.position;
        seeker.StartPath(transform.position, target, OnPathComplete);

        directionFacing = GetDirection(head.transform.rotation.eulerAngles.z);

        StartCoroutine(UpdatePath());
    }

    Direction GetDirection(float headAngle)
    {
        if (90 > headAngle || headAngle > 270)
        {
            if (directionFacing != Direction.Left)
            {
                Flip();
            }
            return Direction.Left;
        }
        if (directionFacing != Direction.Right)
        {
            Flip();
        }
        return Direction.Right;
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            yield return false;
        }
        if (seen)
        {
            target = GameMaster.playerObject.transform.position;
        }
        seeker.StartPath(transform.position, target, OnPathComplete);
        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    void OnPathComplete(Path p)
    {
        //Debug.Log("We got a path Did it have an error" + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            Debug.Log("end of path reached");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        Vector2 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        if (seen) {
            dir *= speed * Time.fixedDeltaTime;
        }

        if (searching && (rayLength > maxDamagingDistance || !seen))
        {
            if (directionFacing == Direction.Left)
            {
                rb.velocity = new Vector2(-maxMovementSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(maxMovementSpeed, rb.velocity.y);
            }
        }
        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }

    }

    public void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        theScale = head.transform.localScale;
        theScale.y *= -1;
        head.transform.localScale = theScale;

        theScale = sightPoint.transform.localPosition;
        theScale.x *= -1;
        sightPoint.transform.localPosition = theScale;

        theScale = statusIndicator.transform.localScale;
        theScale.x *= -1;
        statusIndicator.transform.localScale = theScale;
    }

    public void Update()
    {
        headAngle = head.transform.rotation.eulerAngles.z;
        playerPos = GameMaster.playerObject.transform.position;
        Vector3 sightPointPos = sightPoint.transform.position;
        rayDirection = playerPos - sightPointPos;
        rayAngle = Vector3.Angle(rayDirection, -head.transform.right);
        directionFacing = GetDirection(headAngle);
        rayLength = rayDirection.magnitude;
        hit = Physics2D.Raycast(sightPointPos, rayDirection, mask);

        if (rayLength < maxViewDistance && Math.Abs(rayAngle) < maxViewAngle && hit.collider.gameObject.CompareTag("Player"))
        {
            seen = true;
            searching = false;
            Debug.DrawRay(sightPointPos, rayDirection);
            weapon.Shoot();
        }
        else
        {
            if (seen)
            {
                lastSeenPosition = GameMaster.playerObject.transform.position;
                target = lastSeenPosition;
                searching = true;
            }
            seen = false;
        }
        if (seen)
        {
            searching = false;
            Debug.DrawRay(sightPointPos, rayDirection);
            weapon.Shoot();
        }
    }

    void OnTriggerEnter2D (Collider2D col)
    {
        //Jump
        Debug.Log("Jump");
        rb.AddForce(new Vector2(0f, jumpForce));
    }

}

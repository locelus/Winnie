using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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


    public Vector3 target;

    public float updateRate = 2f;

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

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<EnemyWeapon>();

        target = GameMaster.playerObject.transform.position;
        seeker.StartPath(transform.position, target, OnPathComplete);

        StartCoroutine(UpdatePath());
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

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime * 20;

        if (searching && rayLength > maxDamagingDistance)
        {
            rb.AddForce(dir, fMode);
        }
        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }

    }
    public void Update()
    {
        float headAngle = head.transform.rotation.eulerAngles.z;
        Vector3 playerPos = GameMaster.playerObject.transform.position;
        Vector3 sightPointPos = sightPoint.transform.position;
        rayDirection = playerPos - sightPointPos;
        rayAngle = Vector3.Angle(rayDirection, -transform.right) - headAngle;
        rayLength = rayDirection.magnitude;
        hit = Physics2D.Raycast(sightPointPos, rayDirection, mask);
        //head.transform.eulerAngles = new Vector3(0, 0, -headAngle);
        //head.transform.LookAt(GameMaster.playerObject.transform.position, head.transform.up);
        //head.transform.Rotate(new Vector3(0, 90, 0), Space.Self);//correcting the original rotation 
        Vector3 diff = playerPos - head.transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        head.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 180);
        Debug.Log(head.transform.rotation);
        if (rayLength < maxViewDistance && rayAngle < maxViewAngle)
        {
            seen = true;
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
            Debug.DrawRay(sightPoint.transform.position, rayDirection);
            weapon.Shoot();
        }
        return;
        if (hit.transform.gameObject.name == "Player")
        {   //If the object hit by the raycast is the player...
            seen = true;
        }       //If player is visible, then shoot
        else
        {
            Debug.Log("Not seen");
            seen = false;
        }
        if (seen)
        {
            Debug.Log("Seen");
            weapon.Shoot();
        }
        Debug.DrawRay(sightPoint.transform.position, rayDirection);
    }

}

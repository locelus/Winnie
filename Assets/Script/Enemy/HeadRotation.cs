using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotation : MonoBehaviour
{

    public int rotationOffset = 90;
    public NewEnemyAI parentEnemy;

    public float rotZ { get; }

    void Start()
    {
        parentEnemy = GetComponentInParent<NewEnemyAI>();
        if (parentEnemy == null)
        {
            Debug.LogError("No parent found for enemy head");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }// subtracting the position of the player from the mouse position
        Vector3 difference = GameMaster.playerObject.transform.position - transform.position;
        difference.Normalize();     // normalizing the vector. Meaning that all the sum of the vector will be equal to 1

        float _rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;   // find the angle in degrees

        transform.rotation = Quaternion.Euler(0f, 0f, _rotZ + rotationOffset);
    }
}
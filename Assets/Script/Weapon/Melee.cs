using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override int Shoot() {
        base.Shoot();
        Debug.Log("Print");
        return 0;
    }
}

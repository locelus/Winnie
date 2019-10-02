using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class LaunchArcRenderer : MonoBehaviour {

    LineRenderer lr;

    public float velocity;
    public float angle;
    public int resolution = 10;

    float g; //Force of gravity on the y axis
    float radianAngle; //Theta

    void Awake() {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y);
    }

    void OnValidate() {
        //check that lr != null and that the game is playing
        if (lr != null && Application.isPlaying) {
            RenderArc();
        }
    }


    // Use this for initialization
    void Start () {
        RenderArc();
	}

    public void ChangeValues(float inputAngle, float inputVelocity) {
        angle = inputAngle;
        velocity = inputVelocity;
        RenderArc();
    }

    //Populates the line renderer with the appropriate settings
    void RenderArc() {
        lr.positionCount = resolution + 1;
        lr.SetPositions(CalculateArcArray());
    }

    //Create array of Vector3 positions for arc
    Vector3[] CalculateArcArray() {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = ((velocity * velocity) * Mathf.Sin(2*radianAngle)) / g;

        for (int i = 0; i <= resolution; i++) {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
    }

    //Calculate height and distance of each vertex

    Vector3 CalculateArcPoint(float t, float maxDistance) {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((g * x * x)/(2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x,y);
    }
    
}

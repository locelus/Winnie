using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds;             //List of all foregrounds and backgrounds to be parallaxed
    private float[] parallaxScales;             //Proportion of Camera's movement to move all of the backgrounds by
    public float smoothing = 1f;                //How smooth the parallax will be, set this above 0


    private float parallax;                      //Declares the parallax float
    private Transform cam;                      //Reference to main camera's transform
    private Vector3 previousCamPos;             //The position of the camera in the previous frame
    private float backgroundTargetPosX;                 //X-Target for the background to go to in the Update() function
    Vector3 backgroundTargetPos;                //Target for the background to go in the Update() function


    //Called before Start()
    void Awake() {
        cam = Camera.main.transform;            //Initializes main camera transform
    }

    // Use this for initialization
    void Start () {
        //Previous frame had current frame's camera position
        previousCamPos = cam.position;              
        //Makes the length of parallaxScales[] the amount of backgrounds
        parallaxScales = new float[backgrounds.Length];     
        
        for(int i = 0; i < backgrounds.Length; i++) {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update () {

        //for loop makes it so that our process is called for each background
        for (int i = 0; i < backgrounds.Length; i++) {
            //Multiplies the distance the camera has moved in one frame and moves the background according to how much it should be moved based on parallaxScales[]
            parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            // set a target x position which is the current position plus the parallax
            backgroundTargetPosX = backgrounds[i].position.x + parallax;
            
            // create a target position which is the background's current position with it's target x position
            backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            
            // fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
            }

        // set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;
    }
}

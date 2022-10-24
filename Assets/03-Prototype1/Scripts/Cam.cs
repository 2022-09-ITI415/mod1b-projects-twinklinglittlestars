using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    static public GameObject POI; // the static point of interest

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ; // the desired Z pos of the camera

    void Awake()
    {
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        // if there's only one line following an if, it doesn't need braces
        //if (POI == null) return; // return if there is no poi

        //Get the position of the poi
        //Vector3 destination = POI.transform.position;
        Vector3 destination;
        // if there is no poi, return to P:[0,0,0]
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //Get the position of the poi
            destination = POI.transform.position;
            // if poi is a projectile, check to see if it's at rest
            if (POI.tag == "KirbyProjectile") // changed projectile to kirbyedited
            {
                // if it is sleeping (that is, not moving)
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    //return to default view
                    POI = null;
                    //in the next update
                    return;
                }
            }
        }
        //Limit the X & Y to minimum values
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        // Interpolate from the current Camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        //Force destination.z to be camZ to keep the camera far enough away
        destination.z = camZ;
        //Set the camera to the destination
        transform.position = destination;
        //Set the orthographicSize of the Camera to keep Ground in view
        Camera.main.orthographicSize = destination.y + 10;
    }
}



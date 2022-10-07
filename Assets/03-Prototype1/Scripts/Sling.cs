using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sling : MonoBehaviour
{
    static private Sling S;
    // fields set in the Unity Inspector pane
    [Header("Set in Inspector")]

    public GameObject prefabKirbyEdited;
    public float velocityMult = 8f;

    // fields set dynamically
    [Header("Set Dynamically")]

    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject KirbyEdited;
    public bool aimingMode;

    private Rigidbody kirbyEditedRigidbody;
    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }
    void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

    void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        // The player has pressed the mouse button while over Slingshot
        aimingMode = true;
        //Instantiate a Projectile
        KirbyEdited = Instantiate(prefabKirbyEdited) as GameObject;
        //Start it at the launchPoint
        KirbyEdited.GetComponent<Rigidbody>().isKinematic = true;
        // Set it to isKinematic for now
        kirbyEditedRigidbody = KirbyEdited.GetComponent<Rigidbody>();
        kirbyEditedRigidbody.isKinematic = true;
    }

    void Update()
    {
        // If slingshot is not in aimingMode, don't run this code
        if (!aimingMode) return;
        //Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //Limit mouseDelta to the radius of the Slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //Move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        KirbyEdited.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            // the mouse has been released
            aimingMode = false;
            kirbyEditedRigidbody.isKinematic = false;
            kirbyEditedRigidbody.velocity = -mouseDelta * velocityMult;
            Cam.POI = KirbyEdited;
            KirbyEdited = null;
            KMissionDemolition.ShotsFired();
            KProjectileLine.S.poi = KirbyEdited;
        }
    }
}



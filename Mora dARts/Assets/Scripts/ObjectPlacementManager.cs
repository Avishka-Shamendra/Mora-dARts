using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectPlacementManager : MonoBehaviour
{

    public GameObject dartBoard;  // To store the dartboard which will be placed in the posiotion of placement indicator
    public GameObject placementIndicator; // To store the placement indicator we created
    private Pose placementPose; // To store the placement,rotation of the postition selected for placing object
    private Transform placementTransform; // To store and manipulate the position, rotation and scale of the placement plane
    private bool placementPoseIsValid = false; // To store if the placement position is valid
    private bool isDartboardPlaced = false; // To store if the dartboard placed or not
    private TrackableId selectedPlaneId = TrackableId.invalidId; // To store unique identifier for the selected plane
    ARRaycastManager raycastManager; // To hold the raycast manager.Ray casting allows us to determine where a ray (defined by an origin and direction) intersects with a trackable
    static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();


    // Start is called before the first frame update
    void Start(){}

    // Called when script instance is being loaded. Called once in lifetime of script instance
    void Awake(){
        if(TryGetComponent(out ARRaycastManager raycast)) // get ARRaycastManager Component
            raycastManager = raycast;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDartboardPlaced)
        {
            // Only excutes if the dartboard is not placed => which ensuers only one dartboard can be placed

            UpdatePlacementPosistion();
            UpdatePlacementIndicator();

            // Check if the position of placement inidcator is valid and if the user has touched the screen
            if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceDartboard();
            }
        }
    }

    // Find the correct position to place the placement indicator
    private void UpdatePlacementPosistion()
    {
        var center = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)); // get the screen center
        if (raycastManager.Raycast(center, raycastHits, TrackableType.PlaneWithinPolygon))
        {
            placementPoseIsValid = raycastHits.Count > 0; // placement position is valid if there are zero or more ray cast hits
            if (placementPoseIsValid)
            {
                placementPose = raycastHits[0].pose; // get the position object
                selectedPlaneId = raycastHits[0].trackableId; // get plane id

                var planeManager = GetComponent<ARPlaneManager>(); // get the plane manager
                ARPlane arPlane = planeManager.GetPlane(selectedPlaneId); // get the plane with the id
                placementTransform = arPlane.transform; // store position,scale,rotation of plane
            }
        }
    }

    // Place placement indicator in the correct position
    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid) // if placement position is valid
        {
            placementIndicator.SetActive(true); // activate the placement indicator
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementTransform.rotation);
        }
        else
        {
            placementIndicator.SetActive(false); // deactivate the placement indicator
        }
    }

    // Place the dartboard on the postion of the placement indicator
    private void PlaceDartboard()
    {
        Instantiate(dartBoard, placementPose.position, placementTransform.rotation); // inititate the dartboard game object
        isDartboardPlaced = true; 
        placementIndicator.SetActive(false); // deactivate the placement indicator since it is no longer needed
    }
}

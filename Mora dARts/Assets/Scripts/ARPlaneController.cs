using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class ARPlaneController : MonoBehaviour
{
    ARPlaneManager arPlaneManager;
    // Start is called before the first frame update
    void Start(){}

    // Called when script instance is being loaded. Called once in lifetime of script instance
    void Awake(){
        if(TryGetComponent(out ARPlaneManager planeManager)) // get ARPlaneManager Component
            arPlaneManager = planeManager;
    }

    // Called when script is attached to a scene
    void OnEnable(){
        ObjectPlacementManager.onDartBoardPlacement += DisableARPlaneDetection; // subscribe to on board placement event in object placement manager to trigger Disable Plane Detection method
    }

    // Called when object is destroyed. To prevent memory leakage unsubsrcibe
    void OnDisable(){
        ObjectPlacementManager.onDartBoardPlacement -= DisableARPlaneDetection; // unsubscribe from event
    }


    // Update is called once per frame
    void Update(){}

    // Disable Plane Detection and Hide Existing
    void DisableARPlaneDetection(){
        
        foreach (var plane in arPlaneManager.trackables) // Loop over all the existing planes and deactivates them.
            plane.gameObject.SetActive(false); // disable all planes
            
        arPlaneManager.enabled = !arPlaneManager.enabled; // toggle plane manager
    }
    
}

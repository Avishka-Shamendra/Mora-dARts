using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DartController : MonoBehaviour
{
    public GameObject DartPrefab; // To store the dart prefab (used to instantiate a Dart object)
    public Transform DartIntialPosition;  // To keep the initial cordinates of the dart
    ARSessionOrigin ARSessionOrigin;  // To store the AR Session Origin object
    GameObject ARCam;  // To store the AR Camera object
    private GameObject DartTemp;  // To store the instantiated the Dart object (placed on the screen)
    private Rigidbody DartRigidBody;  // To store the Rigid Body component of the Dart object

    void Start()
    {
        ARSessionOrigin = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();  // Get the AR Session Origin object from the game scene
        ARCam = ARSessionOrigin.transform.Find("AR Camera").gameObject;  // Get the AR Camera object from the game scene
    }

    void OnEnable()
    {
        ObjectPlacementManager.onDartBoardPlacement += InitializeDart;  // Subscribe to the dartboard placement event
    }

    void OnDisable()
    {
        ObjectPlacementManager.onDartBoardPlacement -= InitializeDart; // Unsubscribe from the dartboard placement event
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)  // Check if there's a touch input by the user
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);  // Get a ray that is going through the touch point of the user
            RaycastHit raycastHit;  // Used to get data back from a ray hit
            if (Physics.Raycast(raycast, out raycastHit))  // Check if the ray hits a collider
            {
                if (raycastHit.collider.CompareTag("dart"))  // Check if the collider is the dart collider
                {
                    // Disable back touch Collider from dart
                    raycastHit.collider.enabled = false;

                    DartTemp.transform.parent = ARSessionOrigin.transform;  // Make DartTemp, a child of AR Session Origin

                    Dart currentDartScript = DartTemp.GetComponent<Dart>();  // Get the current dart script enabled by the placed dart
                    currentDartScript.isForceApplied = true;  // Make the applied force on the dart true

                    // Load a new dart
                    InitializeDart();
                }
            }
        }
    }

    // Method to initialize a dart instance on screen
    void InitializeDart()
    {
        StartCoroutine(WaitAndSpawnDart());
    }

    // Coroutine to initialize a dart intance on screen
    public IEnumerator WaitAndSpawnDart()
    {
        yield return new WaitForSeconds(0.1f);  // Wait 0.1 seconds
        DartTemp = Instantiate(DartPrefab, DartIntialPosition.position, ARCam.transform.localRotation);  // Instantiate Dart object
        DartTemp.transform.parent = ARCam.transform;  // Make DartTemp, a child of AR Session Camera
        DartRigidBody = DartTemp.GetComponent<Rigidbody>();  // Assign the Rigid Body of the intiated dart object to DartRigidBody
        DartRigidBody.isKinematic = true;  // Stop all the physics on the dart (dart will stay infront of the camera)
    }
}

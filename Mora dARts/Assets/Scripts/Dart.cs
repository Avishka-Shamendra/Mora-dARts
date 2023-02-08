using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class Dart : MonoBehaviour
{
    private Rigidbody DartRigidBody;  // To store the Rigid Body component of the Dart object
    private GameObject DartInitialPosition;  // To keep the initial cordinates of the dart
    public bool isForceApplied = false;  // Whether a force is applied to the dart by the user
    bool isDartRotating = false;  // Whether the dart is rotating
    bool isDartReadyToShoot = true;  // Whether the dart is ready to be shot
    bool dartHit = false;  // Whether the dart hits on the dartboard

    ARSessionOrigin ARSessionOrigin;   // To store the AR Session Origin object
    GameObject ARCam;  // To store the AR Camera object
    public TMP_Text pointValue = null; // To store point value

    private bool isUpdatingPointValueText = false; // To prevent point value being updated mutiple times by a single dart

    public Collider dartFrontCollider;


    private static Dictionary<string, int> pointMap = new Dictionary<string, int>
    {
        {"HitArea.001", 0}, {"HitArea.002", 0}, {"HitArea.003", 0}, {"HitArea.004", 0}, {"HitArea.005", 0}, {"HitArea.006", 0}, {"HitArea.007", 0}, {"HitArea.008", 0},
        {"HitArea.009", 0}, {"HitArea.010", 0}, {"HitArea.011", 0}, {"HitArea.012", 0}, {"HitArea.013", 0}, {"HitArea.014", 0}, {"HitArea.015", 0}, {"HitArea.016", 0},
        {"HitArea.017", 0}, {"HitArea.018", 0}, {"HitArea.019", 0}, {"HitArea.020", 0}, {"HitArea.021", 0}, {"HitArea.022", 0}, {"HitArea.023", 0}, {"HitArea.024", 0},
        {"HitArea.025", 0}, {"HitArea.026", 0}, {"HitArea.027", 0}, {"HitArea.028", 0}, {"HitArea.029", 0}, {"HitArea.030", 0}, {"HitArea.031", 0}, {"HitArea.032", 0},
        {"HitArea.033", 0}, {"HitArea.034", 0}, {"HitArea.035", 0}, {"HitArea.036", 0}, {"HitArea.037", 0}, {"HitArea.038", 0}, {"HitArea.039", 0}, {"HitArea.040", 0},
        {"HitArea.041", 0}, {"HitArea.042", 0}, {"HitArea.043", 0}, {"HitArea.044", 0}, {"HitArea.045", 0}, {"HitArea.046", 0}, {"HitArea.047", 0}, {"HitArea.048", 0},
        {"HitArea.049", 0}, {"HitArea.050", 0}, {"HitArea.051", 0}, {"HitArea.052", 0}, {"HitArea.053", 0}, {"HitArea.054", 0}, {"HitArea.055", 0}, {"HitArea.056", 0},
        {"HitArea.057", 0}, {"HitArea.058", 0}, {"HitArea.059", 0}, {"HitArea.060", 0}, {"HitArea.061", 0}, {"HitArea.062", 0}, {"HitArea.063", 0}, {"HitArea.064", 0},
        {"HitArea.065", 0}, {"HitArea.066", 0}, {"HitArea.067", 0}, {"HitArea.068", 0}, {"HitArea.069", 0}, {"HitArea.070", 0}, {"HitArea.071", 0}, {"HitArea.072", 0},
        {"HitArea.073", 0}, {"HitArea.074", 0}, {"HitArea.075", 0}, {"HitArea.076", 0}, {"HitArea.077", 0}, {"HitArea.078", 0}, {"HitArea.079", 0}, {"Ring.007",0},
        {"Ring.008",0}
    };

    // Start is called before the first frame update
    void Start()
    {
        ARSessionOrigin = GameObject.FindGameObjectWithTag("AR Session Origin").GetComponent<ARSessionOrigin>(); // Get the AR Session Origin object from the game scene
        ARCam = ARSessionOrigin.transform.Find("AR Camera").gameObject;  // Get the AR Camera object from the game scene

        if (TryGetComponent(out Rigidbody rigid))
            DartRigidBody = rigid;  // Assign the Rigid Body of the intiated dart object to DartRigidBody
        DartInitialPosition = GameObject.FindGameObjectWithTag("DartPosition");  // Initialize dart intial position
    }

    private void FixedUpdate()
    {
        if (isForceApplied)  // Checks if force is applied on the dart
        {
            dartFrontCollider.enabled = true;  // Enable the front collider of the dart
            StartCoroutine(InitDartDestroyVFX());  // Activate the coroutine to destroy the dart
            GetComponent<Rigidbody>().isKinematic = false;  // Enable physics on the dart
            isForceApplied = false;  // Make the applied force false
            isDartRotating = true;  // Make the dart rotating
        }

        // Add force to the dart in the forward direction
        DartRigidBody.AddForce(DartInitialPosition.transform.forward * (12f + 6f) * Time.deltaTime, ForceMode.VelocityChange);

        // If dart is ready to shoot
        if (isDartReadyToShoot)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * 20f);  // Add a rotation to the dart
        }

        // Dart rotating
        if (isDartRotating)
        {
            isDartReadyToShoot = false;  // It's already on the way to the dartboard
            transform.Rotate(Vector3.forward * Time.deltaTime * 400f);  // Continue rotating the dart while its on the air
        }
    }

    // Coroutine to destroy the dart object
    IEnumerator InitDartDestroyVFX()
    {
        yield return new WaitForSeconds(5f);  // Wait for 5 seconds
        if (!dartHit)  // Check if the dart doesn't hit the board within 5 seconds
        {
            Destroy(gameObject);  // Destroy the object
        }
    }

    

    private IEnumerator UpdatePointValue(string colliderName)
    {
        // Check if the point value is already being updated
        if (!isUpdatingPointValueText) // to avoid the point value being updated multiple times
        {
            isUpdatingPointValueText = true; // Set the flag to indicate that the point value is currently being updated
            if (pointMap.ContainsKey(colliderName)){ // check if collider is valid
                int remaining = int.Parse(pointValue.text) - pointMap[colliderName]; // get new point value
                if (remaining > 0){ // if points are not zero yet
                    pointValue.text = remaining.ToString(); // update score
                    yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds to allow the point value to be updated smoothly
                }
                // TODO: remiander = 0 game won
               
            }
            isUpdatingPointValueText = false; // Reset the flag to indicate that the point value has finished updating
        }
    }

    // When dart hits a collider, the follwing function is triggered
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dart_board") && !dartHit)  // Check if the dart collided with the dartboard
        {
            // Trigger vibration
            Handheld.Vibrate();

            GetComponent<Rigidbody>().isKinematic = true;  // Disbale physics on the dart
            isDartRotating = false;  // Stop rotating the dart
            StartCoroutine(UpdatePointValue(other.name)); // Co-routine started to update the score
            // Dart hits the board
            dartHit = true;
        }
    }
}

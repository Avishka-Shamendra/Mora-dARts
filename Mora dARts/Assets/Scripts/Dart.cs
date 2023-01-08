using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

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

    public Collider dartFrontCollider;

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

    // When dart hits a collider, the follwing function is triggered
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dart_board"))  // Check if the dart collided with the dartboard
        {
            // Trigger vibration
            Handheld.Vibrate();

            GetComponent<Rigidbody>().isKinematic = true;  // Disbale physics on the dart
            isDartRotating = false;  // Stop rotating the dart

            // Dart hits the board
            dartHit = true;
        }
    }
}

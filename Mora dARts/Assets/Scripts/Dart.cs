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
        {"HitArea.001", 1}, {"HitArea.002", 18}, {"HitArea.003", 4}, {"HitArea.004", 13}, {"HitArea.005", 6}, {"HitArea.006", 10}, {"HitArea.007", 15}, {"HitArea.008", 2},
        {"HitArea.009", 3}, {"HitArea.010", 19}, {"HitArea.011", 7}, {"HitArea.012", 16}, {"HitArea.013", 8}, {"HitArea.014", 11}, {"HitArea.015", 14}, {"HitArea.016", 9},
        {"HitArea.017", 12}, {"HitArea.018", 5}, {"HitArea.019", 20}, {"HitArea.020", 1}, {"HitArea.021", 18}, {"HitArea.022", 4}, {"HitArea.023", 13}, {"HitArea.024", 6},
        {"HitArea.025", 10}, {"HitArea.026", 15}, {"HitArea.027", 2}, {"HitArea.028", 17}, {"HitArea.029", 3}, {"HitArea.030", 19}, {"HitArea.031", 7}, {"HitArea.032", 16},
        {"HitArea.033", 8}, {"HitArea.034", 11}, {"HitArea.035", 14}, {"HitArea.036", 9}, {"HitArea.037", 12}, {"HitArea.038", 5}, {"HitArea.039", 3*20}, {"HitArea.040", 3*1},
        {"HitArea.041", 3*18}, {"HitArea.042", 3*4}, {"HitArea.043", 3*13}, {"HitArea.044", 3*6}, {"HitArea.045", 3*10}, {"HitArea.046", 3*15}, {"HitArea.047", 3*2}, {"HitArea.048", 3*17},
        {"HitArea.049", 3*3}, {"HitArea.050", 3*19}, {"HitArea.051", 3*7}, {"HitArea.052", 3*16}, {"HitArea.053", 3*8}, {"HitArea.054", 3*11}, {"HitArea.055", 3*14}, {"HitArea.056", 3*9},
        {"HitArea.057", 3*12}, {"HitArea.058", 3*5}, {"HitArea.059", 2*20}, {"HitArea.060", 2*1}, {"HitArea.061", 2*18}, {"HitArea.062", 2*4}, {"HitArea.063", 2*13}, {"HitArea.064", 2*6},
        {"HitArea.065", 2*10}, {"HitArea.066", 2*15}, {"HitArea.067", 2*2}, {"HitArea.068", 2*17}, {"HitArea.069", 2*3}, {"HitArea.070", 2*19}, {"HitArea.071", 2*7}, {"HitArea.072", 2*16},
        {"HitArea.073", 2*8}, {"HitArea.074", 2*11}, {"HitArea.075", 2*14}, {"HitArea.076", 2*9}, {"HitArea.077", 2*12}, {"HitArea.078", 2*5}, {"HitArea.079", 17}, {"HitArea.080", 20}, {"Ring.007",25},
        {"Ring.008",50}
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

    
    // Update point value
    private IEnumerator UpdatePointValue(string colliderName)
    {
        // Check if the point value is already being updated
        if (!isUpdatingPointValueText) // to avoid the point value being updated multiple times
        {
            isUpdatingPointValueText = true; // Set the flag to indicate that the point value is currently being updated
            if (pointMap.ContainsKey(colliderName)){ // check if collider is valid
                int pointMapVal =  pointMap[colliderName];
                int remaining = int.Parse(pointValue.text) - pointMapVal; // get new point value
                if (remaining > 0){ // if points are not zero yet
                    pointValue.text = remaining.ToString(); // update score
                    yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds to allow the point value to be updated smoothly
                    StartCoroutine(ShowDartPointText(0.8f, pointMapVal)); //Show the score for the points score in that round
                }else if (remaining<0){ 
                    StartCoroutine(ShowTooMuchText(0.8f));
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

    // Method will display the points scored when a dart hits the board
    IEnumerator ShowDartPointText(float duration,int points)
    {
        GameObject canvas = GameObject.Find("Canvas");

        TMP_Text dartPointText = GameObject.Instantiate(pointValue, canvas.transform); // get a copy of TMP_Text to display the score
        dartPointText.fontSize = 180;
        dartPointText.color = Color.magenta;
        dartPointText.text = points.ToString(); // show the points
        dartPointText.alignment = TextAlignmentOptions.Center;
        dartPointText.rectTransform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(duration); // display text for given time

        GameObject.Destroy(dartPointText.gameObject);
    }

    // Method will display "Too Much" if player score more than required points in a throw. Ex: need 23, scores 35
    IEnumerator ShowTooMuchText(float duration)
    {
        GameObject canvas = GameObject.Find("Canvas");

        TMP_Text tooMuchText = GameObject.Instantiate(pointValue, canvas.transform); // get a copy of TMP_Text to display the score
        tooMuchText.fontSize = 64;
        tooMuchText.color = Color.magenta;
        tooMuchText.text = "Too Much"; // show the points
        tooMuchText.alignment = TextAlignmentOptions.Center;
        tooMuchText.rectTransform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(duration); // display text for given time

        GameObject.Destroy(tooMuchText.gameObject);
    }
}

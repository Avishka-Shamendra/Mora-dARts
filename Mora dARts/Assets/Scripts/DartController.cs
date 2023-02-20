using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public LevelManager levelManager;  // To store the level manager object

    public TMP_Text PointValue; // To store point value
    public TMP_Text ScoreValue; // To store score value
    
    public TMP_Text DistanceValue; // To store distances to dartbaord;
    private bool isDartBoardSearched = false; // needed for distance calculation
    Transform DartboardObj; // To locate dartboard position, needed for distance calc
    private float distanceFromDartBoard = 0f;


    void Start()
    {
        ARSessionOrigin = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();  // Get the AR Session Origin object from the game scene
        ARCam = ARSessionOrigin.transform.Find("AR Camera").gameObject;  // Get the AR Camera object from the game scene
        DontDestroyOnLoad(gameObject);  // Should not destroy the Dart Contorller on load of the next scene since it is used to retrieve score and points in the GmaeOver Scene
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
        if (Input.touchCount ==1 && Input.GetTouch(0).phase == TouchPhase.Began)  // Check if there's a touch input by the user
        {
            float distance= float.Parse(DistanceValue.text.Substring(0, 3));
            if(distance < 0.8 && isDartBoardSearched) { // if player is too close to throw
                StartCoroutine(ShowTooCloseText(0.8f));
            } else {
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
                        currentDartScript.PointValue = PointValue; // Pass point value holder to dart script to update points once dart collide with board
                        currentDartScript.levelManager = levelManager; // pass level mnanager to dart script
                        int score = int.Parse(ScoreValue.text)-1; // update the score value
                        ScoreValue.text = score.ToString(); //update score value text
                       
                        //TODO: end game if score==0 @dhaura
                        if (score == 0)
                        {
                            StartCoroutine(WaitAndEndGame());
                        }
                        else
                        {
                            // Load a new dart
                            InitializeDart();
                        }
                    }
                } 
            }
            
        }
        if (isDartBoardSearched)
        {
            distanceFromDartBoard = Vector3.Distance(DartboardObj.position, ARCam.transform.position);
            DistanceValue.text = string.Concat(distanceFromDartBoard.ToString().Substring(0, 3)," m");
        }
    }

    // Method to initialize a dart instance on screen
    void InitializeDart()
    {
        DartboardObj = GameObject.FindWithTag("dart_board").transform; // find dart baord from tag
        if (DartboardObj)
        {
            isDartBoardSearched = true;
        }
        StartCoroutine(WaitAndSpawnDart());
    }

    // Coroutine to initialize a dart intance on screen
    public IEnumerator WaitAndSpawnDart()
    {
        yield return new WaitForSeconds(1.0f);  // Wait 1 second
        DartTemp = Instantiate(DartPrefab, DartIntialPosition.position, ARCam.transform.localRotation);  // Instantiate Dart object
        DartTemp.transform.parent = ARCam.transform;  // Make DartTemp, a child of AR Session Camera
        DartRigidBody = DartTemp.GetComponent<Rigidbody>();  // Assign the Rigid Body of the intiated dart object to DartRigidBody
        DartRigidBody.isKinematic = true;  // Stop all the physics on the dart (dart will stay infront of the camera)
    }


    // Method will display "Too close" if player is too close to throw
    IEnumerator ShowTooCloseText(float duration)
    {
        GameObject canvas = GameObject.Find("Canvas");

        TMP_Text tooCloseText = GameObject.Instantiate(PointValue, canvas.transform); // get a copy of TMP_Text to display text
        tooCloseText.fontSize = 64;
        tooCloseText.color = Color.red;
        tooCloseText.text = "Too Close";
        tooCloseText.alignment = TextAlignmentOptions.Center;
        tooCloseText.rectTransform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(duration); // display text for given time

        GameObject.Destroy(tooCloseText.gameObject);
    }

    // Coroutine to end the game
    public IEnumerator WaitAndEndGame()
    {
        yield return new WaitForSeconds(2.0f);  // Wait 2 seconds
        levelManager.LoadGameOver(); // Load game over scene
    }

    // Method to return score valuye
    public string getScore()
    {
        return ScoreValue.text;
    }

    // Method to return point valuye
    public int getPointValue()
    {
        return int.Parse(PointValue.text);
    }
}

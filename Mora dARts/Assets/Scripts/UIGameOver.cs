using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] TMP_Text GameEndTitle;  // To store the game end title of the game over scene
    [SerializeField] TMP_Text Score;  // To store and display the score
    private DartController dartController;  // To store dart controler object from the previous scene

    void Awake()
    {
        dartController = FindObjectOfType<DartController>();  // Find and get the Dart Controller object
        DestroyDarts();  // Destroy all darts
    }

    void Start()
    {
        Score.text = "Score: " + dartController.getScore();  // Update the score value
        int pointValue = dartController.getPointValue();  // Get the point value to decide whether the player won the game

        if (pointValue == 0)
        {
            // Player wins the game
            GameEndTitle.text = "You Won!";
        }
        else
        {
            // Player loses the game
            GameEndTitle.text = "You Lost!";
        }

        // Destroy the existing dart controller as it is of no use further
        Destroy(dartController);

    }

    // Method to destroy all darts after the game is over
    void DestroyDarts()
    {
        GameObject[] darts = GameObject.FindGameObjectsWithTag("dart");  // Get all remaining dart objects
        if (darts.Length > 0)
        {
            foreach (GameObject dart in darts)
            {
                Destroy(dart);  // Destroy each dart object
            }
        }

        Destroy(GameObject.FindGameObjectWithTag("DartPosition"));  // Destroy dart position indicator
    }
}

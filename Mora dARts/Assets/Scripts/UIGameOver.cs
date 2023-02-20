using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] TMP_Text GameEndTitle;  // To store the game end title of the game over scene
    [SerializeField] TMP_Text Score;  // To store and display the score
    [SerializeField] TMP_Text HighScore;  // To store and display the highest score or tell that the player obtained a new highest score
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

        int highestScore = PlayerPrefs.GetInt("highestScore", 0);  // Retirve previous highest score record. If not recorded retruns 0.
        int currentScore = int.Parse(dartController.getScore());   // Retrieve score value of the player
        if (highestScore < currentScore)
        {
            // New Highest score
            HighScore.text = "New Highest Score!";
            PlayerPrefs.SetInt("highestScore", currentScore);  // Update with the newest highest score value
        }
        else
        {
            // Dsiplay previous highest score
            HighScore.text = "Highest Score: " + highestScore.ToString();
        }

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

        GameObject arSessionOrigin = GameObject.FindGameObjectWithTag("AR Session Origin");  // Find and get the Dart Position Indicator game object
        if (arSessionOrigin != null) 
        {
            Destroy(arSessionOrigin);  // Destroy dart position indicator
        }

    }

    // Method to destroy all darts after the game is over
    private void DestroyDarts()
    {
        GameObject[] darts = GameObject.FindGameObjectsWithTag("dart");  // Get all remaining dart objects
        if (darts.Length > 0)
        {
            foreach (GameObject dart in darts)
            {
                Destroy(dart);  // Destroy each dart object
            }
        }
    }
}

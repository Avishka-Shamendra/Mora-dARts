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
        dartController = FindObjectOfType<DartController>();
    }

    void Start()
    {
        Score.text = "Score: " + dartController.getScore();
        int pointValue = dartController.getPointValue();

        if (pointValue == 0)
        {
            GameEndTitle.text = "You Won!";
        }
        else
        {
            GameEndTitle.text = "You Loose!";
        }

    }
}

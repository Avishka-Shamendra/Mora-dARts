using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameMenu : MonoBehaviour
{
    // Needed only when the menu is loaded directly from the game scene
    void Awake()
    {
        DestroyDarts();  // Destroy all darts

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

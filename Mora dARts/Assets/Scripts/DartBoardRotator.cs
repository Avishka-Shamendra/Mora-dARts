using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartBoardRotator : MonoBehaviour{

    // Start is called before the first frame update
    void Start(){}

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2) // if two fingers are dragged on screen
        {
            Touch screenTouch = Input.GetTouch(0);

            if (screenTouch.phase == TouchPhase.Moved)
            {
                //transform.Rotate( 0f,  screenTouch.deltaPosition.y, 0f);
            }
        }
            
    }
}

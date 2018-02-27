using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour {

    public bool gameActive = true; //Variable that states whether the game is going or not
    public int victor = 0;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(victor);
        if (victor > 0) {
            gameActive = false;
        }
    }
}

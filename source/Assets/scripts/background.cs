using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour {

    public bool gameActive = true; //Variable that states whether the game is going or not
    public int victor = 0;
    public bool player1IsHuman, player2IsHuman;
    public GameObject player1, player2;

    // Use this for initialization
    void Start() {
        if (player1IsHuman) {
            player1.GetComponent<AIControl>().enabled = false;
        }
        else {
            player1.GetComponent<humanControl>().enabled = false;
        }

        if (player2IsHuman) {
            player2.GetComponent<AIControl>().enabled = false;
        }
        else {
            player2.GetComponent<humanControl>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(victor);
        if (victor > 0) {
            gameActive = false;
        }
    }
}

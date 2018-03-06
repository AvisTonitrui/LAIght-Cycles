using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class background : MonoBehaviour {

    public bool gameActive = true; //Variable that states whether the game is going or not
    public int victor = 0; //The victor: 0 means no victor yet, 1 and 2 to respective players, 3 is a tie
    public bool player1IsHuman, player2IsHuman, training; //whether certain players are human and whether or not in training
    public GameObject player1, player2; //The players

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

        if (player1IsHuman && player2IsHuman) {
            training = false;
        }
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(victor);
        if (victor > 0) {
            gameActive = false;

            if ((player1IsHuman || player2IsHuman)) {
                if (victor == 1) {
                    SceneManager.LoadScene("player 1 win");
                }
                else if (victor == 2) {
                    SceneManager.LoadScene("player 2 win");
                }
                else {
                    SceneManager.LoadScene("tie");
                }
            }
        }
    }
}

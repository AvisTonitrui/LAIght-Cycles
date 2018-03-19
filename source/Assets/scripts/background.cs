using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class background : MonoBehaviour {

    public bool gameActive = false; //Variable that states whether the game is going or not
    public int victor = 0; //The victor: 0 means no victor yet, 1 and 2 to respective players, 3 is a tie
    public bool player1IsHuman, player2IsHuman, training; //whether certain players are human and whether or not in training
    public GameObject player1, player2; //The players
    public GameObject player1Controls, player2Controls; //The parents for the text boxes showing the controls
    public GameObject AI1Load, AI2Load, AI1Save, AI2Save;

    void restart() {
        player1.GetComponent<cycleMovement>().tiles = 0; //resetting tiles
        player2.GetComponent<cycleMovement>().tiles = 0;
        player1.GetComponent<population>().ready = false; //ressetting ready values to prevent duplication
        player2.GetComponent<population>().ready = false;
        player1.transform.position = new Vector3(-20, 0, 0); //resetting positions
        player2.transform.position = new Vector3(20, 0, 0);
        player1.GetComponent<cycleMovement>().direction = 4; //resetting directions
        player2.GetComponent<cycleMovement>().direction = 3;
        player1.GetComponent<cycleMovement>().restart(); //resetting the trails
        player2.GetComponent<cycleMovement>().restart();
        player1.GetComponent<cycleMovement>().hasHit = false;
        player2.GetComponent<cycleMovement>().hasHit = false;
        player1.GetComponent<cycleMovement>().trail = null;
        player2.GetComponent<cycleMovement>().trail = null;
        victor = 0; //resetting victor
    }

    // Use this for initialization
    void Start() {
        if (player1IsHuman) {
            player1.GetComponent<AIControl>().enabled = false; //disabling Ai controls
            player1.GetComponent<population>().enabled = false;
            player1Controls.SetActive(true);
        }
        else {
            player1.GetComponent<humanControl>().enabled = false;
            AI1Load.SetActive(true);
        }

        if (player2IsHuman) {
            player2.GetComponent<AIControl>().enabled = false;
            player2.GetComponent<population>().enabled = false;
            player2Controls.SetActive(true);
        }
        else {
            player2.GetComponent<humanControl>().enabled = false;
            AI2Load.SetActive(true);
        }

        if (player1IsHuman && player2IsHuman) {
            training = false;
        }

        if (!player1IsHuman && !player2IsHuman) {
            training = true;
        }

    }

    // Update is called once per frame
    void FixedUpdate() {
        //once the AI is ready, reset values and start the game
        if (((!player1IsHuman && player1.GetComponent<population>().ready) || player1IsHuman) && ((!player2IsHuman && player2.GetComponent<population>().ready) || player2IsHuman) && gameActive == false) {
            restart();
            //Debug.Log("starting");
            gameActive = true;
        }


        //this means that the simulation is over, if training, stuff will occur for the next session
        if (victor > 0 && gameActive) {
            gameActive = false;
            if ((!training)) {
                //goes to victory/loss screens if not training
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
            else {
                //passes info to the AI
                float score1 = player1.GetComponent<cycleMovement>().tiles; //the scores for the AI, starting as the number of tiles survived
                float score2 = player2.GetComponent<cycleMovement>().tiles; //They're still processed if a player isn't AI, but is only passed if it is

                //addition of the random factor
                score1 = score1 * (Random.value + 2) / 2.5f;
                score2 = score2 * (Random.value + 2) / 2.5f;

                //score is doubled if victor
                if (victor == 1) {
                    score1 = Mathf.Floor(score1 * 3) + 0.1f;
                    score2 = Mathf.Floor(score2);
                }
                else if (victor == 2) {
                    score2 = Mathf.Floor(score2 * 3) + 0.1f;
                    score1 = Mathf.Floor(score1);
                }

                //passing to the appropriate weight if it is AI
                if (!player1IsHuman && player1.GetComponent<population>().loaded) {
                    player1.GetComponent<population>().isos[player1.GetComponent<population>().org - 1][392] = score1;
                    player1.GetComponent<population>().simComplete = true;
                }

                if (!player2IsHuman && player2.GetComponent<population>().loaded) {
                    player2.GetComponent<population>().isos[player2.GetComponent<population>().org - 1][392] = score2;
                    player2.GetComponent<population>().simComplete = true;
                }
            }
        }
    }
}

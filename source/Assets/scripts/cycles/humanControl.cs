using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class humanControl : MonoBehaviour {

    public int player;//which player this is
    public bool isHuman = true;//boolean for if this player is human controlled
    string up, down, left, right;//These are the strings that relate to the controls for the cycle
    public GameObject cycle;//The cycle this is attached to

    // Use this for initialization
    void Start() {
        if (player == 1 && isHuman) {
            up = "Up1";
            down = "Down1";
            left = "Left1";
            right = "Right1";
        }
        else if (player == 2 && isHuman) {
            up = "Up2";
            down = "Down2";
            left = "Left2";
            right = "Right2";
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown(up)) {
            cycle.GetComponent<cycleMovement>().direction = 1;
        }
        else if (Input.GetButtonDown(down)) {
            cycle.GetComponent<cycleMovement>().direction = 2;
        }
        else if (Input.GetButtonDown(left)) {
            cycle.GetComponent<cycleMovement>().direction = 3;
        }
        else if (Input.GetButtonDown(right)) {
            cycle.GetComponent<cycleMovement>().direction = 4;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class humanControl : MonoBehaviour {

    public int player;//which player this is
    public bool isHuman = true;//boolean for if this player is human controlled
    public string up, down, left, right;//The next 4 are the strings for the control keycodes

	// Use this for initialization
	void Start () {
		if (player == 1) {
            up = "Up1";
            down = "Down1";
            left = "Left1";
            right = "Right1";
        } else if (player == 2) {
            up = "Up2";
            down = "Down2";
            left = "Left2";
            right = "Right2";
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

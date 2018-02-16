using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cycleMovement : MonoBehaviour {
    //This class is for moving the cycles

    public int direction; //This is an int which determines which direction the cycle will go
    private const int speed = 5; //The speed of the cycle, which is constant but will need testing
    public GameObject cycle;
    
    private void turn(int face) {
        if (face == 1) {
            cycle.transform.eulerAngles = new Vector3(0, 0, 90);
        }else if (face == 2) {

        }
    }

	// Use this for initialization
	void Start () {
        turn(1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

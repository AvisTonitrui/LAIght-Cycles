using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cycleMovement : MonoBehaviour {
    //This class is for moving the cycles

    public int direction; //This is an int which determines which direction the cycle will go
    private const float speed = 0.25f; //The speed of the cycle, which is constant but will need testing
    public GameObject cycle;
    public Object trail;
    
    private void turn(int face) { // turns the cycle to face up, down, left, or right (1, 2, 3, 4 respectively)
        if (face == 1) {
            cycle.transform.eulerAngles = new Vector3(0, 0, 90); //turn up
        }else if (face == 2) {
            cycle.transform.eulerAngles = new Vector3(0, 0, 270); //turn down
        }else if (face == 3) {
            cycle.transform.eulerAngles = new Vector3(0, 0, 180); //turn left
        }else if (face == 4) {
            cycle.transform.eulerAngles = new Vector3(0, 0, 0); //turn right
        }
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        cycle.transform.position = Vector3.MoveTowards(cycle.transform.position, cycle.transform.position + cycle.transform.right, speed); //constantly moves the cycle forward

        if (cycle.transform.position.x % 1 == 0 && cycle.transform.position.y % 1 == 0) { //checks if cycle is on a grid point
            
        }
	}
}

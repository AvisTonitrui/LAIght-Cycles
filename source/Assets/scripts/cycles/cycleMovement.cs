using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cycleMovement : MonoBehaviour {
    //This class is for moving the cycles

    public int direction; //This is an int which determines which direction the cycle will go
    private const float speed = 0.25f; //The speed of the cycle, which is constant but will need testing
    public GameObject cycle; //The cycle te script is attached to
    GameObject trail; //The trail of the cycle
    public Sprite color; //The sprite that chooses the color of the trail
    public GameObject processing; //The background processing
    public int player, opponent; //The player and the opponent numbers
    public GameObject cycleOpponent; //The actual cycle of the opponent
    public bool hasHit = false; //If the opponent has hit something, preventing the reset of the victor value and preventing a collision
    public GameObject marker, N, E, S, W, NE, SE, SW, NW; //The markers for AI processing
    const int wallCoordinate = 45; //The constant coordinate for the wals

    private void placeMarkers() { //Function that places the markers onto the walls for AI processing
        //initializing a couple variables to reduce fetching time
        float mainMarkerX = marker.transform.position.x;
        float mainMarkerY = marker.transform.position.y;

        //placing the horizontal axis markers
        N.transform.position = new Vector3(mainMarkerX, wallCoordinate, 0);
        E.transform.position = new Vector3(wallCoordinate, mainMarkerY, 0);
        S.transform.position = new Vector3(mainMarkerX, -wallCoordinate, 0);
        W.transform.position = new Vector3(-wallCoordinate, mainMarkerY, 0);

        //placing NE marker
        if (wallCoordinate - marker.transform.position.y > wallCoordinate - marker.transform.position.x) {
            NE.transform.position = N.transform.position + new Vector3(0, wallCoordinate - marker.transform.position.y, 0);
        }
        else {
            NE.transform.position = E.transform.position + new Vector3(wallCoordinate - marker.transform.position.x, 0, 0);
        }

        //placing SE
        if (marker.transform.position.y + wallCoordinate > wallCoordinate - marker.transform.position.x) {
            SE.transform.position = S.transform.position + new Vector3(0, marker.transform.position.y + wallCoordinate, 0);
        }
        else {
            SE.transform.position = E.transform.position + new Vector3(wallCoordinate - marker.transform.position.x, 0, 0);
        }

        //placing SW (TODO)
    }

    RaycastHit2D[] turn(int face) { // turns the cycle to face up, down, left, or right (1, 2, 3, 4 respectively), raycasts for what's ahead and places the marker for the AI
        if (face == 1) {
            cycle.transform.eulerAngles = new Vector3(0, 0, 90); //turn up
            cycle.transform.position = new Vector3(Mathf.Round(cycle.transform.position.x), cycle.transform.position.y, 0);
            marker.transform.position = cycle.transform.position + new Vector3(0, 1, 0);
            return Physics2D.RaycastAll(cycle.transform.position, Vector3.up, 1);
        }
        else if (face == 2) {
            cycle.transform.eulerAngles = new Vector3(0, 0, 270); //turn down
            cycle.transform.position = new Vector3(Mathf.Round(cycle.transform.position.x), cycle.transform.position.y, 0);
            marker.transform.position = cycle.transform.position + new Vector3(0, -1, 0);
            return Physics2D.RaycastAll(cycle.transform.position, Vector3.up * -1, 1);
        }
        else if (face == 3) {
            cycle.transform.eulerAngles = new Vector3(0, 0, 180); //turn left
            cycle.transform.position = new Vector3(cycle.transform.position.x, Mathf.Round(cycle.transform.position.y), 0);
            marker.transform.position = cycle.transform.position + new Vector3(-1, 0, 0);
            return Physics2D.RaycastAll(cycle.transform.position, Vector3.left, 1);
        }
        else if (face == 4) {
            cycle.transform.eulerAngles = new Vector3(0, 0, 0); //turn right
            cycle.transform.position = new Vector3(cycle.transform.position.x, Mathf.Round(cycle.transform.position.y), 0);
            marker.transform.position = cycle.transform.position + new Vector3(1, 0, 0);
            return Physics2D.RaycastAll(cycle.transform.position, Vector3.right, 1);
        }
        else {
            return null;
        }
    }

    // Use this for initialization
    void Start() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        trail = collision.gameObject;
    }

    // Update is called once per frame
    void Update() {
        //Keeps the cycle moving
        if (processing.GetComponent<background>().gameActive == true) {
            cycle.transform.position = Vector3.MoveTowards(cycle.transform.position, cycle.transform.position + cycle.transform.right, speed); //Constantly moves the cycle forward
        }
        else {
            trail.GetComponent<Renderer>().enabled = false;
        }

        //Corrects floating Point errors
        if (Mathf.Abs(cycle.transform.position.x) < 0.01) {
            cycle.transform.position = new Vector3(0, cycle.transform.position.y, 0);
        }

        if (Mathf.Abs(cycle.transform.position.y) < 0.01) {
            cycle.transform.position = new Vector3(cycle.transform.position.x, 0, 0);
        }

        //turns cycle, creates trails, and ensures the next grid spot it's going to is legal
        if (Mathf.Approximately(cycle.transform.position.x % 1, 0) && Mathf.Approximately(cycle.transform.position.y % 1, 0)) { //Checks if cycle is on a grid point
            RaycastHit2D[] hits; //The variable for what raycasts hit
            hits = turn(direction); //Turns the cycle in the last direction it was given
            //These are the controls to make sure that the square the cycle is heading into is legal


            if (hits.Length != 0 && processing.GetComponent<background>().gameActive == true) {
                //Debug.Log("Hit");
                int winner = 0;
                int cycles = 0;

                for (int i = 0; i < hits.Length; i++) {
                    if (hits[i].collider.gameObject.tag == "wall") { // hit a wall
                        winner = opponent;
                        cycleOpponent.GetComponent<cycleMovement>().hasHit = true;
                        //Debug.Log(winner);
                    }
                    else if (hits[i].collider.gameObject.tag == "cycle") {
                        cycles += 1;
                        if (cycles >= 2) {
                            winner = 3;
                            //Debug.Log(winner);
                            processing.GetComponent<background>().victor = winner;
                            break;
                        }
                    }
                    else if (hits[i].collider.gameObject.tag == "trail") { //hit a trail, making sure that that is an active trail
                        if (hits[i].collider.gameObject.GetComponent<Renderer>().enabled == true) {
                            winner = opponent;
                            cycleOpponent.GetComponent<cycleMovement>().hasHit = true;
                            //Debug.Log(winner);
                        }
                    }

                    if (!hasHit) {
                        processing.GetComponent<background>().victor = winner;
                        //Debug.Log(winner);
                    }

                }
            }

            trail.GetComponent<SpriteRenderer>().sprite = color; //Changes the trail to the proper color
            trail.GetComponent<Renderer>().enabled = true; //Enables the trail
        }


    }
}

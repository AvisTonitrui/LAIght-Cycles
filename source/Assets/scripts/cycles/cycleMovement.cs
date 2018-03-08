using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cycleMovement : MonoBehaviour {
    //This class is for moving the cycles

    public int direction; //This is an int which determines which direction the cycle will go
    private const float speed = 0.25f; //The speed of the cycle, which is constant but will need testing
    public GameObject cycle; //The cycle te script is attached to
    GameObject trail; //The trail of the cycle
    int trailX, trailY; //the position of the trail
    public Sprite color; //The sprite that chooses the color of the trail
    public GameObject processing; //The background processing
    public int player, opponent; //The player and the opponent numbers
    public GameObject cycleOpponent; //The actual cycle of the opponent
    public bool hasHit = false; //If the opponent has hit something, preventing the reset of the victor value and preventing a collision
    public GameObject marker; //The markers for AI processing
    const int gridSize = 44; //The size of the grid along each axis in 4 directions
    const int wallCoordinate = gridSize + 1; //The constant coordinate for the wals
    int tiles = 0; //the number of tiles that the cycle has moved, used for AI

    RaycastHit2D[] turn(int face) { // turns the cycle to face up, down, left, or right (1, 2, 3, 4 respectively), raycasts for what's ahead and places the marker for the AI
        if (face == 1) {
            cycle.transform.eulerAngles = new Vector3(0, 0, 90); //turn up
            cycle.transform.position = new Vector3(Mathf.Round(cycle.transform.position.x), cycle.transform.position.y, 0);
            marker.transform.position = cycle.transform.position + new Vector3(0, 1, 0);

            //calling processes for the AI
            if (player == 1 && !processing.GetComponent<background>().player1IsHuman) {
                cycle.GetComponent<AIControl>().getIn(cycle.transform.position, cycleOpponent.transform.position);

            }
            else if (player == 2 && !processing.GetComponent<background>().player2IsHuman) {
                cycle.GetComponent<AIControl>().getIn(cycle.transform.position, cycleOpponent.transform.position);
            }

            return Physics2D.RaycastAll(cycle.transform.position, Vector3.up, 1);
        }
        else if (face == 2) {
            cycle.transform.eulerAngles = new Vector3(0, 0, 270); //turn down
            cycle.transform.position = new Vector3(Mathf.Round(cycle.transform.position.x), cycle.transform.position.y, 0);
            marker.transform.position = cycle.transform.position + new Vector3(0, -1, 0);

            //calling processes for the AI
            if (player == 1 && !processing.GetComponent<background>().player1IsHuman) {
                cycle.GetComponent<AIControl>().getIn(cycle.transform.position, cycleOpponent.transform.position);
            }
            else if (player == 2 && !processing.GetComponent<background>().player2IsHuman) {
                cycle.GetComponent<AIControl>().getIn(cycle.transform.position, cycleOpponent.transform.position);
            }

            return Physics2D.RaycastAll(cycle.transform.position, Vector3.up * -1, 1);
        }
        else if (face == 3) {
            cycle.transform.eulerAngles = new Vector3(0, 0, 180); //turn left
            cycle.transform.position = new Vector3(cycle.transform.position.x, Mathf.Round(cycle.transform.position.y), 0);
            marker.transform.position = cycle.transform.position + new Vector3(-1, 0, 0);

            //calling processes for the AI
            if (player == 1 && !processing.GetComponent<background>().player1IsHuman) {
                cycle.GetComponent<AIControl>().getIn(cycle.transform.position, cycleOpponent.transform.position);
            }
            else if (player == 2 && !processing.GetComponent<background>().player2IsHuman) {
                cycle.GetComponent<AIControl>().getIn(cycle.transform.position, cycleOpponent.transform.position);
            }

            return Physics2D.RaycastAll(cycle.transform.position, Vector3.left, 1);
        }
        else if (face == 4) {
            cycle.transform.eulerAngles = new Vector3(0, 0, 0); //turn right
            cycle.transform.position = new Vector3(cycle.transform.position.x, Mathf.Round(cycle.transform.position.y), 0);
            marker.transform.position = cycle.transform.position + new Vector3(1, 0, 0);

            //calling processes for the AI
            if (player == 1 && !processing.GetComponent<background>().player1IsHuman) {
                cycle.GetComponent<AIControl>().getIn(cycle.transform.position, cycleOpponent.transform.position);
            }
            else if (player == 2 && !processing.GetComponent<background>().player2IsHuman) {
                cycle.GetComponent<AIControl>().getIn(cycle.transform.position, cycleOpponent.transform.position);
            }

            return Physics2D.RaycastAll(cycle.transform.position, Vector3.right, 1);
        }
        else {
            return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        trail = collision.gameObject;
        trail.GetComponent<SpriteRenderer>().sprite = color; //Changes the trail to the proper color
        trailX = Mathf.RoundToInt(trail.transform.position.x + gridSize); //sets the relative coordinates for the trail for use in our array
        trailY = Mathf.RoundToInt(trail.transform.position.y + gridSize);
    }

    // Use this for initialization
    void Start() {
        cycle.GetComponent<AIControl>().trailMap = processing.GetComponent<gridStart>().trailMap;
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

            trail.GetComponent<Renderer>().enabled = true; //Enables the trail
            cycle.GetComponent<AIControl>().trailMap[trailX, trailY] = true;
            tiles++;
        }


    }
}

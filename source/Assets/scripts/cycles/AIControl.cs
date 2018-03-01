using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour { //This class takes the weights from an organism and uses it for controlling the cycle

    float[] weights = new float[392]; //The wieghts specified by the organism
    float[] inputs = new float[24]; //The inputs, which change at each gridpoint
    float[] hiddens = new float[14]; //The hidden neurons
    float[] outputs = new float[4]; //The output neurons
    public GameObject cycle; //The cycle this is attached to
    public bool haveIns;
    const int gridSize = 44;
    public bool[,] trailMap = new bool[gridSize, gridSize];

    void setWallDist(Vector3 cyclePos, int wallNX, int wallEY, int wallSX, int wallWY) {
        //Cardinal directions first
        inputs[1] = wallNX - cyclePos.x;
        inputs[2] = wallEY - cyclePos.y;
        inputs[3] = cyclePos.x - wallSX;
        inputs[4] = cyclePos.y - wallWY;

        //NE
        if (inputs[1] < inputs[2]) {
            inputs[5] = inputs[1] * Mathf.Sqrt(2);
        }
        else {
            inputs[5] = inputs[2] * Mathf.Sqrt(2);
        }

        //SE
        if (inputs[3] < inputs[2]) {
            inputs[5] = inputs[3] * Mathf.Sqrt(2);
        }
        else {
            inputs[5] = inputs[2] * Mathf.Sqrt(2);
        }

        //SW
        if (inputs[3] < inputs[4]) {
            inputs[5] = inputs[3] * Mathf.Sqrt(2);
        }
        else {
            inputs[5] = inputs[4] * Mathf.Sqrt(2);
        }

        //NW
        if (inputs[1] < inputs[4]) {
            inputs[5] = inputs[1] * Mathf.Sqrt(2);
        }
        else {
            inputs[5] = inputs[4] * Mathf.Sqrt(2);
        }
    }

    void setCycleDist(Vector3 thisCyclePos, Vector3 enemyCyclePos) {
        //First check if Ys are equal, meaning N or S
        if (thisCyclePos.y == enemyCyclePos.y) {
            //set all but N and S to 0
            inputs[10] = 0;
            inputs[12] = 0;
            inputs[13] = 0;
            inputs[14] = 0;
            inputs[15] = 0;
            inputs[16] = 0;

            //checks whether N or S
            if (thisCyclePos.y < enemyCyclePos.y) {
                //N
                inputs[9] = enemyCyclePos.y - thisCyclePos.y;
                inputs[11] = 0;
            }
            else {
                inputs[9] = 0;
                inputs[11] = thisCyclePos.y - enemyCyclePos.y;
            }
        }

        //Next, check if Xs are equal, denoting E or W
        if (thisCyclePos.x == enemyCyclePos.x) {
            //set all but E and W to 0
            inputs[9] = 0;
            inputs[11] = 0;
            inputs[13] = 0;
            inputs[14] = 0;
            inputs[15] = 0;
            inputs[16] = 0;

            //checks whether E or W
            if (thisCyclePos.x < enemyCyclePos.x) {
                //N
                inputs[10] = enemyCyclePos.x - thisCyclePos.x;
                inputs[12] = 0;
            }
            else {
                inputs[10] = 0;
                inputs[12] = thisCyclePos.x - enemyCyclePos.x;
            }
        }

        //Find the slope to determine if the cycles are along a diagonal
        float slope = (thisCyclePos.y - enemyCyclePos.y) / (thisCyclePos.x - enemyCyclePos.x);

        //Either NE or SW
        if (slope == 1) {
            //set all but NE and SW to 0
            inputs[9] = 0;
            inputs[10] = 0;
            inputs[11] = 0;
            inputs[12] = 0;
            inputs[14] = 0;
            inputs[16] = 0;

            //checks whether NE or SW
            if (thisCyclePos.x < enemyCyclePos.x) {
                //N
                inputs[13] = (enemyCyclePos.x - thisCyclePos.x) * Mathf.Sqrt(2);
                inputs[15] = 0;
            }
            else {
                inputs[13] = 0;
                inputs[15] = (thisCyclePos.x - enemyCyclePos.x) * Mathf.Sqrt(2);
            }
        }

        //Either SE or NW
        if (slope == -1) {
            //set all but SE and NW to 0
            inputs[9] = 0;
            inputs[10] = 0;
            inputs[11] = 0;
            inputs[12] = 0;
            inputs[13] = 0;
            inputs[15] = 0;

            //checks whether SE or NW
            if (thisCyclePos.x < enemyCyclePos.x) {
                //N
                inputs[14] = (enemyCyclePos.x - thisCyclePos.x) * Mathf.Sqrt(2);
                inputs[16] = 0;
            }
            else {
                inputs[14] = 0;
                inputs[16] = (thisCyclePos.x - enemyCyclePos.x) * Mathf.Sqrt(2);
            }
        }

    }

    void setTrailDist() {

    }

    //Function that calls functions to fill out the input nodes for this gridpoint
    public void getIn(Vector3 thisCyclePos, Vector3 enemyCyclePos, int wallNX, int wallEY, int wallSX, int wallWY) {
        //Get wall distance
        setWallDist(thisCyclePos, wallNX, wallEY, wallSX, wallWY);

        //Get cycle distance, if applicable
        setCycleDist(thisCyclePos, enemyCyclePos);

        //Get distance to trail, if applicable
        setTrailDist();

    }

    // Use this for initialization
    void Start() {
        haveIns = false;
    }

    // Update is called once per frame
    void Update() {
        
    }
}

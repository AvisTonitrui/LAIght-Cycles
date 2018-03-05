using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour { //This class takes the weights from an organism and uses it for controlling the cycle

    public float[] weights = new float[392]; //The wieghts specified by the organism
    float[] inputs = new float[24]; //The inputs, which change at each gridpoint
    float[] hiddens = new float[14]; //The hidden neurons
    float[] outputs = new float[4]; //The output neurons
    public GameObject cycle; //The cycle this is attached to
    public bool haveIns, haveHiddens, haveOuts; //Booleans for knowing where we are at in calculations
    const int gridSize = 44;
    const int wallCoordinate = gridSize + 1;
    public bool[,] trailMap = new bool[gridSize, gridSize];

    //sets the inputs for wall distance
    void setWallDist(Vector3 cyclePos) {
        //Cardinal directions first
        inputs[0] = wallCoordinate - cyclePos.y;
        inputs[1] = wallCoordinate - cyclePos.x;
        inputs[2] = cyclePos.y - wallCoordinate;
        inputs[3] = cyclePos.x - wallCoordinate;

        //NE
        if (inputs[0] < inputs[1]) {
            inputs[4] = inputs[0] * Mathf.Sqrt(2);
        }
        else {
            inputs[4] = inputs[1] * Mathf.Sqrt(2);
        }

        //SE
        if (inputs[2] < inputs[1]) {
            inputs[5] = inputs[2] * Mathf.Sqrt(2);
        }
        else {
            inputs[5] = inputs[1] * Mathf.Sqrt(2);
        }

        //SW
        if (inputs[2] < inputs[3]) {
            inputs[6] = inputs[2] * Mathf.Sqrt(2);
        }
        else {
            inputs[6] = inputs[3] * Mathf.Sqrt(2);
        }

        //NW
        if (inputs[1] < inputs[4]) {
            inputs[7] = inputs[1] * Mathf.Sqrt(2);
        }
        else {
            inputs[7] = inputs[4] * Mathf.Sqrt(2);
        }
    }

    //sets the inputs for cycle distance
    void setCycleDist(Vector3 thisCyclePos, Vector3 enemyCyclePos) {
        //First check if Ys are equal, meaning N or S
        if (thisCyclePos.y == enemyCyclePos.y) {
            //set all but N and S to 0
            inputs[9] = 0;
            inputs[11] = 0;
            inputs[12] = 0;
            inputs[13] = 0;
            inputs[14] = 0;
            inputs[15] = 0;

            //checks whether N or S
            if (thisCyclePos.y < enemyCyclePos.y) {
                //N
                inputs[8] = enemyCyclePos.y - thisCyclePos.y;
                inputs[10] = 0;
            }
            else {
                inputs[8] = 0;
                inputs[10] = thisCyclePos.y - enemyCyclePos.y;
            }
        }

        //Next, check if Xs are equal, denoting E or W
        if (thisCyclePos.x == enemyCyclePos.x) {
            //set all but E and W to 0
            inputs[8] = 0;
            inputs[10] = 0;
            inputs[12] = 0;
            inputs[13] = 0;
            inputs[14] = 0;
            inputs[15] = 0;

            //checks whether E or W
            if (thisCyclePos.x < enemyCyclePos.x) {
                //N
                inputs[9] = enemyCyclePos.x - thisCyclePos.x;
                inputs[11] = 0;
            }
            else {
                inputs[9] = 0;
                inputs[11] = thisCyclePos.x - enemyCyclePos.x;
            }
        }

        //Find the slope to determine if the cycles are along a diagonal
        float slope = (thisCyclePos.y - enemyCyclePos.y) / (thisCyclePos.x - enemyCyclePos.x);

        //Either NE or SW
        if (slope == 1) {
            //set all but NE and SW to 0
            inputs[8] = 0;
            inputs[9] = 0;
            inputs[10] = 0;
            inputs[11] = 0;
            inputs[13] = 0;
            inputs[15] = 0;

            //checks whether NE or SW
            if (thisCyclePos.x < enemyCyclePos.x) {
                //N
                inputs[12] = (enemyCyclePos.x - thisCyclePos.x) * Mathf.Sqrt(2);
                inputs[14] = 0;
            }
            else {
                inputs[12] = 0;
                inputs[14] = (thisCyclePos.x - enemyCyclePos.x) * Mathf.Sqrt(2);
            }
        }

        //Either SE or NW
        if (slope == -1) {
            //set all but SE and NW to 0
            inputs[8] = 0;
            inputs[9] = 0;
            inputs[10] = 0;
            inputs[11] = 0;
            inputs[12] = 0;
            inputs[14] = 0;

            //checks whether SE or NW
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

    }

    //separate function for each trail distance direction if needed to offload to other cores
    float FindTrailDistance(int xStart, int yStart, int xInc, int yInc) {
        //setting some variables to increment
        int x = xStart;
        int y = yStart;
        int diagonal; //Whether or not we are on a diagonal, 1 if not, 2 if we are.

        //setting diagonal
        if (yInc != 0 && xInc != 0) {
            diagonal = 2;
        }
        else {
            diagonal = 1;
        }

        //goes through and checks point by point if there is an active trail there
        while (0 < x && x < gridSize * 2 && 0 < y && y < gridSize * 2) {
            if (trailMap[x, y] && xInc != 0) {
                return Mathf.Abs(xStart - x) * Mathf.Sqrt(diagonal);
            }
            else if (trailMap[x, y] && yInc != 0) {
                return Mathf.Abs(yStart - y) * Mathf.Sqrt(diagonal);
            }

            //incrementing x and y
            x += xInc;
            y += yInc;
        }

        //if no trail is detected, returns 0
        return 0;
    }

    //sets the inputs for trail distance
    void setTrailDist(Vector3 thisCyclePos) {
        //First, set the relative X and Y coordinates of the cycle
        int cycleX = Mathf.RoundToInt(thisCyclePos.x + gridSize);
        int cycleY = Mathf.RoundToInt(thisCyclePos.y + gridSize);

        //Calls for each direction in case of the need to offload to other cores (order is N, S, E, W, NE, SE, SW, NW)
        inputs[16] = FindTrailDistance(cycleX, cycleY, 0, 1);
        inputs[17] = FindTrailDistance(cycleX, cycleY, 1, 0);
        inputs[18] = FindTrailDistance(cycleX, cycleY, 0, -1);
        inputs[19] = FindTrailDistance(cycleX, cycleY, -1, 0);
        inputs[20] = FindTrailDistance(cycleX, cycleY, 1, 1);
        inputs[21] = FindTrailDistance(cycleX, cycleY, 1, -1);
        inputs[22] = FindTrailDistance(cycleX, cycleY, -1, -1);
        inputs[23] = FindTrailDistance(cycleX, cycleY, -1, 1);
    }

    //Function that calls functions to fill out the input nodes for this gridpoint
    public void getIn(Vector3 thisCyclePos, Vector3 enemyCyclePos) {
        //Get wall distance
        setWallDist(thisCyclePos);

        //Get cycle distance, if applicable
        setCycleDist(thisCyclePos, enemyCyclePos);

        //Get distance to trail, if applicable
        setTrailDist(thisCyclePos);

        //letting the system know that the inputs are in
        haveIns = true;

    }

    //function for setting the hidden nodes
    void setHidden() {
        //starting by resetting the hiddens from the last time
        for (int i = 0; i < hiddens.Length; i++) {
            hiddens[i] = 0;
        }

        int inp = -1;
        int hid = 0;

        //iterate through the weights that go from inputs to hiddens
        for (int i = 0; i < 336; i++) {
            //checking to see if it's time to start taking the next input
            if ((i) % 14 == 0) {
                inp++;
            }

            hiddens[hid] += inputs[inp] * weights[i];
            hid++;

            if (hid == hiddens.Length) {
                hid = 0;
            }
        }

        haveHiddens = true;
        haveIns = false;
    }

    //function for setting the output nodes
    void setOut() {
        //reset the outputs for this iteration
        for (int i = 0; i < outputs.Length; i++) {
            outputs[i] = 0;
        }

        int hid = -1;
        int ou = 0;

        //iterate through the weights that go from hiddens to outputs
        for (int i = 336; i < weights.Length; i++) {
            //checking if it's time to go to the next hidden node
            if ((i) % 4 == 0) {
                hid++;

            }

            outputs[ou] += hiddens[hid] * weights[i];
            ou++;

            if (ou == outputs.Length) {
                ou = 0;
            }
        }

        haveOuts = true;
        haveHiddens = false;
    }

    //function for setting the turn direction
    void setDirection() {
        int direction = 0;
        int i = 0;

        foreach (float outp in outputs) {
            //set the new direction if there is a greater number
            if (outputs[direction] < outputs[i]) {
                direction = i;
            }

            i++;
        }

        //incrementing direction so it fits properly with the turn function
        direction++;

        cycle.GetComponent<cycleMovement>().direction = direction;
    }

    // Use this for initialization
    void Start() {
        haveIns = false;
    }

    // Update is called once per frame
    void Update() {
        //once inputs are found, set the hiddens
        if (haveIns) {
            setHidden();
        }

        //once the hiddens are calculated, set the outputs
        if (haveHiddens) {
            setOut();
        }

        //once the outputs are set, determine direction and set the direction
        if (haveOuts) {
            setDirection();
        }
    }
}

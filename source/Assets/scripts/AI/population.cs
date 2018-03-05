using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class population : MonoBehaviour { //This script is for controlling the genetic algorithm, both population and breeding/eliminating

    public float[][] isos = new float[99][]; //The entire population for this generation
    public int gen; //The generation number, for ducumenting purposes
    public bool save; //Wehther we're loading a save or starting from the beginning
    public bool ready; //Whether the population is ready to begin or not

    //Function that makes a randomly generated population from scratch
    public void makePop() {
        //double iterator to run through every element in this two-dimensional array
        for (int i = 0; i < isos.Length; i++) {
            for (int j = 0; j < isos[i].Length; j++) {
                isos[i][j] = Random.value;
            }
        }
    }

    //determining whether to get a save file or create a new one when called
    public void readyUp() {
        if (save) {

        }
        else {
            makePop();
        }
    }

    // Use this for initialization
    void Start() {
        //fills out isos with declared arrays
        for (int i = 0; i < isos.Length; i++) {
            isos[i] = new float[392];
        }

    }

    // Update is called once per frame
    void Update() {

    }
}

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class population : MonoBehaviour { //This script is for controlling the genetic algorithm, both population and breeding/eliminating

    public float[][] isos = new float[99][]; //The entire population for this generation
    public int gen; //The generation number, for ducumenting purposes
    public bool save = false; //Wehther we're loading a save or starting from the beginning
    public bool ready = false; //Whether the population is ready to begin or not
    public GameObject cycle; //The cycle this script is attached to
    public string saveName;

    //Function that makes a randomly generated population from scratch
    void makePop() {
        //double iterator to run through every element in this two-dimensional array
        for (int i = 0; i < isos.Length; i++) {
            for (int j = 0; j < isos[i].Length; j++) {
                isos[i][j] = Random.value;
            }
        }
    }

    //parses the save file to get the population and the generation number
    float[][] parseSave() {
        string line;//The line we're on
        StreamReader save = new StreamReader("Saves/" + saveName + ".txt"); //The save file opened for reading
        int organism = 0; //index marker while reading the file
        float[][] saveList = new float[99][]; //The placeholder for the save file
        float[] iso = new float[392]; //The organism
        string[] isoString; //The organism still in string form

        /* Save file format:
         *   Line 1: Generation number
         *   Each succesive line: An iso in the population
         */

        gen = int.Parse(save.ReadLine()); //This reads the generation number line
        line = save.ReadLine(); //This reads the first organism

        //starts iterating through the organisms
        while (line != null) {
            //explodes the string into an array
            isoString = line.Split(',');

            //converting the strings into floats and putting them into iso
            for (int i = 0; i < isoString.Length; i++) {
                iso[i] = float.Parse(isoString[i]);
            }

            //puts this organism into the save list
            saveList[organism] = iso;

            //increment and readline for the next iteration
            organism++;
            line = save.ReadLine();
        }

        return saveList;
    }

    //determining whether to get a save file or create a new one when called
    public void readyUp() {
        if (save) {
            parseSave();
        }
        else {
            makePop();
        }

        ready = true;
    }

    //passes the weights to AI Control
    public float[] quorra() {
        return new float[392];
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

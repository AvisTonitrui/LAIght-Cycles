using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class population : MonoBehaviour { //This script is for controlling the genetic algorithm, both population and breeding/eliminating

    public float[][] isos = new float[99][]; //The entire population for this generation
    public string[] isoInfo = new string[99]; //The info on each of the isos in the population (Format: "W/L,survived tiles,random number,organism#")
    public int gen; //The generation number, for ducumenting purposes
    public bool save = false; //Wehther we're loading a save or starting from the beginning
    public bool ready = false; //Whether the population is ready to begin or not
    public bool nextIso = true; //Whether to continue to the next organism or not
    public bool nextGen = false; //Indicator to to calculations for the next generation
    int org = 0; //the index of the organism we're on
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
    public float[] quorra(int isoIndex) {
        return isos[isoIndex];
    }

    //compares two isos' info to determine which is better. True is iso 1, False is iso 2
    public bool compareInfo(string iso1Info, string iso2Info) {
        //the factors; 0 is a tie, 1 is iso 1, 2 is iso 2
        int factor1 = 0;
        int factor2 = 0;
        int factor3 = 0;

        //the number of victories per side
        int vic1 = 0;
        int vic2 = 0;

        //each factor split for easier parsing
        string[] iso1 = iso1Info.Split(',');
        string[] iso2 = iso2Info.Split(',');

        //evaluating factor1, which is whether the iso won or not
        if (iso1[0] == "W" && iso2[0] != "W") {
            factor1 = 1;
            vic1 += 1;
        }
        else if (iso1[0] != "W" && iso2[0] == "W") {
            factor1 = 2;
            vic2 += 1;
        }
        else {
            factor1 = 0;
        }

        //evaluating factor2, how long many tiles they survived
        if (int.Parse(iso1[1]) > int.Parse(iso2[1])) {
            factor2 = 1;
            vic1 += 1;
        }
        else if (int.Parse(iso1[1]) < int.Parse(iso2[1])) {
            factor2 = 2;
            vic2 += 1;
        }
        else {
            factor2 = 0;
        }

        //evaluating factor3, a randomly generated number between 0 and 1 used as a tiebreaker and to simulate a degree of randomness
        if (int.Parse(iso1[2]) > int.Parse(iso2[2])) {
            factor3 = 1;
            vic1 += 1;
        }
        else if (int.Parse(iso1[2]) < int.Parse(iso1[2])) {
            factor3 = 2;
            vic2 += 1;
        }
        else {
            factor3 = 0;
        }

        //checking if one side one by majority, and returning the victor if so
        if (vic1 > vic2) {
            return true;
        }
        else if (vic1 < vic2) {
            return false;
        }

        //the tiebreaker, checking through each factor in order of priority and seeing who won the highest priority. 1 wins if everything tied
        if (factor1 == 1) {
            return true;
        }
        else if (factor1 == 2) {
            return false;
        }
        else if (factor2 == 1) {
            return true;
        }
        else if (factor2 == 2) {
            return false;
        }
        else if (factor3 == 1) {
            return true;
        }
        else if (factor3 == 2) {
            return false;
        }
        else {
            return true;
        }

    }

    //sorts the isos, using 
    public float[][] sortIsos(float[][] isoList, string[] info) {
        //variable declarations for the function
        string[] hold1 = info;
        string[] hold2 = new string[99];
        float[][] myReturn;



        //dummy return to stop the hassle
        return new float[392][];
    }

    //creates the next generation and resets needed variables
    public void newGen() {

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
        //gives AI control the next organism
        if (nextIso) {
            quorra(org);
            org++;
            nextIso = false;
        }

    }
}

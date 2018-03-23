using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions; // needed for Regex
using UnityEngine.UI;

public class population : MonoBehaviour { //This script is for controlling the genetic algorithm, both population and breeding/eliminating

    public float[][] isos = new float[99][]; //The entire population for this generation. Each has a score in the following format ((W/L) * Tiles Survived * R) a win is 2, a loss is 1, R is a random number between 1 and 1.1 
    public int gen = 0; //The generation number, for ducumenting purposes
    public bool nextIso = false; //Whether to continue to the next organism or not
    public bool nextGen = false; //Indicator to do calculations for the next generation
    public bool simComplete = false; //Turns true when the current simulation ends
    public bool ready = false; //Lets processing know that AI is prepped for the next simulation
    public int org = 0; //the index of the organism we're on
    public GameObject cycle; //The cycle this script is attached to
    string saveName; //The name of the save file
    const int SCOREINDEX = 392; //The index that the score is kept at
    public GameObject input1, input2; //The input for the save file name
    public bool loaded = false; //To show that we currently have a loaded file in
    public GameObject genDisplay, orgDisplay, simpleToggle, bonusChange;
    public float winBonus = 2; //the bonus value

    //function for sanitizing input for save names
    string sanitize(string given) {
        string output = given;

        output = Regex.Replace(output, @"[^a-zA-Z0-9\s-_]", "");
        output = Regex.Replace(output, @"[^a-zA-Z0-9-_]", "-");

        return output;
    }

    //function for deloading for the button to activate
    public void deLoad() {
        loaded = false;
        nextIso = false;
        nextGen = false;
        ready = false;
        org = 0;
        gen = 0;
    }

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
    void parseSave() {
        saveName = sanitize(input1.GetComponent<InputField>().text);
        string line;//The line we're on
        StreamReader save = new StreamReader("Saves/" + saveName + ".txt"); //The save file opened for reading
        int organism = 0; //index marker while reading the file
        float[][] saveList = new float[99][]; //The placeholder for the save file

        /* Save file format:
         *   Line 1: Generation number
         *   Each succesive line: An iso in the population
         */

        gen = int.Parse(save.ReadLine()); //This reads the generation number line
        line = save.ReadLine(); //This reads the first organism

        //starts iterating through the organisms
        while (line != null) {
            string[] isoString; //The organism still in string form
            float[] iso = new float[393]; //The organism
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
        //closing the file before returning
        save.Close();
        isos = saveList;
    }

    //determining whether to get a save file or create a new one when called
    public void readyUp(bool save) {
        if (save) {
            parseSave();
        }
        else {
            makePop();
        }

        genDisplay.GetComponent<Text>().text = "Gen: " + gen;
        nextIso = true;
        loaded = true;
    }

    //compares two isos' info to determine which is better. True is iso 1, False is iso 2
    bool compareInfo(float[] iso1, float[] iso2, bool simple) {
        if (simple) {
            return iso1[SCOREINDEX] >= iso2[SCOREINDEX];
        }
        else {
            if (iso1[SCOREINDEX] % 1 == 0 && iso2[SCOREINDEX] % 1 == 0) {
                return iso1[SCOREINDEX] >= iso2[SCOREINDEX];
            }
            else if (iso1[SCOREINDEX] % 1 != 0 && iso2[SCOREINDEX] % 1 != 0) {
                return iso1[SCOREINDEX] <= iso2[SCOREINDEX];
            }
            else {
                return iso1[SCOREINDEX] >= iso2[SCOREINDEX];
            }
        }

    }

    List<float[]> merge(List<float[]> hold1, List<float[]> hold2, int mergeSize, bool simple) {
        List<float[]> mergeHold = new List<float[]>(); //The merged list
        int mergeLength = hold1.Count + hold2.Count; //The total length that mergeHold should be
        int i1 = 0;
        int i2 = mergeSize;

        //if hold2 is empty, we can just return hold1 as it is already sorted
        if (hold2.Count == 0) {
            return hold1;
        }

        while (i2 < mergeLength) {
            //sorting one by one
            if (compareInfo(hold1[i1], hold2[i2 - mergeSize], simple)) {
                mergeHold.Add(hold1[i1]);
                i1++;
            }
            else {
                mergeHold.Add(hold2[i2 - mergeSize]);
                i2++;
            }

            //adding the rest of 2 once 1 is finished
            if (i1 == mergeSize) {
                //adding the remainder of 2
                for (int i = i2 - mergeSize; i < hold2.Count; i++) {
                    mergeHold.Add(hold2[i2 - mergeSize]);
                    i2++;
                }
            }

            //adding the rest of 1 if 2 finished
            if (i2 == mergeLength) {
                //adding the remainder of 1
                for (int i = i1; i < hold1.Count; i++) {
                    mergeHold.Add(hold1[i1]);
                    i1++;
                }
            }
        }

        return mergeHold;
    }

    //sorts the isos using merge sort
    float[][] sortIsos(float[][] isoList, bool simple) {
        //variable declarations for the function
        List<float[]> hold1 = new List<float[]>(); //Holders for performing merges
        List<float[]> hold2 = new List<float[]>();
        List<float[]> mergeHold = new List<float[]>();
        float[][] myReturn = isoList; //What will get returned at the end of the sort
        int mergeSize = 1;//The size of the groups for the merge
        int j = 0; //the index of the beginning

        //looping until our mergesize is the whole of the population
        do {
            //going through all of the array and performing the sort
            for (int i = 0; i < myReturn.Length; i++) {
                //first add to the holds if they're not full yet
                if (hold1.Count < mergeSize) {
                    hold1.Add(myReturn[i]);
                }
                else if (hold2.Count < mergeSize) {
                    hold2.Add(myReturn[i]);
                }
                else { //Perform a merge and start filling out the next set of holds afterwards
                    mergeHold = merge(hold1, hold2, mergeSize, simple); //Gets the merged version of the segment we're on

                    //puts the merged segment into the array
                    foreach (float[] iso in mergeHold) {
                        myReturn[j] = iso;
                        j++;
                    }

                    //clearing the holds and adding the current iteration to hold1
                    hold1.Clear();
                    hold2.Clear();
                    hold1.Add(myReturn[i]);
                    j = i;
                }

                //checking to see if this is the last element, therefore requiring a final merge and incrementing mergeSize
                if (i + 1 == myReturn.Length) {
                    mergeHold = merge(hold1, hold2, mergeSize, simple);

                    //putting the final merge into the return
                    foreach (float[] iso in mergeHold) {
                        myReturn[j] = iso;
                        j++;
                    }

                    hold1.Clear();
                    hold2.Clear();
                    mergeSize = mergeSize * 2;
                    j = 0;
                }
            }

        } while (mergeSize < isoList.Length);


        //return the sorted array
        return myReturn;
    }

    //breeds two Isos by averaging their values and possibly causing a mutation
    float[] breedIsos(float[] iso1, float[] iso2) {
        float[] isoSpawn = new float[393];

        for (int i = 0; i < isoSpawn.Length; i++) {
            isoSpawn[i] = (iso1[i] + iso2[i]) / 2;

            //mutation check
            if (Random.value >= 0.99) {
                isoSpawn[i] = isoSpawn[i] * (Random.value + 2) / 2.5f;
            }
        }

        return isoSpawn;
    }

    //creates the next generation and resets needed variables
    public void newGen() {
        //getting the current multiplier
        try {
            winBonus = float.Parse(bonusChange.GetComponent<InputField>().text);
        }
        catch {
            winBonus = globals.winBonus;
        }


        //reapplying the win bonus
        foreach (float[] iso in isos) {
            //checking if the iso was a victor
            if (iso[SCOREINDEX] % 1 != 0) {
                iso[SCOREINDEX] = Mathf.Floor((iso[SCOREINDEX] - 0.1f) / globals.winBonus * winBonus) + 0.1f;
            }
        }

        //setting the global multiplier to keep track of last used
        globals.winBonus = winBonus;

        //setting the background's winBonus to temporarily calculate for this generation
        cycle.GetComponent<cycleMovement>().processing.GetComponent<background>().winBonus = winBonus;

        //ranking each Iso
        isos = sortIsos(isos, simpleToggle.GetComponent<Toggle>().isOn);

        int j = 0; //Second index marker

        //breeding and replacing the bottom third
        for (int i = Mathf.FloorToInt(isos.Length * 2 / 3); i < isos.Length; i++) {
            isos[i] = breedIsos(isos[j], isos[j + 1]);
            j += 2;
        }

        //shuffling the array for more fair competition
        for (int i = isos.Length - 1; i > 0; i--) {
            int r = Random.Range(0, i);
            float[] tmp = isos[i];
            isos[i] = isos[r];
            isos[r] = tmp;
        }

        gen++; //Incrementing the generation number
        org = 0; //Resetting the organism count
        genDisplay.GetComponent<Text>().text = "Generation: " + gen;
        nextGen = false;
    }

    //writes to the designated save file
    public void savePop() {
        saveName = sanitize(input2.GetComponent<InputField>().text);
        StreamWriter file = new StreamWriter("Saves/" + saveName + ".txt"); //The save file opened for writing

        //write the gen number
        file.WriteLine(gen);

        //write each of the isos on a separate line
        foreach (float[] iso in isos) {
            string line = ""; //Resetting the line

            //adding to the line
            for (int i = 0; i < iso.Length - 1; i++) {
                if (i + 1 != iso.Length - 1) {
                    line += iso[i].ToString() + ",";
                }
                else {
                    line += iso[i].ToString();
                }
            }

            //writing the line
            file.WriteLine(line);
        }

        //closing before the function finishes
        file.Close();
    }

    // Use this for initialization
    void Start() {
        //fills out isos with declared arrays
        for (int i = 0; i < isos.Length; i++) {
            isos[i] = new float[393];
        }

        //check if save directory doesn't exist
        if (!Directory.Exists("Saves")) {
            //if it doesn't, create it
            Directory.CreateDirectory("Saves");

        }
    }

    // Update is called once per frame
    void Update() {
        //gives AI control the next organism
        if (nextIso && !nextGen) {
            cycle.GetComponent<AIControl>().weights = isos[org];
            org++;
            orgDisplay.GetComponent<Text>().text = "Iso: " + org;
            nextIso = false;
            ready = true;
        }

        //stuff to run when the current simulation finishes
        if (simComplete && loaded) {
            nextIso = true;

            if (org == isos.Length) {
                nextGen = true;
            }
            ready = false;
            simComplete = false;
        }

        if (nextGen) {
            newGen();
        }

    }
}
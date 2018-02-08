using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fileReadTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        String line;
        try {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader("MyFile.txt");

            //Read the first line of text
            line = sr.ReadLine();

            //Continue to read until you reach end of file
            while (line != null) {
                //write the lie to console window
                Debug.Log(line);
                //Read the next line
                line = sr.ReadLine();
            }

            //close the file
            sr.Close();
        }
        catch (Exception e) {
            Debug.Log("Exception: " + e.Message);
        }
        finally {
            Debug.Log("Executing finally block.");
        }
    }
        
	
	// Update is called once per frame
	void Update () {
		
	}
}

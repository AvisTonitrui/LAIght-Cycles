using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class fileWriteTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        try {

            //Pass the filepath and filename to the StreamWriter Constructor
            StreamWriter sw = new StreamWriter("Saves/MyFile.txt");

            //Write a line of text
            sw.WriteLine("Hello World!!");

            //Write a second line of text
            sw.WriteLine("From the StreamWriter class");

            //Close the file
            sw.Close();
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

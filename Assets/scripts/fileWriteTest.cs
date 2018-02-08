using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fileWriteTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        const string FILE_NAME = "MyFile.txt";

        if (File.Exists(FILE_NAME)){
            Debug.Log("File already exists.");
            return;
        }
        StreamWriter sr = File.CreateText(FILE_NAME);
        sr.WriteLine("This is my file.");
        sr.WriteLine("I can write ints {0} or floats {1}, and so on.", 1, 4.2);
        sr.Close();
    }
        
	
	// Update is called once per frame
	void Update () {
		
	}
}

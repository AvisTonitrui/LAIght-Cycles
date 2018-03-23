using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuing : MonoBehaviour {

    public void stopGame() {
        Application.Quit();
    }

    public void loadSim(string inp) {
        string[] inputs = inp.Split(',');
        globals.human1 = inputs[0] == "true";
        globals.human2 = inputs[1] == "true";
        globals.train = inputs[2] == "true";
        SceneManager.LoadScene("the grid");
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

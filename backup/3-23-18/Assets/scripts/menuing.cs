using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuing : MonoBehaviour {

    public GameObject victorBox;

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

    public void retry() {
        SceneManager.LoadScene("the grid");
    }

    public void toMain() {
        SceneManager.LoadScene("Main Menu");
    }

	// Use this for initialization
	void Start () {
		if (victorBox != null) {
            victorBox.GetComponent<Text>().text = globals.victoryMessage;
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

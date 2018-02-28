using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour { //This class takes the weights from an organism and uses it for controlling the cycle

    public int[] weights = new int[392]; //The wieghts specified by the organism
    public int[] inputs = new int[24]; //The inputs, which change at each gridpoint
    public int[] hiddens = new int[14]; //The hidden neurons
    public int[] outputs = new int[4]; //The output neurons
    public GameObject cycle;

    public void getIn() {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridStart : MonoBehaviour {

    public GameObject trail;
    private const int gridSize = 44;


	// Use this for initialization
	void Start () {
		for (int x = -gridSize; x <= gridSize; x++) {
            for (int y = -gridSize; y <= gridSize; y++) {
                GameObject thisTrail = Instantiate(trail, new Vector3(x, y, 1), Quaternion.identity);
                thisTrail.SetActive(false);
            }

        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class gridStart : MonoBehaviour {

    public GameObject trail;
    public const int gridSize = 45;
    public List<GameObject> map = new List<GameObject>();
    Transform listMap;
    public bool[,] trailMap = new bool[(gridSize * 2 + 1), (gridSize * 2) + 1];

    //reset for the next simulation
    public void restart() {
        //reset the array
        for (int x = 0; x < (gridSize * 2) + 1; x++) {
            for (int y = 0; y < (gridSize * 2) + 1; y++) {
                trailMap[x, y] = false;
            }
        }

        //reset the trails
        foreach (GameObject trail in map) {
            if (trail.transform.position != new Vector3(-20f, 0f, 0f) && trail.transform.position != new Vector3(20f, 0f, 0f)) {
                trail.GetComponent<Renderer>().enabled = false;
            } else if (Mathf.FloorToInt(trail.transform.position.y + 0.5f) == gridSize || Mathf.FloorToInt(trail.transform.position.x + 0.5f) == gridSize) {
                trailMap[Mathf.FloorToInt(trail.transform.position.x + 0.5f) + gridSize, Mathf.FloorToInt(trail.transform.position.y + 0.5f) + gridSize] = true;
            }
            else {
                //Debug.Log("Rendering");
                trail.GetComponent<Renderer>().enabled = true;
            }
        }

        //setting the initial spots to be activated
        trailMap[25, 45] = true;
        trailMap[65, 45] = true;
    }

    // Use this for initialization
    void Start() {
        listMap = GameObject.Find("Map").transform;
        for (int x = -gridSize; x <= gridSize; x++) {
            for (int y = -gridSize; y <= gridSize; y++) {
                map.Add(Instantiate(trail, new Vector3(x, y, 0), Quaternion.identity, listMap));

                if (trail.transform.position != new Vector3(-20f, 0f, 0f) && trail.transform.position != new Vector3(20f, 0f, 0f)) {
                    trail.GetComponent<Renderer>().enabled = false;
                }
                else {
                    //Debug.Log("Rendering");
                    trail.GetComponent<Renderer>().enabled = true;
                }
            }

        }

        for (int x = 0; x < (gridSize * 2) + 1; x++) {
            for (int y = 0; y < (gridSize * 2) + 1; y++) {
                trailMap[x, y] = false;
            }
        }

        //setting the initial spots to be activated
        trailMap[25, 45] = true;
        trailMap[65, 45] = true;
    }

    // Update is called once per frame
    void Update() {

    }
}

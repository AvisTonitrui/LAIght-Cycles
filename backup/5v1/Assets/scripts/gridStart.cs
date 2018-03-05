﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class gridStart : MonoBehaviour {

    public GameObject trail;
    public const int gridSize = 44;
    public List<GameObject> map= new List<GameObject>();
    Transform listMap;
    public bool[,] trailMap = new bool[(gridSize * 2 + 1), (gridSize * 2) + 1];

    // Use this for initialization
    void Start() {
        listMap = GameObject.Find("Map").transform;
        for (int x = -gridSize; x <= gridSize; x++) {
            for (int y = -gridSize; y <= gridSize; y++) {
                map.Add(Instantiate(trail, new Vector3(x, y, 1), Quaternion.identity, listMap));
                map.ElementAt(map.Count - 1).GetComponent<Renderer>().enabled = false;
            }

        }

        for (int x = 0; x < (gridSize * 2) + 1; x++) {
            for (int y = 0; y < (gridSize * 2) + 1; y++) {
                trailMap[x, y] = false;
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
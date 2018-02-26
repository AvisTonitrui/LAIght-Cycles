using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class gridStart : MonoBehaviour {

    public GameObject trail;
    private const int gridSize = 44;
    public List<GameObject> map= new List<GameObject>();
    Transform listMap;

    // Use this for initialization
    void Start() {
        listMap = GameObject.Find("Map").transform;
        for (int x = -gridSize; x <= gridSize; x++) {
            for (int y = -gridSize; y <= gridSize; y++) {
                map.Add(Instantiate(trail, new Vector3(x, y, 1), Quaternion.identity, listMap));
                map.ElementAt(map.Count - 1).GetComponent<Renderer>().enabled = false;
            }

        }
    }

    // Update is called once per frame
    void Update() {

    }
}

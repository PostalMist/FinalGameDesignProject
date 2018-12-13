using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follower : MonoBehaviour {
    public GameObject POI;
    public Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - POI.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 POIPos = POI.transform.position;
        transform.position = POIPos + offset;
	}
}

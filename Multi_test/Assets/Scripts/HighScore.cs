using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {
    public static float score = 0;

    [Header("Dynamically Set")]
    public Text gt; 
	// Use this for initialization
	void Start () {
		gt = GetComponent<Text>(); ;
    }
	
	// Update is called once per frame
	void Update () {
       
        gt.text = "High Score: " + score;
    }
}

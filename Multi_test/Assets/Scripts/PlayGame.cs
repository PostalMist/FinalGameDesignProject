using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour {

	// Use this for initialization
	public void StartGame () {
        SceneManager.LoadScene("singlePlayer");

    }
	
	
}

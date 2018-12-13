using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    public GameObject p;
    public override void OnStartClient()
    {
        GameObject plane = (GameObject)Instantiate(p, new Vector3(0, -1f, 0), Quaternion.identity);
        NetworkServer.Spawn(plane);
        //rb = GetComponent<Rigidbody>();
        
    }
}

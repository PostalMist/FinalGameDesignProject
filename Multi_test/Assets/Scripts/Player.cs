using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    public GameObject p;
    public float speed = 10.0f;
    private Rigidbody rb;
    public float jumpForce = 5.0f;
   // public GameObject holderL;
   // public GameObject holderR;
    // Use this for initialization
    /*public override void OnStartServer () {
        GameObject plane = (GameObject) Instantiate(p, new Vector3(0,-0.5f,0), Quaternion.identity);
        NetworkServer.Spawn(plane);
	}*/
    /*void Start() {
        rb = GetComponent<Rigidbody>();
        holderL = transform.Find("HolderL").gameObject;
        holderR = transform.Find("HolderR").gameObject;
    }*/
     void Start()
    {
       // GameObject plane = (GameObject)Instantiate(p, new Vector3(0,-1f,0), Quaternion.identity);
       // NetworkServer.Spawn(plane);
        rb = GetComponent<Rigidbody>();
       /* holderL = transform.Find("HolderL").gameObject;
        holderR = transform.Find("HolderR").gameObject;*/
        // CmdSpawn();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocalPlayer == true)
        {
            //Vector3 oldPositionL = holderL.transform.position;
             
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
           
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            this.gameObject.transform.root.transform.position = this.transform.position;
           rb.AddForce(movement * speed);
           // holderL.transform.position = oldPositionL;
            //holderR.transform.position = oldPositionR;

            if ( Input.GetKeyDown(KeyCode.Space))
            {

                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                this.gameObject.transform.root.transform.position = this.transform.position;
            }
        }

    }
    /*void Update () {
        if (isLocalPlayer == true) {
            if (Input.GetKey(KeyCode.D))
            {
                this.transform.Translate(Vector3.right * Time.deltaTime * 3f);
            }
            else if (Input.GetKey(KeyCode.A)) {

                this.transform.Translate(Vector3.left * Time.deltaTime * 3f);
            }
        }
	}*/

    /* [Command]
     public void CmdSpawn() {
         GameObject plane = (GameObject)Instantiate(p, new Vector3(0, -0.5f, 0), Quaternion.identity);
         NetworkServer.SpawnWithClientAuthority(plane,connectionToClient);
     }*/
}

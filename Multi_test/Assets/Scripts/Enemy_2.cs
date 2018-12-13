using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {
idle,
CanShoot
}

public class Enemy_2 : Enemy {
    [Header("Set In Inspector")]
    public Weapon wpn;
    public float delayBetweenShots = 2.0f;
    [Header("Dynamically Set")]
    public Vector3 _target; //target position
    public delegate void WeaponFireDelegate(); 
    public WeaponFireDelegate fireDelegate;
    private SphereCollider sphColl;
    private float lastShotTime;
    private State currState;
    public int MainPositionIndex;
    // Use this for initialization
    void Start () {
        wpn.SetType(WeaponType.blaster);
        sphColl = GetComponent<SphereCollider>();
        currState = State.idle;
        
	}
    public Vector3 target
    {
        get { return (_target); }
        set { _target = value; }
    }


    public override void Move() {
        //Doesn't move but shoots at enemy

        Vector3 targetDirection = target - transform.position;

        if (targetDirection.magnitude <= sphColl.radius)
        {
            //fire at player if within the sphere collider
            if (Time.time - lastShotTime >= delayBetweenShots) {
                if (fireDelegate != null)
                {
                    if (currState == State.CanShoot) {
                    
                    fireDelegate();
                    lastShotTime = Time.time;
                }
                }
            }

        }

        Alive(); // check if it is still in its position
	}



    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Transform rootT = other.gameObject.transform.root;
            target = rootT.position;
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        if (rootT.gameObject.tag == "Player")
        {
            
            target = rootT.position;
            currState = State.CanShoot;
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        if (rootT.gameObject.tag == "Player")
        {
            /*Transform rootT = other.gameObject.transform.root;
            target = rootT.position;*/
            currState = State.idle;
        }
    }

    private void Alive() {
        Vector3 offset = this.transform.position - Main.S.spawnPoint[MainPositionIndex];

        if (offset.magnitude > 10.0f) {
            currState = State.idle;
            Main.S.spawnPositions[MainPositionIndex] = Vector3.zero; // alert main of availabilty in position
            Main.S.ShipDestroyed(this);
            Destroy(this.gameObject);

        }

    }

   /* public Vector3 getTarget() {

        return target;
    }*/
}

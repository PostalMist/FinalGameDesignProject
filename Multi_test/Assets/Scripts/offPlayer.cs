using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class offPlayer : MonoBehaviour {
     [Header("Set in Inspector")]
    public float speed = 10.0f;
    public float turnSpeed = 5.0f;
    public float shieldLevel = 50f;
    public float health = 100f;
    public float gameRestartDelay = 2f;
    public Image healthBar;
    public Image shieldBar;
    public Weapon[] weapons; // a
    

    [Header("Set Dynamically")]
    // This variable holds a reference to the last triggering GameObject
    private GameObject lastTriggerGo = null; // a
    // Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate(); // a
    // Create a WeaponFireDelegate field named fireDelegate.
    public WeaponFireDelegate fireDelegate;

    public delegate void SecondaryWeaponFireDelegate();
    public WeaponFireDelegate SecondaryFireDelegate;
    public float jumpForce = 5.0f;
   
   

    void Start()
    {

        // rb = GetComponent<Rigidbody>();
        ClearWeapons();
        //weapons[6].SetType(WeaponType.cannon);
        // weapons[7].SetType(WeaponType.cannon);
        weapons[0].SetType(WeaponType.blaster);
        weapons[1].SetType(WeaponType.blaster);
        Cursor.lockState = CursorLockMode.Locked;
        healthBar.fillAmount = health / 100;
        shieldBar.fillAmount = shieldLevel / 100;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        
           

            float horizontalLook = Input.GetAxis("Mouse X");
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
             

        transform.Rotate(Vector3.up, horizontalLook * turnSpeed * Time.deltaTime);

        if (moveVertical != 0) {
            this.transform.Translate(0.0f, 0.0f, speed * moveVertical);
            

        } else if (moveHorizontal != 0) {
            this.transform.Translate(speed * moveHorizontal,0.0f,0.0f);
            

        }

        if (Input.GetKeyDown(KeyCode.Space) && fireDelegate != null)
            {
           
            fireDelegate();
        }

        if (Input.GetKeyDown(KeyCode.B) && SecondaryFireDelegate != null) {

            SecondaryFireDelegate();
        }

        if (Input.GetKeyDown("escape")) {
            Cursor.lockState = CursorLockMode.None;

        }

        

    }

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //print("Triggered: " + go.name);
        // Make sure it's not the same triggering go as last time
        if (go == lastTriggerGo)
        { // c
            return;
        }
        lastTriggerGo = go; // d
        if (go.tag == "Enemy" && go.name != "Shooter(Clone)")
        { // If the shield was triggered by an enemy
            TakeDamage(); // Decrease the level of the shield by 1
            //print(shieldLevel);
            Destroy(go); // … and Destroy the enemy // e
        }
        else if (go.tag == "PowerUp")
        {
            // If the shield was triggered by a PowerUp
            AbsorbPowerUp(go);
        }
        else if (go.tag == "Enemy" && go.name == "Shooter(Clone)")
        {
            Enemy_2 enemyShooter = go.GetComponent<Enemy_2>();
            enemyShooter.target = this.transform.position;
          // print(enemyShooter.target);
            


        }else
        {
            print("Triggered by non-Enemy: " + go.name); // f
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform otherRoot = collision.gameObject.transform.root;
        GameObject otherRootGo = otherRoot.gameObject;

        if (collision.gameObject.tag == "ProjectileEnemy")
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
        else if (otherRootGo.tag == "Enemy")
        {
            TakeDamage();
            Destroy(otherRootGo);

        }
        else if (otherRootGo.tag == "PickUp")
        {
            Main.S.score += 5f;
            Main.S.UpdateScore();

            Destroy(otherRootGo);

        }

    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.health:
                if(health < 100) {
                    health += 25f;
                }
                if (health >= 100) {
                    health = 100f;
                }
                healthBar.fillAmount = health / 100;
                break;
            case WeaponType.shield: // a
                if (shieldLevel < 100) {
                    shieldLevel += 25f;
                }

                if (shieldLevel >= 100) {
                    shieldLevel = 100f;
                }
               
                shieldBar.fillAmount = shieldLevel / 100;
                break;
            case WeaponType.cannon:
                if (pu.type == weapons[6].type)
                { // If it is the same type // c
                    Weapon w = GetEmptySecondaryWeaponSlot();
                    if (w != null)
                    {
                        // Set it to pu.type
                        w.SetType(pu.type);
                    }
                }
                else
                { // If this is a different weapon type // d
                    ClearSecondaryWeapons();
                    weapons[6].SetType(pu.type);
                }
                break;
            default: // b
                if (pu.type == weapons[0].type)
                { // If it is the same type // c
                    Weapon w = GetEmptyPrimaryWeaponSlot();
                    if (w != null)
                    {
                        // Set it to pu.type
                        w.SetType(pu.type);
                    }
                }
                else
                { // If this is a different weapon type // d
                    ClearPrimaryWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }

    Weapon GetEmptyPrimaryWeaponSlot()
    {
        for (int i = 0; i < (weapons.Length - 2); i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }

    Weapon GetEmptySecondaryWeaponSlot()
    {
        for (int i = (weapons.Length - 2); i < weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }
    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }

    void ClearPrimaryWeapons()
    {
        for (int i = 0; i < (weapons.Length - 2); i++) {
            weapons[i].SetType(WeaponType.none);
        }
    }

    void ClearSecondaryWeapons()
    {
        for (int i = (weapons.Length - 2); i < weapons.Length; i++)
        {
            weapons[i].SetType(WeaponType.none);
        }
    }

    private void TakeDamage() {
        if (shieldLevel > 0)
        {
            //damage goes to shield
            shieldLevel -= 20f; // all enemy damage will be -20

            if (shieldLevel < 0)
            {
                shieldLevel = 0f;
            }

            shieldBar.fillAmount = shieldLevel/100;

        }
        else {
            health -= 20f;

            if (health <= 0) {
                Destroy(this.gameObject);
                // Tell Main.S to restart the game after a delay
                Main.S.DelayedRestart(gameRestartDelay); // a
            }

            healthBar.fillAmount = health / 100;

        }


    }
}

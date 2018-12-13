using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is an enum of the various possible weapon types.
/// It also includes a "shield" type to allow a shield power-up.
/// Items marked [NI] below are Not Implemented in the IGDPD book.
/// </summary>
public enum WeaponType
{
    none, // The default / no weapon
    blaster, // A simple blaster
    spread, // Two shots simultaneously
    cannon, // bombs
    missile, // [NI] Homing missiles
    laser, // [NI]Damage over time
    shield, // Raise shieldLevel
    health
}
[System.Serializable] // a
public class WeaponDefinition
{ // b
    public WeaponType type = WeaponType.none;
    public string letter; // Letter to show on the power-up
    public Color color = Color.white; // Color of Collar & power-up
    public GameObject projectilePrefab; // Prefab for projectiles
    public Color projectileColor = Color.white;
    public float damageOnHit = 0; // Amount of damage caused
    public float continuousDamage = 0; // Damage per second (Laser)
    public float delayBetweenShots = 0;
    public float velocity = 20; // Speed of projectiles
}

public class Weapon : MonoBehaviour
{

    static public Transform PROJECTILE_ANCHOR;
    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public GameObject front;
    public GameObject back;
    public GameObject firePoint;
    public float lastShotTime; // Time last shot was fired
    public float lastShotTimeSec;
    private Renderer collarRend;
    private Renderer frontRend;
    private Renderer backRend;
    public Vector3 target = Vector3.zero;
    float pokeForce = 10f;
    void Start()
    {
        try {
            collar = transform.Find("Collar").gameObject;
        }
        catch (NullReferenceException e) { }
        if (collar == null) {
            //it is the cannons
            front = transform.Find("Front").gameObject;
            back = transform.Find("Back").gameObject;
            firePoint = transform.Find("FirePoint").gameObject;

        }
        if (collar != null) {
            collarRend = collar.GetComponent<Renderer>();
        }

        if (front != null) {
            frontRend = front.GetComponent<Renderer>();
            backRend = back.GetComponent<Renderer>();

        }
        // Call SetType() for the default _type of WeaponType.none
        SetType(_type); // a
                        // Dynamically create an anchor for all Projectiles
        if (PROJECTILE_ANCHOR == null)
        { // b
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        // Find the fireDelegate of the root GameObject
        GameObject rootGO = transform.root.gameObject; // c
        if (rootGO.GetComponent<offPlayer>() != null)
        { // d
            rootGO.GetComponent<offPlayer>().fireDelegate += Fire;
            rootGO.GetComponent<offPlayer>().SecondaryFireDelegate += SecondaryFire;
        } else if (rootGO.GetComponent<Enemy_2>() != null) {
            rootGO.GetComponent<Enemy_2>().fireDelegate += Fire;

        }
    }
    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }
    public void SetType(WeaponType wt)
    {
        _type = wt;
        if (type == WeaponType.none)
        { // e
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type); // f
        if (collar != null) {
            collarRend.material.color = def.color;
        } else if (front != null) {
            frontRend.material.color = def.color;
            backRend.material.color = def.color;

        }

        lastShotTime = 0; // You can fire immediately after _type is set. // g
    }
    public void Fire()
    {
        // If this.gameObject is inactive, return
        if (!gameObject.activeInHierarchy) return; // h
                                                   // If it hasn't been enough time between shots, return
        if (Time.time - lastShotTime < def.delayBetweenShots)
        { // i
            return;
        }
        if (transform.root.gameObject.tag == "Player") {
            Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
            //  Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,3f,0f));

            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 100f))
            {
                target = hitInfo.point;
                if (hitInfo.rigidbody != null)
                {
                   // hitInfo.rigidbody.AddForceAtPosition(ray.direction * pokeForce, hitInfo.point);
                }
            }
        } else if (transform.root.gameObject.tag == "Enemy") {
            Enemy_2 enemyShooter = transform.root.gameObject.GetComponent<Enemy_2>();
            target = enemyShooter.target;
            //print(target);
        }
        Projectile p;
        Vector3 vel = Vector3.zero;
        Debug.Log(target);
        if (collar != null) { 
         vel = (target - collar.transform.position) * def.velocity; // j
      }
       /* if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }*/
        switch (type)
        { // k
            case WeaponType.blaster:
                p = MakeProjectile();
                 p.rigid.velocity = vel;
               // p.rigid.MovePosition(new Vector3(-20,0,30) * def.velocity *200f);
                break;
            case WeaponType.spread: // l
                p = MakeProjectile(); // Make middle Projectile
                p.rigid.velocity = vel;
                p = MakeProjectile(); // Make right Projectile
                p.transform.rotation = Quaternion.AngleAxis(10, collar.transform.forward);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile(); // Make left Projectile
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.forward);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
        }
    }

    public void SecondaryFire()
    {
        // If this.gameObject is inactive, return
        if (!gameObject.activeInHierarchy) return; // h
                                                   // If it hasn't been enough time between shots, return
        if (Time.time - lastShotTime < def.delayBetweenShots)
        { // i
            return;
        }
        
        Projectile p;
       
        switch (type)
        { // k
            case WeaponType.cannon:
                p = MakeSecProjectile();
                p.rigid.isKinematic = false;
                p.rigid.velocity = -(back.transform.position - front.transform.position) * def.velocity;
               
                // p.rigid.MovePosition(new Vector3(-20,0,30) * def.velocity *200f);
                break;
            
            default:
                break;
        }
    }
    public Projectile MakeProjectile()
    { // m
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.root.gameObject.tag == "Player")
        { // n
            go.tag = "ProjectilePlayer";
            go.layer = LayerMask.NameToLayer("ProjectilePlayer");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true); // o
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time; // p
        return (p);
    }

    public Projectile MakeSecProjectile()
    { // m

         GameObject projectile = Instantiate(def.projectilePrefab) as GameObject;

        projectile.tag = "ProjectilePlayer";
        projectile.layer = LayerMask.NameToLayer("ProjectilePlayer");

        projectile.transform.position = firePoint.transform.position;
       /* Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>(); // a
        projectileRigidbody.isKinematic = true;*/
        projectile.transform.SetParent(PROJECTILE_ANCHOR, true); // o
        Projectile p = projectile.GetComponent<Projectile>();
        p.type = type;
        //lastShotTimeSec = Time.time; // p
        lastShotTime = Time.time;
        return (p);
    }
}

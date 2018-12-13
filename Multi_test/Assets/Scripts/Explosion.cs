using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Set In Inspector")]
    public float power = 30.0f;
    public float radius = 10.0f;
    public float upForce = 5.0f;
    public float explosionDuration = 5.0f;
    public GameObject explosionPrefab;
    public AudioSource explosionSound;
    [Header("Dynamically Set")]
    public GameObject existingGO;
    

    void Start()
    {
        // explosiveBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    /*void FixedUpdate () {
        if (explosiveBody.IsSleeping()) {
            Detonate();
           Destroy(this);
            fire = Instantiate(SmallExplosion);
            fire.transform.

        }
	}*/
    void OnCollisionEnter(Collision collision)
    {
        // Invoke("Detonate",1.0f);
        Detonate();
    }

    public void Detonate()
    {
        Vector3 explosionPosition = this.transform.position;
        //Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

        //fire = Instantiate(SmallExplosion);
        existingGO  = Instantiate(explosionPrefab) as GameObject;
        existingGO.transform.position = this.transform.position;
        Invoke("playExplosion", 100f);
        Invoke("DestroyExplosion", explosionDuration);
         //explosion.transform.position = transform.position;
         //explosion.Play(true);
         //Destroy(fire,3.0f);
        //print(colliders);
       /*foreach (Collider hit in colliders)
        {
            Transform root = hit.transform.root;
            //check to mkae sure that powerUps don't get harmed
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upForce, ForceMode.Impulse);
                if (this.gameObject != null)
                {
                    Destroy(this.gameObject);
                }
                // Destroy(fire);

            }
        }*/
       // Destroy(go);
        //explosionSound.Play();
        //fire.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    void playExplosion() {
        explosionSound.Play();
    }

    private void DestroyExplosion()
    {
        Destroy(existingGO);
    }
}

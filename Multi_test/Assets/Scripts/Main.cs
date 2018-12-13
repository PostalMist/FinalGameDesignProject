using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

    static public Main S; // A singleton for Main
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies; // Array of Enemy prefabs
    public float enemySpawnPerSecond = 0.5f; // # Enemies/second
    public float enemyDefaultPadding = 1.5f; // Padding for position
    public float delay = 0.1f;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp; // a
    public GameObject pickUpPrefab;
    public Text scoreText;
    public WeaponType[] powerUpFrequency = new WeaponType[] { // b
WeaponType.blaster, WeaponType.blaster,
WeaponType.spread, WeaponType.shield,
        WeaponType.cannon, WeaponType.cannon,
        WeaponType.health,};
    [Header("Dynamically Set")]
    public List<Vector3> spawnPositions;
    public Dictionary<int,Vector3> spawnPoint;
    public float score;

    // private BoundsCheck bndCheck;

    public void ShipDestroyed(Enemy e)
    { // c
      // Potentially generate a PowerUp
        if (Random.value <= e.powerUpDropChance)
        { // d
          // Choose which PowerUp to pick
          // Pick one from the possibilities in powerUpFrequency
            int ndx = Random.Range(0, powerUpFrequency.Length); // e
            WeaponType puType = powerUpFrequency[ndx];
            // Spawn a PowerUp
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            // Set it to the proper WeaponType
            pu.SetType(puType); // f
                                // Set it to the position of the destroyed ship
                                // pu.transform.position = e.transform.position;
            if (e.transform.gameObject.name == "Shooter(Clone)")
            {

                Enemy_2 e2 = e.transform.root.gameObject.GetComponent<Enemy_2>();
                spawnPositions[e2.MainPositionIndex] = Vector3.zero;
                pu.transform.position = new Vector3(0f, 1.5f, 0f);
            }
            else { 
            pu.transform.position = new Vector3(e.transform.position.x, 1.5f, e.transform.position.z);
            }
        }
        if (e.transform.gameObject.name == "Shooter(Clone)")
        {
            score += 20f;
        }
        else {

            score += 10f;
        }

        UpdateScore();
    }

    void Awake()
    {
        S = this;
        // Set bndCheck to reference the BoundsCheck component on this GameObject
       // bndCheck = GetComponent<BoundsCheck>();
        // Invoke SpawnEnemy() once (in 2 seconds, based on default values)
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond); // a

        Invoke("SpawnPickUp", (1f / enemySpawnPerSecond) + delay);
        // A generic Dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>(); // a
        foreach (WeaponDefinition def in weaponDefinitions)
        { // b
            WEAP_DICT[def.type] = def;
        }

        spawnPoint = new Dictionary<int, Vector3>(){
            { 0, new Vector3(-55.63f, 0.56f, 58.55f)},
            {1, new Vector3(55.86f, 0.56f, 58.55f) },
            {2, new Vector3(-55.89f,0.56f, -56.79f) },
            {3, new Vector3(55.46f, 0.56f, -56.79f) }


        };

        score = 0;
    }
    public void SpawnEnemy()
    {
        // Pick a random Enemy prefab to instantiate
        int ndx = Random.Range(0, prefabEnemies.Length); // b
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]); // c
                                                                     // Position the Enemy above the screen with a random x position
        float enemyPadding = enemyDefaultPadding; // d
          
        Vector3 pos = Vector3.zero;
        float xMin = -60f;
        float xMax = 60f;
        float zMin = -60f;
        float zMax = 60f;
        pos.x = Random.Range(xMin, xMax);
        pos.y = 0.5f;
        if (go.name == "Enemy(Clone)") {
            pos.z = zMax; //North of the wall
        } else if (go.name == "Shooter(Clone)") {
            Enemy_2 e2Go = go.GetComponent<Enemy_2>();


            switch (spawnPositions.Count) {
                case 0:
                    pos = spawnPoint[0];
                    spawnPositions.Add(spawnPoint[0]);// North West corner
                    e2Go.MainPositionIndex = 0;
                    break;
                case 1:
                    pos = spawnPoint[1];
                    spawnPositions.Add(spawnPoint[1]); // North East corner
                    e2Go.MainPositionIndex = 1;
                    break;
                case 2:
                    pos = spawnPoint[2];
                    spawnPositions.Add(spawnPoint[2]); // South East corner
                    e2Go.MainPositionIndex = 2;
                    break;
                case 3:
                    pos = spawnPoint[3];
                    spawnPositions.Add(spawnPoint[3]); // South West corner
                    e2Go.MainPositionIndex = 3;
                    break;
                default:
                    for (int i = 0; i < spawnPositions.Count; i++) {
                        if (spawnPositions[i] == Vector3.zero) {
                            //there is a vacancy
                            pos = spawnPoint[i];
                            spawnPositions[i] = spawnPoint[i];
                            e2Go.MainPositionIndex = i;
                        }
                        if(i == spawnPositions.Count - 1){

                            go.SetActive(false);
                        }

                    }
                   

                    break;

            }

        }
        if (go.activeInHierarchy) { 
        go.transform.position = pos;
        }else{
            Destroy(go);
        
        }

        // Invoke SpawnEnemy() again
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond); // g
    }

    public void DelayedRestart(float delay)
    {
        // Invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        // Reload _Scene_0 to restart the game
        score = 0;
        SceneManager.LoadScene("singlePlayer");
        
    }

    /// <summary>
    /// Static function that gets a WeaponDefinition from the WEAP_DICT

    ///static protected field of the Main class.
    /// </summary>
    /// <returns>The WeaponDefinition or, if there is no WeaponDefinition

    ///with the WeaponType passed in, returns a new WeaponDefinition with a
    /// WeaponType of none..</returns>
    /// <param name="wt">The WeaponType of the desired
    /// WeaponDefinition</param>
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    { //a

        // Check to make sure that the key exists in the Dictionary
        // Attempting to retrieve a key that didn't exist, would throw an error,
        // so the following if statement is important.
        if (WEAP_DICT.ContainsKey(wt))
        { // b
            return (WEAP_DICT[wt]);
        }
        // This returns a new WeaponDefinition with a type of WeaponType.none,
        // which means it has failed to find the right WeaponDefinition
        return (new WeaponDefinition()); // c
    }

    public void UpdateScore() {
        scoreText.text = "Score: " + score;

        if (score > HighScore.score) {
            HighScore.score = score;
        }
    }

    void SpawnPickUp() {
        GameObject go = Instantiate<GameObject>(pickUpPrefab); 
        

        Vector3 pos = Vector3.zero;
        float xMin = -60f;
        float xMax = 60f;
        float zMin = -60f;
        float zMax = 60f;
        pos.x = Random.Range(xMin, xMax);
        pos.y = 1f;
        pos.z = Random.Range(zMin, zMax);
        
        go.transform.position = pos;
       

        // Invoke SpawnEnemy() again
        Invoke("SpawnPickUp", (1f / enemySpawnPerSecond )+  delay); // g


    }
}

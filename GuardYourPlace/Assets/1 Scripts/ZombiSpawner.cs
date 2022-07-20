using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiSpawner : MonoBehaviour,IStartGameObserver,ILoseObserver
{
    public static ZombiSpawner Instance;
    public List<GameObject> enemyAll;
    [SerializeField] GameObject[] zombiePrefab;
    [SerializeField] float spawnPeriod;
    public bool spawnActive = true;
    int spawnPointSelect;
    int spawnPointCount;
    public GameObject player;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] public int[] zombieLevel;
    [SerializeField] public int[] spawnSpeed;
    public int level;

    public int[] pink;
    public int[] orange;
    public int[] red;
    public int[] purple;
    public int[] green;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        GameManager.Instance.Add_StartObserver(this);
        GameManager.Instance.Add_LoseObserver(this);
        Globals.currentBox = 0;
        spawnPointCount = transform.childCount;
    }
    public void LoseScenario()
    {
        for(int i = 0; i< enemyAll.Count; i++)
        {
            enemyAll[i].GetComponent<Collider>().enabled = false;
            enemyAll[i].GetComponent<Zombie>().currentBehaviour = Zombie.States.idle;
        }

        StartCoroutine(loseDelay());
    }
    IEnumerator loseDelay()
    {
        for (int i = 0; i < enemyAll.Count; i++)
        {
            enemyAll[i].GetComponent<Zombie>().currentBehaviour = Zombie.States.dead;
            //enemyAll.Remove(enemyAll[i]);

            yield return new WaitForSeconds(0.05f);
        }

    }
    public void StartGame()
    {
        StartCoroutine(spawning());

    }
    IEnumerator spawning()
    {
        yield return null;

        while (spawnActive)
        {
            if (Globals.currentBox < zombieLevel[zombieLevel.Length - 1])
            {
                if (Globals.currentBox >= zombieLevel[level])
                {
                    level++;
                    Debug.Log("LEVEL  " + level);
                    //upgradeManager.Instance.upgradeOpen();
                    //spawnActive = false;
                }
            }
            if (spawnActive && enemyAll.Count < 150)
            {
                StartCoroutine(enemySpawn());
            }
            yield return new WaitForSeconds(10 / (float)spawnSpeed[level]);
        }
    }
    IEnumerator enemySpawn()
    {
        for (int i = 0; i < pink[level]; i++)
        { 
            spawnPointSelect = Random.Range(0, spawnPoints.Length);

            if (player.GetComponent<PlayerControl>().players.Count > 0 && Globals.isGameActive)
            {
                var pinkEnemy = Instantiate(zombiePrefab[0], spawnPoints[spawnPointSelect].position, Quaternion.identity);
                pinkEnemy.GetComponent<Zombie>().player = player;
                //pinkEnemy.GetComponent<Zombie>().player = player.GetComponent<PlayerControl>().players[Random.Range(0, player.GetComponent<PlayerControl>().players.Count)];
                enemyAll.Add(pinkEnemy);
                //zombie.GetComponent<Zombie>().player = player;
                yield return new WaitForSeconds(1 / (float)spawnSpeed[level]);
            }
        }
        for (int i = 0; i < orange[level]; i++)
        {
            spawnPointSelect = Random.Range(0, spawnPoints.Length);

            if (player.GetComponent<PlayerControl>().players.Count > 0 && Globals.isGameActive)
            {
                var orangeEnemy = Instantiate(zombiePrefab[1], spawnPoints[spawnPointSelect].position, Quaternion.identity);
                orangeEnemy.GetComponent<Zombie>().player = player;
                //orangeEnemy.GetComponent<Zombie>().player = player.GetComponent<PlayerControl>().players[Random.Range(0, player.GetComponent<PlayerControl>().players.Count)];
                enemyAll.Add(orangeEnemy);
                yield return new WaitForSeconds(1 / (float)spawnSpeed[level]);
            }
        }
        for (int i = 0; i < red[level]; i++)
        {
            spawnPointSelect = Random.Range(0, spawnPoints.Length);

            if (player.GetComponent<PlayerControl>().players.Count > 0 && Globals.isGameActive)
            {
                var redEnemy = Instantiate(zombiePrefab[2], spawnPoints[spawnPointSelect].position, Quaternion.identity);
                redEnemy.GetComponent<Zombie>().player = player;
                //redEnemy.GetComponent<Zombie>().player = player.GetComponent<PlayerControl>().players[Random.Range(0, player.GetComponent<PlayerControl>().players.Count)];
                enemyAll.Add(redEnemy);
                yield return new WaitForSeconds(1 / (float)spawnSpeed[level]);
            }
        }
        for (int i = 0; i <  purple[level]; i++)
        {
            spawnPointSelect = Random.Range(0, spawnPoints.Length);

            if (player.GetComponent<PlayerControl>().players.Count > 0 && Globals.isGameActive)
            {
                var purpleEnemy = Instantiate(zombiePrefab[3], spawnPoints[spawnPointSelect].position, Quaternion.identity);
                purpleEnemy.GetComponent<Zombie>().player = player;
                //purpleEnemy.GetComponent<Zombie>().player = player.GetComponent<PlayerControl>().players[Random.Range(0, player.GetComponent<PlayerControl>().players.Count)];
                enemyAll.Add(purpleEnemy);
                yield return new WaitForSeconds(1 / (float)spawnSpeed[level]);
            }
        }
        for (int i = 0; i <  green[level]; i++)
        {
            spawnPointSelect = Random.Range(0, spawnPoints.Length);

            if (player.GetComponent<PlayerControl>().players.Count > 0 && Globals.isGameActive)
            {
                var greenEnemy = Instantiate(zombiePrefab[4], spawnPoints[spawnPointSelect].position, Quaternion.identity);
                greenEnemy.GetComponent<Zombie>().player = player;
                //greenEnemy.GetComponent<Zombie>().player = player.GetComponent<PlayerControl>().players[Random.Range(0, player.GetComponent<PlayerControl>().players.Count)];
                enemyAll.Add(greenEnemy);
                yield return new WaitForSeconds(1 / (float)spawnSpeed[level]);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }
}

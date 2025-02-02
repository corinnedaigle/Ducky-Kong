using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class randomSpawn : MonoBehaviour
{
    public GameObject[] enemies;

    public Vector3 spawnValues;
    public float spawnWait;
    public float spawnMostWait;
    public float spawnLeastWait;
    public int startWait;
    public bool stop;

    private int randomEnemy;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitSpawner());
    }


    // Update is called once per frame
    void Update()
    {
        spawnWait = Random.Range(spawnLeastWait, spawnMostWait);
    }

    IEnumerator waitSpawner()
    {
        yield return new WaitForSeconds(startWait);

        while (true)
        {
            randomEnemy = Random.Range(0, 1);
            Vector3 spawnPosition = new Vector3(1, 1, 0);
            Instantiate(enemies[randomEnemy], spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);
            yield return new WaitForSeconds(spawnWait);
        }
    }

}

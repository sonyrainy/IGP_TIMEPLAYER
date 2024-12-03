using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneManager : MonoBehaviour
{
    [SerializeField] GameObject seed;
    [SerializeField] GameObject[] seedSpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!FindObjectOfType<SeedObject>())
        {
            int index = Random.Range(0, seedSpawnPoints.Length);
            Instantiate(seed, seedSpawnPoints[index].transform.position, seedSpawnPoints[index].transform.rotation);
        }
    }
}

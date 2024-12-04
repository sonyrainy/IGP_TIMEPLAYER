using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneManager : MonoBehaviour
{
    private static BossSceneManager _instance;
    public static BossSceneManager Instance 
    { 
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(BossSceneManager)) as BossSceneManager;

                if (_instance == null)
                {
                    Debug.Log("No BossSceneManager object");
                }
            }
            return _instance;
        }
    }

    [SerializeField] GameObject seed;
    [SerializeField] GameObject[] seedSpawnPoints;
    [SerializeField] GameObject treeAttackSpawnPoint;
    [SerializeField] public GameObject playerTreeAttackOjbect;

    [SerializeField] float seedSpawnDelayTime = 1;

    public bool isSeedSpawned;

    // Start is called before the first frame update
    void Start()
    {
        DelaySpawnSeed();
        playerTreeAttackOjbect.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DelaySpawnSeed()
    {
        StartCoroutine(DelaySpawnSeedCoroutine());
    }

    IEnumerator DelaySpawnSeedCoroutine()
    {
        yield return new WaitForSeconds(seedSpawnDelayTime);
        SpawnSeed();
    }

    void SpawnSeed()
    {
        isSeedSpawned = true;
        int index = Random.Range(0, seedSpawnPoints.Length);
        Instantiate(seed, seedSpawnPoints[index].transform.position, seedSpawnPoints[index].transform.rotation);
    }

    public void SetActiveTrueTreeAttackObject()
    {
        playerTreeAttackOjbect.gameObject.SetActive(true);
    }

}

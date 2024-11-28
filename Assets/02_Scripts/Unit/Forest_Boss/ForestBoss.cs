using Forest_Boss_States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBoss : MonoBehaviour
{
    public ForestBossState forestBossState = ForestBossState.Idle;
    public ForestBossState prevForestBossState = ForestBossState.Idle;
    public Animator animator;

    public State<ForestBoss>[] states;

    [SerializeField] public Transform[] LogPositions = new Transform[9];
    [SerializeField] public Transform[] rockPositions = new Transform[4];

    [SerializeField] public GameObject logTelegraph;
    [SerializeField] public GameObject log;
    [SerializeField] public GameObject rockTelegraph;
    [SerializeField] public GameObject rock;

    [SerializeField] private int spawnLogNumber = 3;
    [SerializeField] private int spawnRockNumber = 2;
    [SerializeField] private int difficultPerPlayTime = 10;
    [SerializeField] private float logsSpawnTerm = 1f;
    [SerializeField] private float rocksSpawnTerm = 1f;

    [SerializeField] private float logsSpawnWaitTime = 1f;
    [SerializeField] private float rocksSpawnWaitTime = 1f;
    private float playTime = 0f;

    public void ChangeState(ForestBossState state) 
    {
        states[(int)forestBossState].Exit();
        prevForestBossState = forestBossState;
        forestBossState = state;
        states[(int)forestBossState].Enter();
    }

    public void ChangeAnimation(ForestBossAnimation newAnimation, float normalizedTime = 0)
    {
        animator.Play(newAnimation.ToString(), 0, normalizedTime);
    }

    public void ChangeAnimation(string animationName, float normalizedTime = 0)
    {
        animator.Play(animationName, 0, normalizedTime);
    }

    private void OnEnable()
    {
        InitFSM();
    }

    void InitFSM()
    {
        animator = GetComponentInChildren<Animator>();
        forestBossState = ForestBossState.Spawn;

        states = new State<ForestBoss>[(int)ForestBossState.Last];

        states[(int)ForestBossState.Spawn] = new FBossSpawn(this);
        states[(int)ForestBossState.Idle] = new FBossIdle(this);
        states[(int)ForestBossState.LogAttack] = new LogAttack(this);
        states[(int)ForestBossState.RockAttack] = new RockAttack(this);
        states[(int)ForestBossState.Hit] = new Hit(this);

        states[(int)forestBossState].Enter();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        states[(int)forestBossState].Execute();
        states[(int)forestBossState].OnTransition();

        playTime += Time.deltaTime;

        // if playTime is over than 3 and isStateChanging is false, start ChangeStateRoutine coroutine
        if (playTime > 3 && !isStateChanging)
        {
            StartCoroutine(ChangeStateRoutine());
        }
    }

    private bool isStateChanging = false;

    private IEnumerator ChangeStateRoutine()
    {
        isStateChanging = true;

        while (true)
        {
            // The wait time of LogAttack or RockAttack
            float nextStateTime = Random.Range(5f, 7f);
            yield return new WaitForSeconds(nextStateTime);

            // Change random states
            ForestBossState randomState = (Random.Range(0, 2) == 0) ? ForestBossState.LogAttack : ForestBossState.RockAttack;
            ChangeState(randomState);
        }
    }
    
    public void OnDamaged()
    {
        ChangeState(ForestBossState.Hit);
    }


    public void InstantiateLogs()
    {
        StartCoroutine(InstantiateLogsWithDelay());
    }

    IEnumerator InstantiateLogsWithDelay()
    {
        if (playTime > difficultPerPlayTime)
        {
            spawnLogNumber = 4;

            if (playTime > difficultPerPlayTime * 2)
                spawnLogNumber = 5;
        }

        List<int> randomNumbers = GetUniqueRandomNumbers(0, LogPositions.Length - 1, spawnLogNumber);
        float randomSpawnTime = Random.Range(logsSpawnTerm, logsSpawnTerm * 1.25f);

        // Instantiate LogTelegraphs
        List<GameObject> telegraphs = new List<GameObject>();
        foreach (int number in randomNumbers)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, -90);
            GameObject telegraphInstance = Instantiate(logTelegraph, LogPositions[number].position, rotation);
            telegraphs.Add(telegraphInstance);
            yield return new WaitForSeconds(randomSpawnTime);
        }

        // Destory LogTelegraphs
        foreach (GameObject telegraph in telegraphs)
        {
            Destroy(telegraph);
        }

        // Instantiate logs
        foreach (int number in randomNumbers)
        {
            Instantiate(log, LogPositions[number].position, Quaternion.identity);
            yield return new WaitForSeconds(randomSpawnTime);
        }
    }

    public void InstantiateRocks()
    {
        StartCoroutine(InstantiateRocksWithDelay());
    }

    IEnumerator InstantiateRocksWithDelay()
    {
        if (playTime > difficultPerPlayTime)
        {
            spawnRockNumber = 3;
        }

        List<int> randomNumbers = GetUniqueRandomNumbers(0, rockPositions.Length - 1, spawnRockNumber);
        float randomSpawnTime = Random.Range(rocksSpawnTerm, rocksSpawnTerm * 1.5f);

        // Instantiate RockTelegraphs
        List<GameObject> telegraphs = new List<GameObject>();
        foreach (int number in randomNumbers)
        {
            GameObject telegraphInstance = Instantiate(rockTelegraph, rockPositions[number].position, Quaternion.identity);
            telegraphs.Add(telegraphInstance);
            yield return new WaitForSeconds(randomSpawnTime);
        }

        // Destroy RockTelegraphs
        foreach (GameObject telegraph in telegraphs)
        {
            Destroy(telegraph);
        }

        // Instantiate Rocks
        foreach (int number in randomNumbers)
        {
            Vector3 rockSpawnPoint = new Vector3(rockPositions[number].position.x + 8, rockPositions[number].position.y, rockPositions[number].position.z);
            Instantiate(rock, rockSpawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(randomSpawnTime);
        }
    }

    // Random Number Function 
    List<int> GetUniqueRandomNumbers(int min, int max, int count)
    {
        List<int> numbers = new List<int>();

        for (int i = min; i <= max; i++)
        {
            numbers.Add(i);
        }

        List<int> result = new List<int>();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, numbers.Count);
            result.Add(numbers[randomIndex]);
            numbers.RemoveAt(randomIndex);
        }

        return result;
    }
}

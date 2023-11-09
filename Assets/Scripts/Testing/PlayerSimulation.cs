using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerSimulation : MonoBehaviour
{

    public static PlayerSimulation instance;
    
    public SimulationSettings SimulationSettings;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnGameStarted.AddListener(() =>
        {
            StartCoroutine(InitiateSimulation());
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Began)
            {
                SimulateRandomSpawning();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SimulateRandomSpawning();
        }
    }

    IEnumerator InitiateSimulation()
    {
        Random.InitState(Random.Range(0, SimulationSettings.randomizerSeedRange));
        
        yield return new WaitForSeconds(GameManager.instance.gameSettings.playerEmptyDelayBeforeStart);
    
        //Simulate totally random data received
        for (int i = 0; i < SimulationSettings.simulatedDataReceivengOnStart; i++)
        {
            SimulateRandomSpawning();
        }
        
    }
    
    //Function to simulate spawning using player's data
    public void SimulateRandomSpawning()//bool initiate = false, int playerInfoId = 0)
    {
        
        SimulationSettings.DummyPlayerInfo spawnedPlayerInfo;
        if (TryGetRandomPlayer(out spawnedPlayerInfo))
        {
            //Get Turret Shooting based on the team
            Debug.Log("Try Get Random Player "+ spawnedPlayerInfo.nickname, gameObject);
            ArenaManager.Instance.ProcessData(spawnedPlayerInfo);
        }
    }
    
    public bool TryGetRandomPlayer(out SimulationSettings.DummyPlayerInfo randomPlayerGet)
    {
        if (SimulationSettings.dummyPlayerDatabase.Count <= 0)
        {

            randomPlayerGet = new SimulationSettings.DummyPlayerInfo();   
            return false;
        }
        
        //Will have a chance to spawn totally random dummy instead of the one on the list
        if (Random.Range(0,2) > 0)
        {
            SimulationSettings.DummyPlayerInfo newRandomDummy = new SimulationSettings.DummyPlayerInfo(SimulationSettings.randomDummyPlayerPlaceHolder);
            int uniqueId = Random.Range(0, 10);
            newRandomDummy.nickname = newRandomDummy.nickname +""+ uniqueId.ToString();
            newRandomDummy.bullet = Random.Range(1, 20);
            newRandomDummy.teamId = Random.Range(1, 5);
            
            randomPlayerGet = newRandomDummy;
        }
        else
        {
             randomPlayerGet =
                SimulationSettings.dummyPlayerDatabase[Random.Range(0, SimulationSettings.dummyPlayerDatabase.Count)];
            
        }
        return true;
        
        
    }
    
   
    
    
}

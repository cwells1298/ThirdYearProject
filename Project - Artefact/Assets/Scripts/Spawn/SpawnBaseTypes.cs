using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBaseTypes : MonoBehaviour
{
    public GameObject[] baseActors;
    public GameObject[] modifiedActors;
    public bool usingBaseTypes = true;
    public Transform pawnHolder;
    public GameObject houseHolder;
    public WorkContainer workContainer;

    public int numPawnsToSpawn = 20;

    private InitPawn[] initPawns;
    private int[] pawnSpawnThreshold;
    private HomeZone[] homeZones;
    public int pawnsSpawned = 0;
    private GameObject[] usedSubset;

    public static int[] classNumbers = new int[14];

    private int totalChance = 0;

    void Start()
    {     
        if (usingBaseTypes)
        {
            usedSubset = baseActors;
        }
        else
        {
            usedSubset = modifiedActors;
        }
     
        initPawns = new InitPawn[usedSubset.Length];
        pawnSpawnThreshold = new int[usedSubset.Length];

        for (int i = 0; i < usedSubset.Length; i++)
        {
            initPawns[i] = usedSubset[i].GetComponent<InitPawn>();          
            totalChance += initPawns[i].chanceToSpawn;
            if (i == 0)
            {
                pawnSpawnThreshold[i] = initPawns[i].chanceToSpawn;
            }
            else
            {
                pawnSpawnThreshold[i] = pawnSpawnThreshold[i] + initPawns[i].chanceToSpawn;
            }         
        }

        int pawnsLeftToSpawn = numPawnsToSpawn;

        homeZones = houseHolder.GetComponentsInChildren<HomeZone>();
        numPawnsToSpawn = homeZones.Length;
        //Debug.Log("Pawn Count: " + numPawnsToSpawn);
        //while (pawnsLeftToSpawn > 0)
        //{
        //    if (WeightedPawnSpawn())
        //    {
        //        pawnsLeftToSpawn -= 1;
        //    } else {
        //        Debug.Log("pawnNotSpawned");
        //    }
        //}

        while (pawnsSpawned < numPawnsToSpawn) {
            Transform homeTransform = homeZones[pawnsSpawned].relaxZone.transform;

            if (WeightedPawnSpawn(homeTransform)) {
                pawnsSpawned++;
            }
        }
    }
    
    bool WeightedPawnSpawn(Transform t)
    {
        int chosenBase = 0;
        bool pawnSpawned = false;

        for (int i = 0; i < usedSubset.Length; i++)
        {
            int pawnRand = Random.Range(0, totalChance);
            if (pawnRand <= pawnSpawnThreshold[i])
            {
                chosenBase = i;
                classNumbers[i]++;
                //GameObject pawnToSpawn = Instantiate(usedSubset[0], t.position, t.rotation,pawnHolder);
                GameObject pawnToSpawn = Instantiate(usedSubset[chosenBase], t.position, t.rotation,pawnHolder);
                InitPawn pawnInit = pawnToSpawn.GetComponent<InitPawn>();
                pawnInit.workContainer = workContainer;
                pawnInit.SetRestZone(homeZones[pawnsSpawned].relaxZone.transform);
                pawnInit.SetSleepZone(homeZones[pawnsSpawned].sleepZone.transform);
                pawnInit.classType = i;
                pawnSpawned = true;
                break;
            }
        }

        return pawnSpawned;
    }
}

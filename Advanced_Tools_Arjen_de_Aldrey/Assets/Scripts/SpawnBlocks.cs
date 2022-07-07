using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocks : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    private Block[,] blocks;
    private List<Block> blocksList;
    [Range(1, 10000)][SerializeField] private int blockAmountWidth = 180;
    [Range(1, 10000)] [SerializeField] private int blockAmountHeight = 100;

    private float blockWidth;
    private float blockHeight;

    [SerializeField] private GameObject parentObject;
    private bool start;

    private float timePassed;
    [SerializeField] private float TimeInbetweenGenerations;

    [SerializeField] private NeighbourCheckingPreset preset;
    [SerializeField] private MapGenerationPreset spawningPreset;

    private DateTime timeBeforeTest;


    void Start()
    {
        blocksList = new List<Block>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (start) start = false;
            else start = true;
        }
        if (Input.GetKeyDown(KeyCode.R)) reset();
        if(start)
        {
            if (timePassed >= TimeInbetweenGenerations)
            {
                timePassed = 0;
                preset.checkBehaviour.CheckBlocks(blocksList, preset.ruleset);
            }
            else
            {
                timePassed += Time.deltaTime;
            }
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 100), "Generate squares"))
        {
            timeBeforeTest = DateTime.Now;
            blockWidth = Screen.width / (float)blockAmountWidth;
            blockHeight = Screen.height / (float)blockAmountHeight;
            spawningPreset.spawnRuleset.GenerateMap(blockWidth, blockHeight, blockAmountWidth, blockAmountHeight, blockPrefab, ref blocks, ref blocksList, parentObject.transform);
            DisplaySpawnTiming();
        }
    }
    
    private void DisplaySpawnTiming()
    {
        Debug.Log($"It took {(DateTime.Now - timeBeforeTest).TotalSeconds} seconds to generate the map.");
    }

    private void reset()
    {
        for(int i = 0;i<blocksList.Count;i++)
        {
            Destroy(blocksList[i].transform.gameObject);
        }
        start = false;
        blocksList.Clear();
        spawningPreset.spawnRuleset.GenerateMap(blockWidth, blockHeight, blockAmountWidth, blockAmountHeight, blockPrefab, ref blocks, ref blocksList, parentObject.transform);
    }
}

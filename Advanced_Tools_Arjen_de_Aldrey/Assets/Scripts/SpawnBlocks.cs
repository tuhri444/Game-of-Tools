using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocks : MonoBehaviour
{
    //GPU specific
    public static SpawnBlocks instance;
    [SerializeField] public ComputeShader GPUShader;
    [SerializeField] private GameObject renderQuad;

    private Material renderMat;
    public RenderTexture renderTarget;

    [SerializeField] private GameObject blockPrefab;
    private Block[,] blocks;
    private List<Block> blocksList;
    [Range(1, 1000)][SerializeField] private int blockAmountWidth = 180;
    [Range(1, 1000)] [SerializeField] private int blockAmountHeight = 100;

    private float blockWidth;
    private float blockHeight;

    [SerializeField] private GameObject parentObject;
    private bool start;

    private float timePassed;
    [SerializeField] private float TimeInbetweenGenerations;

    [SerializeField] private NeighbourCheckingPreset preset;
    [SerializeField] private MapGenerationPreset spawningPreset;

    private DateTime timeBeforeTest;
    private float timePassedUnScaled;
    private float averageFPS;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log($"Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    void Start()
    {
        blocksList = new List<Block>();

        renderTarget = new RenderTexture(blockAmountWidth, blockAmountHeight, 24);
        renderTarget.enableRandomWrite = true;
        renderTarget.Create();

        if (renderQuad == null) return;
        renderMat = renderQuad.GetComponent<MeshRenderer>().material;
        renderMat.SetTexture("_MainTex", renderTarget);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (start) start = false;
            else start = true;
        }
        if(start)
        {
            if (timePassed >= TimeInbetweenGenerations)
            {
                timePassed = 0;
                preset.checkBehaviour.CheckBlocks(blocksList, preset.ruleset, blockAmountWidth, blockAmountHeight);
            }
            else
            {
                timePassed += Time.deltaTime;
            }

            DisplayAverageFPSOverTime(10f);
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 50), "Generate squares"))
        {
            reset();
            timeBeforeTest = DateTime.Now;
            blockWidth = Screen.width / (float)blockAmountWidth;
            blockHeight = Screen.height / (float)blockAmountHeight;
            spawningPreset.spawnRuleset.GenerateMap(blockWidth, blockHeight, blockAmountWidth, blockAmountHeight, blockPrefab, ref blocks, ref blocksList, parentObject.transform);
            DisplaySpawnTiming();
        }
        if (GUI.Button(new Rect(10, 65, 200, 50), "Reset"))
        {
            reset();
        }
    }
    
    private void DisplaySpawnTiming()
    {
        Debug.Log($"It took {(DateTime.Now - timeBeforeTest).TotalSeconds} seconds to generate the map.");
    }

    private void DisplayAverageFPSOverTime(float overTimeInSeconds)
    {
        if (timePassedUnScaled > overTimeInSeconds)
        {
            Debug.Log($"The average FPS over 10 seconds is: {averageFPS * 0.1f} FPS");

            averageFPS = 0;
            timePassedUnScaled = 0;
            return;
        } else if (timePassedUnScaled > overTimeInSeconds*0.1)
        {
            averageFPS += 1.0f / Time.deltaTime;
        }
        timePassedUnScaled += Time.unscaledDeltaTime;
    }

    private void reset()
    {
        for(int i = 0;i<blocksList.Count;i++)
        {
            Destroy(blocksList[i].transform.gameObject);
        }
        start = false;
        blocksList.Clear();
    }
}

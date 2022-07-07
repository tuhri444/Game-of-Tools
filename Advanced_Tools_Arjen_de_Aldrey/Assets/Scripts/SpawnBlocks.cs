using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocks : MonoBehaviour
{
    //GPU specific
    public static SpawnBlocks instance;
    [SerializeField] public ComputeShader GPUShader;
    [SerializeField] public ComputeShader CreationShader;
    [SerializeField] public ComputeShader CopyShader;
    [SerializeField] private GameObject renderQuad;

    private Material renderMat;
    public RenderTexture renderTarget;
    public RenderTexture currentStates;

    [SerializeField] private GameObject blockPrefab;
    private Block[,] blocks;
    private List<Block> blocksList;
    [Range(1, 1400)][SerializeField] private int blockAmountWidth = 180;
    [Range(1, 1400)] [SerializeField] private int blockAmountHeight = 100;

    private float blockWidth;
    private float blockHeight;

    [SerializeField] private GameObject parentObject;
    private bool start;

    private float timePassed;
    [SerializeField] private float TimeInbetweenGenerations;

    [SerializeField] private NeighbourCheckingPreset preset;
    [SerializeField] private MapGenerationPreset spawningPreset;

    private DateTime timeBeforeTest;
    private float fpsChecked = 0;
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
        //blocksList = new List<Block>();

        currentStates = new RenderTexture(blockAmountWidth, blockAmountHeight, 24);
        currentStates.enableRandomWrite = true;
        currentStates.filterMode = FilterMode.Point;
        currentStates.Create();

        renderTarget = new RenderTexture(blockAmountWidth, blockAmountHeight, 24);
        renderTarget.enableRandomWrite = true;
        renderTarget.filterMode = FilterMode.Point;
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
                UpdateGrid();
                CopyGrid(renderTarget, currentStates);
                //preset.checkBehaviour.CheckBlocks(blocksList, preset.ruleset, blockAmountWidth, blockAmountHeight);
            }
            else
            {
                timePassed += Time.deltaTime;
            }

            DisplayAverageFPSOverTime(10f);
        }
        else
        {
            timeBeforeTest = DateTime.Now;
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 50), "Generate squares"))
        {
            CreateGrid();
            //reset();
            //timeBeforeTest = DateTime.Now;
            CopyGrid(currentStates, renderTarget);
            //blockWidth = Screen.width / (float)blockAmountWidth;
            //blockHeight = Screen.height / (float)blockAmountHeight;
            //spawningPreset.spawnRuleset.GenerateMap(blockWidth, blockHeight, blockAmountWidth, blockAmountHeight, blockPrefab, ref blocks, ref blocksList, parentObject.transform);
            //DisplaySpawnTiming();
        }
    }

    private void CreateGrid()
    {
        CreationShader.SetTexture(0, "Result", currentStates);
        CreationShader.SetInt("gridWidth", blockAmountWidth);
        CreationShader.SetInt("gridHeight", blockAmountHeight);
        CreationShader.Dispatch(0, blockAmountWidth* blockAmountHeight / 32, 1, 1);
    }
    
    private void CopyGrid(RenderTexture from, RenderTexture to)
    {
        CopyShader.SetTexture(0, "Original", from);
        CopyShader.SetTexture(0, "Result", to);
        CopyShader.SetInt("gridWidth", blockAmountWidth);
        CopyShader.SetInt("gridHeight", blockAmountHeight);
        CopyShader.Dispatch(0, blockAmountWidth * blockAmountHeight / 32, 1, 1);
    }

    private void UpdateGrid()
    {
        GPUShader.SetTexture(0, "Original", currentStates);
        GPUShader.SetTexture(0, "Result", renderTarget);
        GPUShader.SetInt("gridWidth", blockAmountWidth);
        GPUShader.SetInt("gridHeight", blockAmountHeight);
        GPUShader.Dispatch(0, blockAmountWidth * blockAmountHeight / 32, 1, 1);
    }
    
    private void DisplaySpawnTiming()
    {
        Debug.Log($"It took {(DateTime.Now - timeBeforeTest).TotalMilliseconds} milliseconds to generate the map.");
    }

    private void DisplayAverageFPSOverTime(float overTimeInSeconds)
    {
        if ((DateTime.Now - timeBeforeTest).Seconds > overTimeInSeconds)
        {
            Debug.Log($"The average FPS over 10 seconds is: {averageFPS * 0.1f} FPS");

            averageFPS = 0;
            fpsChecked = 0;
            timeBeforeTest = DateTime.Now;
            return;
        } else if ((DateTime.Now - timeBeforeTest).Seconds > overTimeInSeconds*0.1f * fpsChecked)
        {
            averageFPS += 1.0f / Time.deltaTime;
            fpsChecked++;
        }
    }

    private void reset()
    {
        //for(int i = 0;i<blocksList.Count;i++)
        //{
        //    Destroy(blocksList[i].transform.gameObject);
        //}
        start = false;
        //blocksList.Clear();

    }
}

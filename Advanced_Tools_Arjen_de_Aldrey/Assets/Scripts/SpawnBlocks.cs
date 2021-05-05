using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocks : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    private int screenWidth;
    private int screenHeight;
    private Block[,] blocks;
    private List<Block> blocksList;
    [Range(1,10)][SerializeField] private int blockSize;

    private int blockWidth;
    private int blockHeight;
    private int blockAmountHor;
    private int blockAmountVert;

    [SerializeField] private GameObject parentObject;
    private bool start;

    private float timePassed;
    [SerializeField] private float TimeInbetweenGenerations;

    [SerializeField] private StandardPreset preset;


    void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        blockWidth = 12* blockSize;
        blockHeight = 12* blockSize;
        blockAmountHor = screenWidth / blockWidth;
        blockAmountVert = screenHeight / blockHeight;
        blocksList = new List<Block>();
        generateMap();
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

    private void generateMap()
    {
        blocks = new Block[blockAmountHor, blockAmountVert+1];
        for(int i = 0;i < blockAmountVert; i++)
        {
            for (int j = 0; j < blockAmountHor; j++)
            {
                Vector2 pos = new Vector2(j*blockWidth+blockWidth*0.5f, i * blockHeight + blockHeight * 0.5f);
                blocks[j, i] = GameObject.Instantiate(blockPrefab, new Vector3(pos.x,pos.y,0),Quaternion.identity).GetComponent<Block>();
                blocks[j, i].GetComponent<RectTransform>().sizeDelta = new Vector2(blockWidth,blockHeight);
                blocks[j, i].transform.parent = parentObject.transform;
                int randomnumba = Random.Range(0, 4);
                //if (randomnumba == 1) blocks[j, i].alive = true;
                blocksList.Add(blocks[j, i]);
            }
        }
        for (int i = 0; i < blockAmountVert; i++)
        {
            for (int j = 0; j < blockAmountHor; j++)
            {
                //north
                if (i + 1 < blockAmountVert) blocks[j, i].North = blocks[j, i + 1];
                //south
                if(i - 1 >= 0) blocks[j, i].South = blocks[j, i - 1];
                //east
                if (j+1 < blockAmountHor) blocks[j, i].East = blocks[j+1, i];
                //west
                if (j-1 >= 0) blocks[j, i].West = blocks[j - 1, i];
                //north-west
                if (i + 1 < blockAmountVert && j - 1 >= 0) blocks[j, i].NorthW = blocks[j - 1, i + 1];
                //north-east
                if (i + 1 < blockAmountVert && j + 1 < blockAmountHor) blocks[j, i].NorthE = blocks[j + 1, i + 1];
                //south-west
                if (i - 1 >= 0 && j - 1 >= 0) blocks[j, i].SouthW = blocks[j - 1, i - 1];
                //south-east
                if (i - 1 >= 0 && j + 1 < blockAmountHor) blocks[j, i].SouthE = blocks[j + 1, i - 1];
            }
        }

    }

    private void reset()
    {
        for(int i = 0;i<blocksList.Count;i++)
        {
            Destroy(blocksList[i].transform.gameObject);
        }
        start = false;
        blocksList.Clear();
        generateMap();
    }
}

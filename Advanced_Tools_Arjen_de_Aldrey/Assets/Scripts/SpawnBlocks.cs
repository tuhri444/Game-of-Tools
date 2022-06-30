using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocks : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    private Block[,] blocks;
    private List<Block> blocksList;
    [Range(1,10000)][SerializeField] private int blockAmountWidth;
    [Range(1,10000)][SerializeField] private int blockAmountHeight;

    private float blockWidth;
    private float blockHeight;

    [SerializeField] private GameObject parentObject;
    private bool start;

    private float timePassed;
    [SerializeField] private float TimeInbetweenGenerations;

    [SerializeField] private StandardPreset preset;


    void Start()
    {
        blockWidth = Screen.width / (float)blockAmountWidth;
        blockHeight = Screen.height / (float)blockAmountHeight;
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
        blocks = new Block[blockAmountWidth, blockAmountHeight + 1];
        for(int i = 0;i < blockAmountHeight; i++)
        {
            for (int j = 0; j < blockAmountWidth; j++)
            {
                Vector2 pos = new Vector2(j*blockWidth+blockWidth*0.5f, i * blockHeight + blockHeight * 0.5f);
                blocks[j, i] = GameObject.Instantiate(blockPrefab, new Vector3(pos.x,pos.y,0),Quaternion.identity).GetComponent<Block>();
                blocks[j, i].GetComponent<RectTransform>().sizeDelta = new Vector2(blockWidth,blockHeight);
                blocks[j, i].transform.parent = parentObject.transform;
                //int randomnumba = Random.Range(0, 4);
                //if (randomnumba == 1) blocks[j, i].alive = true;
                blocksList.Add(blocks[j, i]);
            }
        }
        for (int i = 0; i < blockAmountHeight; i++)
        {
            for (int j = 0; j < blockAmountWidth; j++)
            {
                //north
                if (i + 1 < blockAmountHeight) blocks[j, i].North = blocks[j, i + 1];
                //south
                if(i - 1 >= 0) blocks[j, i].South = blocks[j, i - 1];
                //east
                if (j+1 < blockAmountWidth) blocks[j, i].East = blocks[j+1, i];
                //west
                if (j-1 >= 0) blocks[j, i].West = blocks[j - 1, i];
                //north-west
                if (i + 1 < blockAmountHeight && j - 1 >= 0) blocks[j, i].NorthW = blocks[j - 1, i + 1];
                //north-east
                if (i + 1 < blockAmountHeight && j + 1 < blockAmountWidth) blocks[j, i].NorthE = blocks[j + 1, i + 1];
                //south-west
                if (i - 1 >= 0 && j - 1 >= 0) blocks[j, i].SouthW = blocks[j - 1, i - 1];
                //south-east
                if (i - 1 >= 0 && j + 1 < blockAmountWidth) blocks[j, i].SouthE = blocks[j + 1, i - 1];
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

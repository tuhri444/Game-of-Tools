﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public Block North;
    public Block NorthW;
    public Block NorthE;
    public Block South;
    public Block SouthW;
    public Block SouthE;
    public Block East;
    public Block West;
    public bool alive;
    public int aliveNeighbours = 0;

    private Material spriteMaterial;

    private void Awake()
    {
        RawImage image = GetComponent<RawImage>();
        spriteMaterial = Instantiate(image.material);
        image.material = spriteMaterial;
    }
    private void Start()
    {
        ToggleAlive(alive);
    }

    public void checkNeighbours()
    {
        aliveNeighbours = 0;
        if (South != null && South.alive) aliveNeighbours++;
        if (North != null && North.alive) aliveNeighbours++;
        if (East != null && East.alive) aliveNeighbours++;
        if (West != null && West.alive) aliveNeighbours++;
        if (NorthW != null && NorthW.alive) aliveNeighbours++;
        if (NorthE != null && NorthE.alive) aliveNeighbours++;
        if (SouthW != null && SouthW.alive) aliveNeighbours++;
        if (SouthE != null && SouthE.alive) aliveNeighbours++;
    }

    public void ToggleAlive(bool isAlive)
    {
        alive = isAlive;
        if (alive)
            spriteMaterial.color = Color.white;
        else
            spriteMaterial.color = Color.black;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            alive = true;
            spriteMaterial.color = Color.white;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        alive = true;
        spriteMaterial.color = Color.white;
    }    
}

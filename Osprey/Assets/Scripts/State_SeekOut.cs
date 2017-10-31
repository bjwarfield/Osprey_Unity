using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_SeekOut : IState
{
    private LayerMask layer;
    private GameObject parent;
    private String searchTag;

    public State_SeekOut(LayerMask layer, GameObject parent, String searchTag)
    {
        this.layer = layer;
        this.parent = parent;
        this.searchTag = searchTag;
    }

    public void Start()
    {
        GameObject.FindGameObjectWithTag(searchTag);
    }

    public void Execute()
    {
        
    }

    public void Stop()
    {
        
    }
}

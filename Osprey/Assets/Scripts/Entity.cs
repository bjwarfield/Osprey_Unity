using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum polarType : int { LIGHT = 0, DARK = 1 };

public abstract class Entity: MonoBehaviour
{

    //utility vars
    
    public Transform thisTransform = null;
    [SerializeField]
    protected Renderer thisRenderer = null;
    [SerializeField]
    protected Texture[] textures;
    public GameObject gibs = null;


    //weapon vars
    public GameObject[] shot;
    public GameObject[] gunBarrel;
    public float shotFireRate = 25.0f;
    protected float lastShot;

    //status vars
    [SerializeField]
    protected polarType polarity { get; set; }
    
}

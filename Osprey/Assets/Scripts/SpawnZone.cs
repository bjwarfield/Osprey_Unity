using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour {

    public List<Entity> entitlyList;
    public float vertScroll = 0.1f;
    private bool spawned;
    public bool scrollAfterSpawn;

    // Use this for initialization
    void Start () {
        spawned = false;
    }

    public void Spawn()
    {
        
        foreach (Entity e in entitlyList)
        {
            e.gameObject.SetActive(true);
            e.Init();
            
        }
        spawned = true;
    }


    void Update () {
        if (!spawned || scrollAfterSpawn)
        {
            
            transform.Translate(Vector3.down * Time.deltaTime * vertScroll);

        }
        int activeEntities = 0;

        for(int i = 0; i < entitlyList.Count; i++)
        {
            if (entitlyList[i]) activeEntities++;
        }
        if(activeEntities <= 0)
        {
            Destroy(gameObject);
        }

    }
}

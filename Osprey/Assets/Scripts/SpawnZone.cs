using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour {

    public List<Entity> entitlyList;
    

	// Use this for initialization
	void Start () {
        foreach (Entity e in entitlyList)
        {
            e.gameObject.SetActive(true);
        }
    }

    private void OnBecameVisible()
    {
        
    }
    // Update is called once per frame
    void Update () {
		
	}
}

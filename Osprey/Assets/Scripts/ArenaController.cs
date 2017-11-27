using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Something has entered arena");
        SpawnZone zone = other.gameObject.GetComponent<SpawnZone>();
        if (zone)
        {
            //Debug.Log("Zone has entered arena");
            zone.Spawn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity e = other.gameObject.GetComponent<Entity>();

        if (e)
        {
            e.ExitArena();
        }
    }
    private void Update()
    {
    }
}

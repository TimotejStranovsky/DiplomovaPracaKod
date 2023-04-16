using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public int zoneID;
    public WorldBuilder worldBuilder;


    public void OnTriggerExit(Collider other)
    {
        //unload level
        //if potential new zone =/= exited zone then set players new zone
        Debug.Log("Boop" + zoneID);
    }
    public void OnTriggerEnter(Collider other)
    {
        //load level
        //set players potential new zone
        Debug.Log("Blep" + zoneID);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject levelObject;
    public int id;

    public void DestroyObject()
    {
        Destroy(levelObject);
    }
}

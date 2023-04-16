using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] Button ghost;
    private void Start()
    {
        ghost.onClick.AddListener(GenerateNewGhost);
        GenerateNewGhost();
    }

    private void GenerateNewGhost()
    {
        int idx = Random.Range(0, (spawnPoints.Length - 1));
        ghost.gameObject.transform.position = spawnPoints[idx].transform.position;
    }
}

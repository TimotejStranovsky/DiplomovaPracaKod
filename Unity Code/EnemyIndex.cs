using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndex : MonoBehaviour
{
    List<EnemyEntry> enemyEntries;

    public void SubmitEntry(string entryName)
    {
        if (enemyEntries == null)
            enemyEntries = new List<EnemyEntry>();
        EnemyEntry foundEntry = FindEntry(entryName);
        if(foundEntry == null)
        {
            AddEntry(new EnemyEntry(entryName, 1));
            return;
        }
        foundEntry.NumberOfCaught += 1;
    }

    private void AddEntry(EnemyEntry newEntry)
    {
        enemyEntries.Add(newEntry);
    }

    private EnemyEntry FindEntry(string entryName)
    {
        EnemyEntry foundEntry = enemyEntries.Find(elm => elm.Name == entryName);
        return foundEntry;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    foreach(var entry in enemyEntries)
        //    {
        //        Debug.Log(entry.ToString());
        //    }
        //}
    }
}
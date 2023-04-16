using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntry
{
    public string Name;
    public int NumberOfCaught;
    //public Image EnemyImage;

    public EnemyEntry(string pName, int pNOC)
    {
        Name = pName;
        NumberOfCaught = pNOC;
    }

    public override string ToString()
    {
        return Name + " " + NumberOfCaught;
    }
}